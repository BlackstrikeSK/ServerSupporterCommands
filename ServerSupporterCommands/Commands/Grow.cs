using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ServerSupporterCommands.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Grow : ParentCommand
    {
        public Grow() => LoadGeneratedCommands();

        public override string Command { get; } = "grow";
        public override string[] Aliases { get; } = new string[] { };
        public override string Description { get; } = "Sizes the user to 1.1 on all axis.";

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get(((CommandSender)sender));

            if (!p.CheckPermission("pc.resize"))
            {
                response = "You are not allowed to use the grow command!";
                Player.Get((CommandSender)sender).SendConsoleMessage(response, "red");
                return false;
            }

            if (arguments.Count > 0)
            {
                response = "Too many arguments!";
                Player.Get((CommandSender)sender).SendConsoleMessage(response, "red");
                return false;
            }

            if (p.Role == RoleType.Spectator)
            {
                response = "You cannot use this command as a spectator!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            float xval = 1.1f;
            float yval = 1.1f;
            float zval = 1.1f;

            EventsHandler.SetPlayerScale(Player.Get((CommandSender)sender).GameObject, xval, yval, zval);

            response = "You have used the grow command! You have been scaled slightly taller than other human classes.";
            Player.Get((CommandSender)sender).SendConsoleMessage(response, "green");
            return true;
        }
    }
}