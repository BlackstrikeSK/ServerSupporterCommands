using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ServerSupporterCommands.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Speed : ParentCommand
    {
        public Speed() => LoadGeneratedCommands();

        public override string Command { get; } = "speed";
        public override string[] Aliases { get; } = new string[] { };
        public override string Description { get; } = "Gives the player the Invigorated effect for 15 seconds.";

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get((CommandSender)sender);
            if (!p.CheckPermission("pc.speed"))
            {
                response = "You are not allowed to use the speed command!";
                p.SendConsoleMessage(response, "red");
                return false;
            }
            Plugin.Instance.EventsHandler.SpeedUses.TryGetValue(p, out int limit);
            Plugin.Instance.EventsHandler.SpeedCooldown.TryGetValue(p, out int CurrentCooldown);

            int cooldown = 15;

            if (limit >= 5)
            {
                response = "You have reached the command limit for the speed command! Please wait until next round in order to use it again.";
                p.SendConsoleMessage(response, "red");
                return true;
            }

            if (p.Role == RoleType.Spectator)
            {
                response = "You cannot use this command as a spectator!";
                p.SendConsoleMessage(response, "red");
                return true;
            }

            if(CurrentCooldown >= 1)
            {
                response = $"Please wait until the you can use this again. Cooldown remaining: {CurrentCooldown}";
                p.SendConsoleMessage(response, "red");
                return true;
            }

            p.EnableEffect(Exiled.API.Enums.EffectType.Invigorated, 15f, false);
            p.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost, 15f, false);

            response = "You have used the speed command! You have been given the invigorated effect for 15 seconds.";
            p.SendConsoleMessage(response, "green");

            limit++;

            Plugin.Instance.EventsHandler.SpeedUses.Remove(p);
            Plugin.Instance.EventsHandler.SpeedUses.Add(p, limit);


            Plugin.Instance.EventsHandler.SpeedCooldown.Remove(p);
            Plugin.Instance.EventsHandler.SpeedCooldown.Add(p, cooldown);

            Plugin.Instance.EventsHandler.SpeedCountdown(p, cooldown);

            return true;
        }
    }
}