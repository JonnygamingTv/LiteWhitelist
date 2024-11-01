using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteWhitelist.Cmds
{
    class Unwhitelist : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "litewhitelist.unwhitelist"
                };
            }
        }
        public string Name = "unwhitelist";
        public string Help => "Remove user from whitelist.";
        public string Syntax => "/whitelistremove [steam64id]";
        public List<string> Aliases => new List<string> { "whitelistremove", "dewhitelist" };
        string IRocketCommand.Name => "unwhitelist";
        public void Execute(IRocketPlayer caller, string[] command)
        {
            ulong id = 0;
            if(command.Length == 0 || !ulong.TryParse(command[0], out id))
            {
                UnturnedChat.Say(caller, "/whitelistremove [steam64id]", true);
                return;
            }
            if (LiteWhitelist.Instance.config.WhitelistedUsers.Contains(id))
            {
                LiteWhitelist.Instance.config.WhitelistedUsers.Remove(id);
                LiteWhitelist.Instance.SaveConfig();
                UnturnedChat.Say(caller, id.ToString() + " was removed from the whitelist.", true);
                return;
            }
            UnturnedChat.Say(caller, id.ToString() + " is not whitelisted.", true);
        }

    }
}
