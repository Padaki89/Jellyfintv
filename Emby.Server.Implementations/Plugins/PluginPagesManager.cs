using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Controller.Plugins;

namespace Emby.Server.Implementations.Plugins
{
    /// <inheritdoc/>
    public class PluginPagesManager : IPluginPagesManager
    {
        private List<PluginPage> m_pluginPages = new List<PluginPage>();

        /// <inheritdoc/>
        public IEnumerable<PluginPage> GetPages()
        {
            return m_pluginPages;
        }

        /// <inheritdoc/>
        public void RegisterPluginPage(PluginPage page)
        {
            if (m_pluginPages.Any(x => x.Id == page.Id))
            {
                // The page is already added
                // TODO: Log error
                return;
            }

            m_pluginPages.Add(page);
        }
    }
}
