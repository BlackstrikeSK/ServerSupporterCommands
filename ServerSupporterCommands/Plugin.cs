using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using Server = Exiled.Events.Handlers.Server;

namespace ServerSupporterCommands
{
    public class Plugin : Plugin<PluginConfig>
    {
        public override string Author => "BlackstrikeSK";
        public override string Name => "ServerSupporterCommands";
        public override PluginPriority Priority => PluginPriority.Default;
        public override string Prefix => "PC";
        public override Version RequiredExiledVersion => new Version(3, 0, 0);
        public override Version Version => new Version(3, 0, 0);

        public static Plugin Instance { get; private set; }

        public EventsHandler EventsHandler;

        public override void OnEnabled()
        {
            Instance = this;
            EventsHandler = new EventsHandler();
            Server.RestartingRound += EventsHandler.OnRestartingRound;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventsHandler = null;
            Server.RestartingRound -= EventsHandler.OnRestartingRound;
            base.OnDisabled();
        }
    }
}