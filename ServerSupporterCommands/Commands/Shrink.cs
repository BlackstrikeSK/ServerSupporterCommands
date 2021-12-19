using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ServerSupporterCommands.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Shrink : ParentCommand
    {
        public Shrink() => LoadGeneratedCommands();

        public override string Command { get; } = "shrink";
        public override string[] Aliases { get; } = new string[] { };
        public override string Description { get; } = "Sizes the user to 0.8 on all axis";

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get(((CommandSender)sender));

            if (!p.CheckPermission("pc.resize"))
            {
                response = "You are not allowed to use the shrink command!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            if (arguments.Count > 0)
            {
                response = "Too many arguments!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            if (p.Role == RoleType.Spectator)
            {
                response = "You cannot use this command as a spectator!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            float xval = .9f;
            float yval = .9f;
            float zval = .9f;

            EventsHandler.SetPlayerScale(p.GameObject, xval, yval, zval);

            response = "You have used the shrink command! You have been scaled slightly shorter than other human classes.";
            p.SendConsoleMessage(response, "green");
            return true;
        }
    }
}