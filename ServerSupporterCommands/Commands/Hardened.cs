using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ServerSupporterCommands.Commands
{
    class Hardened : ParentCommand
    {
        public Hardened() => LoadGeneratedCommands();

        public override string Command => "hardened";
        public override string Description => "Gives damage reduction for 15 seconds.";
        public override string[] Aliases => new string[] { };

        public override void LoadGeneratedCommands()
        {
            
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get((CommandSender)sender);

            if(!p.CheckPermission("pc.hardened"))
            {
                response = "You are not allowed to use the hardened command!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            Plugin.Instance.EventsHandler.HardenedUses.TryGetValue(p, out int uses);
            Plugin.Instance.EventsHandler.HardenedCooldown.TryGetValue(p, out int CurrentCooldown);

            int cooldown = 15;

            if(uses >= 5)
            {
                response = "You have reached the command limit for the hardened command! Please wait until next round in order to use it again.";
                return true;
            }

            if(CurrentCooldown >= 1)
            {
                response = $"Please wait until the you can use this again. Cooldown remaining: {CurrentCooldown}";
                return true;
            }

            if (p.Role == RoleType.Spectator)
            {
                response = "You cannot use this command as a spectator!";
                p.SendConsoleMessage(response, "red");
                return true;
            }

            p.EnableEffect(Exiled.API.Enums.EffectType.DamageReduction, 15f, false);
            response = "";

            Plugin.Instance.EventsHandler.HardenedUses.Remove(p);
            Plugin.Instance.EventsHandler.HardenedUses.Add(p, uses);

            Plugin.Instance.EventsHandler.HardenedCooldown.Remove(p);
            Plugin.Instance.EventsHandler.HardenedCooldown.Add(p, cooldown);

            Plugin.Instance.EventsHandler.HardenedCountdown(p, cooldown);

            return true;
        }
    }
}
