using Exiled.API.Interfaces;

namespace ServerSupporterCommands
{
    public class PluginConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}