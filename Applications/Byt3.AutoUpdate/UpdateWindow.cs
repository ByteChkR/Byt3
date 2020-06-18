﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.Utilities.FastString;

namespace Byt3.AutoUpdate
{
    public partial class UpdateWindow : Form
    {
        private readonly WebClient fileDownloadClient = new WebClient();

        public UpdateWindow()
        {
            InitializeComponent();

        }

        private void FileDownloadClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int totalKB = (int)(e.TotalBytesToReceive / 1024);
            int currentKB = (int)(e.BytesReceived / 1024);
            SetStatus(
                $"Downloading.. {Math.Round(currentKB / 1024f, 2)} MB ({Math.Round(100 * currentKB / (float)totalKB, 2)}%)",
                currentKB, totalKB); //Prevent overflow of integer by having slightly less granular progress percentage
        }

        private void SetStatus(string text, int process, int maxProcess)
        {
            pbProgress.Style = ProgressBarStyle.Blocks;
            pbProgress.Maximum = maxProcess;
            pbProgress.Value = process;
            lblStatus.Text = text;
        }

        private Task<Version> GetNewestVersion()
        {
            return new Task<Version>(() =>
            {
                if (AutoUpdateEntry.TargetVersion != null)
                {
                    return AutoUpdateEntry.TargetVersion;
                }

                WebClient wc = new WebClient();
                List<Version> vers = GetAllVersions();
                return vers.Last();
                //Version v = Version.Parse(wc.DownloadString($"{Program.TargetURL}{Program.ProjectName}/newest.ver"));
                //wc.Dispose();
                //return v;
            });
        }

        private List<Version> GetAllVersions()
        {
            WebClient wc = new WebClient();
            string[] vers = wc.DownloadString($"{AutoUpdateEntry.TargetURL}{AutoUpdateEntry.ProjectName}/listing.php")
                .Split(' ').Where(x => x.EndsWith(".zip")).Select(ver => ver.Replace(".zip", "")).ToArray();
            wc.Dispose();

            List<Version> versions = new List<Version>();
            for (int i = 0; i < vers.Length; i++)
            {
                try
                {
                    versions.Add(Version.Parse(vers[i]));
                }
                catch (Exception e)
                {
                }
            }
            versions.Sort();
            return versions;
        }

        private Task DownloadUpdate(Version version, out string tempFile)
        {
            tempFile = Path.GetRandomFileName();
            return fileDownloadClient.DownloadFileTaskAsync($"{AutoUpdateEntry.TargetURL}{AutoUpdateEntry.ProjectName}/{version}.zip",
                tempFile);
            //return new Task<string>(() =>
            //{
            //    string tempFile = Path.GetRandomFileName();
            //    fileDownloadClient.DownloadFile($"{Program.TargetURL}{Program.ProjectName}/{version}.zip", tempFile);
            //    return tempFile;
            //});
        }

        private Task ExtractUpdate(string archiveFile, string targetDir, Action<string, int, int> waitAction)
        {
            return new Task(() =>
            {
                ZipArchive file = ZipFile.OpenRead(archiveFile);

                for (int i = 0; i < file.Entries.Count; i++)
                {
                    ZipArchiveEntry zipArchiveEntry = file.Entries[i];
                    string destinationDir = Path.Combine(targetDir, Path.GetDirectoryName(zipArchiveEntry.FullName));
                    if (!Directory.Exists(destinationDir))
                    {
                        Directory.CreateDirectory(destinationDir);
                    }


                    waitAction?.Invoke($"[{i + 1}/{file.Entries.Count}] Extracting File: {zipArchiveEntry.Name}", i + 1,
                        file.Entries.Count);
                    if (File.Exists(Path.Combine(destinationDir, zipArchiveEntry.Name)))
                    {
                        File.Delete(Path.Combine(destinationDir, zipArchiveEntry.Name));
                    }
                    try
                    {
                        zipArchiveEntry.ExtractToFile(Path.Combine(destinationDir, zipArchiveEntry.Name));
                    }
                    catch (Exception e)
                    {
                    }
                }

                file.Dispose();
                File.Delete(archiveFile);
            });
        }

        private void tmrStartup_Tick(object sender, EventArgs e)
        {
            tmrStartup.Stop();
            lblStatus.Text = "Checking for updates.";
            Task<Version> v = GetNewestVersion();
            v.Start();
            while (!v.IsCompleted)
            {
                Application.DoEvents();
            }

            if (v.IsFaulted)
            {
                throw v.Exception.InnerException;
            }

            if (v.Result > AutoUpdateEntry.CurrentVersion &&
                (AutoUpdateEntry.Direct ||
                    MessageBox.Show(
                     $"Update Found!\n\tOld Version: {AutoUpdateEntry.CurrentVersion}\n\tNew Version: {v.Result}\nUpdate Now?",
                     "Update Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                Task d = DownloadUpdate(v.Result, out string tempFile);
                //d.Start();
                while (!d.IsCompleted)
                {
                    Application.DoEvents();
                }

                Task waitTask = new Task(WaitForProcessTask);
                waitTask.Start();
                while (!waitTask.IsCompleted)
                {
                    Application.DoEvents();
                }

                Thread.Sleep(1000); //Give some time to drop filehandles etc..

                if (waitTask.IsFaulted)
                {
                    throw waitTask.Exception.InnerException;
                }

                RunExtractTask(tempFile);
            }

            if (AutoUpdateEntry.StartAfter == AutoUpdateEntry.StartAction.StartProduct)
            {
                ProcessStartInfo psi = new ProcessStartInfo(AutoUpdateEntry.DestinationFile, AutoUpdateEntry.Args.Unpack(" "));
                psi.WorkingDirectory = AutoUpdateEntry.DestinationFolder;
                Process.Start(psi);
            }
            else if (AutoUpdateEntry.StartAfter == AutoUpdateEntry.StartAction.OpenFolder)
            {
                ProcessStartInfo psi = new ProcessStartInfo(Path.GetDirectoryName(AutoUpdateEntry.DestinationFile), AutoUpdateEntry.Args.Unpack(" "));
                psi.WorkingDirectory = AutoUpdateEntry.DestinationFolder;
                Process.Start(psi);
            }

            if (AutoUpdateEntry.CloseOnFinish)
            {
                Application.Exit();
            }

            Close();

        }

        private void RunExtractTask(string file)
        {
            Task extractTask = ExtractUpdate(file, AutoUpdateEntry.DestinationFolder, SetStatus);
            extractTask.Start();
            while (!extractTask.IsCompleted)
            {
                Application.DoEvents();
            }

            if (extractTask.IsFaulted)
            {
                DialogResult res = MessageBox.Show(extractTask.Exception.ToString(), "Error", MessageBoxButtons.RetryCancel);
                if (res == DialogResult.Cancel)
                {
                    throw extractTask.Exception;
                    Application.Exit();
                    return;
                }
                else
                {
                    RunExtractTask(file);
                }
            }
        }

        private void WaitForProcessTask()
        {
            if (AutoUpdateEntry.WaitProcess != null && !AutoUpdateEntry.WaitProcess.HasExited)
            {
                pbProgress.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = $"Waiting for Process: {AutoUpdateEntry.WaitProcess.ProcessName}({AutoUpdateEntry.WaitProcess.Id})";
                while (!AutoUpdateEntry.WaitProcess.HasExited)
                {
                    Application.DoEvents();
                }
            }
        }

        private void UpdateWindow_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            fileDownloadClient.DownloadProgressChanged += FileDownloadClient_DownloadProgressChanged;
            tmrStartup.Start();
            lblVersion.Text = "Updater Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            Point pos = Screen.PrimaryScreen.Bounds.Location + new Size(Screen.PrimaryScreen.Bounds.Width / 2,
                            Screen.PrimaryScreen.Bounds.Height / 2) - new Size(Bounds.Width / 2, Bounds.Height / 2);
            Location = pos;
        }
    }
}