using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace PeeChat
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        /// <summary>
        /// Which chat channel should be activated upon character login.
        /// </summary>
        public ChatChannel DefaultChatChannel { get; set; } = ChatChannel.None;

        // the below exist just to make saving less cumbersome

        [NonSerialized]
        private DalamudPluginInterface pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface.SavePluginConfig(this);
        }
    }
}
