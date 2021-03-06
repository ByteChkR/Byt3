﻿using System.IO;

namespace Byt3.Engine.BuildTools.PackageCreator.Versions
{
    /// <summary>
    /// Interface of a Package Version
    /// </summary>
    public interface IPackageVersion
    {
        string ManifestPath { get; }
        string PackageVersion { get; }

        bool IsVersion(string path);

        void CreateGamePackage(string packageName, string executable, string outputFile, string workingDir,
            string[] files, string version);

        void CreateEnginePackage(string outputFile, string workingDir, string[] files, string version = null);

        void UnpackPackage(string file, string outPutDir);
        IPackageManifest GetPackageManifest(string path);

        IPackageManifest ReadManifest(Stream s);

        void WriteManifest(Stream s, IPackageManifest manifest);
    }
}