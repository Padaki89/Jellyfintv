#pragma warning disable CS1591

using System;

namespace MediaBrowser.Common.Plugins
{
    public interface IPluginAssembly
    {
        void SetAttributes(string assemblyFilePath, string dataFolderPath, Version assemblyVersion, string imageUrl);

        void SetId(Guid assemblyId);
    }
}
