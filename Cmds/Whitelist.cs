using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteWhitelist.Cmds
{
    class Whitelist : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "litewhitelist.whitelist"
                };
            }
        }
        public string Name = "whitelist";
        public string Help => "Add user to whitelist.";
        public string Syntax => "/whitelistadd [steam64id]";
        public List<string> Aliases => new List<string> { "whitelistadd" };
        string IRocketCommand.Name => "whitelist";
        public void Execute(IRocketPlayer caller, string[] command)
        {
            ulong id = 0;
            if(command.Length == 0 || !ulong.TryParse(command[0], out id))
            {
                UnturnedChat.Say(caller, "/whitelistadd [steam64id]", true);
                return;
            }
            if (!LiteWhitelist.Instance.config.WhitelistedUsers.Contains(id))
            {
                LiteWhitelist.Instance.config.WhitelistedUsers.Add(id);
                LiteWhitelist.Instance.SaveConfig();
                UnturnedChat.Say(caller, id.ToString() + " was added to whitelist.", true);
                return;
            }
            UnturnedChat.Say(caller, id.ToString() + " was already added to whitelist.", true);
        }

    }
}
