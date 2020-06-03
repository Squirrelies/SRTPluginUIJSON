using SRTPluginBase;
using System;

namespace SRTPluginUIJSON
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "JSON Web Server";

        public string Description => "A JSON Web Server Endpoint for displaying game memory values using JSON.";

        public string Author => "Squirrelies";

        public Uri MoreInfoURL => new Uri("https://github.com/Squirrelies/SRTPluginUIJSON");

        public int VersionMajor => assemblyFileVersion.ProductMajorPart;

        public int VersionMinor => assemblyFileVersion.ProductMinorPart;

        public int VersionBuild => assemblyFileVersion.ProductBuildPart;

        public int VersionRevision => assemblyFileVersion.ProductPrivatePart;

        private System.Diagnostics.FileVersionInfo assemblyFileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }
}
