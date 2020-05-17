using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
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
            CheckForIllegalCrossThreadCalls = false;
            fileDownloadClient.DownloadProgressChanged += FileDownloadClient_DownloadProgressChanged;
            tmrStartup.Start();
            lblVersion.Text = "Updater Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            Point pos = Screen.PrimaryScreen.Bounds.Location + new Size(Screen.PrimaryScreen.Bounds.Width / 2,
                            Screen.PrimaryScreen.Bounds.Height / 2) - new Size(Bounds.Width / 2, Bounds.Height / 2);
            Location = pos;
        }

        private void FileDownloadClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int totalKB = (int) (e.TotalBytesToReceive / 1024);
            int currentKB = (int) (e.BytesReceived / 1024);
            SetStatus(
                $"Downloading.. {Math.Round(currentKB / 1024f, 2)} MB ({Math.Round(currentKB / (float) totalKB, 2)}%)",
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
                WebClient wc = new WebClient();
                Version v = Version.Parse(wc.DownloadString($"{Program.TargetURL}{Program.ProjectName}/newest.ver"));
                wc.Dispose();
                return v;
            });
        }

        private Task DownloadUpdate(Version version, out string tempFile)
        {
            tempFile = Path.GetRandomFileName();
            return fileDownloadClient.DownloadFileTaskAsync($"{Program.TargetURL}{Program.ProjectName}/{version}.zip",
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

                    zipArchiveEntry.ExtractToFile(Path.Combine(destinationDir, zipArchiveEntry.Name));
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

            if (v.Result > Program.CurrentVersion)
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

                if (waitTask.IsFaulted)
                {
                    throw waitTask.Exception.InnerException;
                }

                Task extractTask = ExtractUpdate(tempFile, Program.DestinationFolder, SetStatus);
                extractTask.Start();
                while (!extractTask.IsCompleted)
                {
                    Application.DoEvents();
                }

                if (extractTask.IsFaulted)
                {
                    throw extractTask.Exception.InnerException;
                }
            }

            ProcessStartInfo psi = new ProcessStartInfo(Program.DestinationFile, Program.Args.Unpack(" "));
            psi.WorkingDirectory = Program.DestinationFolder;
            Process.Start(psi);
            Close();
            Application.Exit();
        }

        private void WaitForProcessTask()
        {
            if (Program.WaitProcess != null)
            {
                pbProgress.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = $"Waiting for Process: {Program.WaitProcess.ProcessName}({Program.WaitProcess.Id})";
                while (!Program.WaitProcess.HasExited)
                {
                    Application.DoEvents();
                }
            }
        }
    }
}