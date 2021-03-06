﻿using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace Byt3.Engine.BuildTools.PackageCreator.Versions.v1
{
    /// <summary>
    /// Package Version 1
    /// </summary>
    public class Version1 : IPackageVersion
    {
        public string ManifestPath => "PackageManifest.xml";
        public string PackageVersion => "v1";

        public void WriteManifest(Stream s, IPackageManifest manifest)
        {
            XmlSerializer xs = new XmlSerializer(typeof(PackageManifest));
            xs.Serialize(s, manifest);
        }

        public void UnpackPackage(string file, string outPutDir)
        {
            ZipFile.ExtractToDirectory(file, outPutDir);
            File.Delete(outPutDir + "/" + ManifestPath);
            if (Directory.Exists(outPutDir + "/patches"))
            {
                Creator.ApplyPatches(outPutDir, PackageVersion);
            }
        }

        public bool IsVersion(string path)
        {
            TextReader tr = null;
            Stream s = null;
            string v = "";
            ZipArchive pack = null;
            try
            {
                pack = ZipFile.OpenRead(path);
                s = pack.GetEntry(ManifestPath).Open();
                tr = new StreamReader(s);
                XmlSerializer xs = new XmlSerializer(typeof(PackageManifestHeader));
                PackageManifestHeader pm = (PackageManifestHeader) xs.Deserialize(tr);
                v = pm.PackageVersion;
            }
            catch (Exception)
            {
                tr?.Close();
                pack?.Dispose();
                return false;
            }


            pack.Dispose();
            tr.Close();
            return v == PackageVersion;
        }

        public IPackageManifest ReadManifest(Stream s)
        {
            XmlSerializer xs = new XmlSerializer(typeof(PackageManifest));
            PackageManifest pm = (PackageManifest) xs.Deserialize(s);
            s.Close();
            return pm;
        }


        public IPackageManifest GetPackageManifest(string path)
        {
            ZipArchive pack = ZipFile.OpenRead(path);
            Stream s = pack.GetEntry(ManifestPath).Open();
            PackageManifest pm = (PackageManifest) ReadManifest(s);
            pack.Dispose();
            return pm;
        }

        public void CreateGamePackage(string packageName, string executable, string outputFile, string workingDir,
            string[] files, string version)
        {
            File.WriteAllBytes(outputFile,
                new byte[] {80, 75, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
            FileStream fs = new FileStream(outputFile, FileMode.Open);
            ZipArchive a = new ZipArchive(fs, ZipArchiveMode.Update);
            Uri wdir = new Uri(workingDir);
            string wdirname = Path.GetFileName(workingDir);
            foreach (string file in files)
            {
                Uri f = new Uri(file);
                string fname = wdir.MakeRelativeUri(f).ToString().Remove(0, wdirname.Length + 1);
                ZipArchiveEntry e = a.CreateEntry(fname);

                byte[] content = File.ReadAllBytes(file);
                Stream s = e.Open();
                s.Write(content, 0, content.Length);
                s.Close();
            }

            ZipArchiveEntry engVersion = a.CreateEntry(ManifestPath);
            Stream str = engVersion.Open();
            PackageManifest pm = new PackageManifest(packageName, executable, version);
            Creator.WriteManifest(str, pm);
            str.Close();
            a.Dispose();
            fs.Close();
        }

        public void CreateEnginePackage(string outputFile, string workingDir, string[] files, string version = null)
        {
            File.WriteAllBytes(outputFile,
                new byte[] {80, 75, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
            FileStream fs = new FileStream(outputFile, FileMode.Open);

            ZipArchive a = new ZipArchive(fs, ZipArchiveMode.Update);
            Uri wdir = new Uri(workingDir);
            string wdirname = Path.GetFileName(workingDir);
            if (string.IsNullOrEmpty(version))
            {
                version = Creator.GetEngineVersion(workingDir);
            }

            foreach (string file in files)
            {
                Uri f = new Uri(file);
                string fname = wdir.MakeRelativeUri(f).ToString().Remove(0, wdirname.Length + 1);
                ZipArchiveEntry e = a.CreateEntry(fname);

                byte[] content = File.ReadAllBytes(file);
                Stream s = e.Open();
                s.Write(content, 0, content.Length);
                s.Close();
            }

            ZipArchiveEntry engVersion = a.CreateEntry(ManifestPath);
            Stream str = engVersion.Open();
            PackageManifest pm = new PackageManifest("Engine", "", version);
            Creator.WriteManifest(str, pm);
            str.Close();
            a.Dispose();
            fs.Close();
        }
    }
}