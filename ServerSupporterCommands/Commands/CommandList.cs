using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ServerSupporterCommands.Commands
{
    internal class CommandList : ParentCommand
    {
        public CommandList() => LoadGeneratedCommands();

        public override string Command => "commandlist";

        public override string[] Aliases => new string[] { };

        public override string Description => "Gives you the list of all commands from Server Supporter Commands.";

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get((CommandSender)sender);
            if (!p.CheckPermission("pc.commandlist"))
            {
                response = "Insufficient Perms!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            if (arguments.Count >= 0)
            {
                response = "Too many arguments!";
                p.SendConsoleMessage(response, "red");
                return false;
            }

            response = "<size=30><b>[•] SUPPORTER COMMAND LIST\n[»] /alive\n[»] /speed\n[»] /coin\n[»] /grow\n[»] /shrink\n[»] /commandlist\n[»] /hardened</b></size>";
            p.SendConsoleMessage(response, "green");

            return true;
        }
    }
}