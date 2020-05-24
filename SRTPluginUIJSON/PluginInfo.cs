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

        public int VersionMajor => assemblyVersion.Major;

        public int VersionMinor => assemblyVersion.Minor;

        public int VersionBuild => assemblyVersion.Build;

        public int VersionRevision => assemblyVersion.Revision;

        private Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }
}
