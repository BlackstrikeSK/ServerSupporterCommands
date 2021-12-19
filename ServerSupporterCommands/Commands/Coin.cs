using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ServerSupporterCommands.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Coin : ParentCommand
    {
        public Coin() => LoadGeneratedCommands();

        public override string Command { get; } = "coin";
        public override string[] Aliases { get; } = new string[] { };
        public override string Description { get; } = "Coin Command";

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get((CommandSender)sender);
            if (!p.CheckPermission("pc.coin"))
            {
                response = "You are not allowed to use the coin command!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            if (p.Role == RoleType.Spectator)
            {
                response = "You cannot use this command as a spectator!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            Plugin.Instance.EventsHandler.CoinUses.TryGetValue(p, out int value);

            if(value >= 5)
            {
                response = "You have reached the command limit for the coin command! Please wait until next round in order to use it again.";
                p.SendConsoleMessage(response, "red");
                return true;
            }

            p.AddItem(ItemType.Coin);
            response = "You have used the coin command! You have been given 1 coin.";
            p.SendConsoleMessage(response, "green");

            value++;
            Plugin.Instance.EventsHandler.CoinUses.Remove(p);
            Plugin.Instance.EventsHandler.CoinUses.Add(p, value);
            return true;
        }
    }
}