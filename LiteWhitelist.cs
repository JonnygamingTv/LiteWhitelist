using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using System.IO;
using UnityEngine;

namespace LiteWhitelist
{
    public class LiteWhitelist : RocketPlugin
    {
        public Config config;
        public static LiteWhitelist Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;
            config = Config.Load(Path.Combine(Directory, "Whitelist.json"));
            if (config.WhitelistEnabled)
            {
                Rocket.Unturned.U.Events.OnPlayerConnected += PC;
            }
            Rocket.Core.Logging.Logger.Log(config.WhitelistEnabled ? "Whitelist is enabled." : "Whitelist is disabled.");
            Rocket.Core.Logging.Logger.Log(config.WhitelistedUsers.Count.ToString()+" whitelisted players.");
            Rocket.Core.Logging.Logger.Log("Loaded.");
        }
        protected override void Unload()
        {
            if (config.WhitelistEnabled) Rocket.Unturned.U.Events.OnPlayerConnected -= PC;
            Rocket.Core.Logging.Logger.Log("Unloaded.");
        }
        private void PC(UnturnedPlayer player)
        {
            if (!config.WhitelistedUsers.Contains(player.CSteamID.m_SteamID))
            {
                SDG.Unturned.Provider.kick(player.CSteamID, config.DeniedMessage);
            }
        }

        public void SaveConfig()
        {
            config.Save(Path.Combine(Directory, "Whitelist.json"));
        }
    }
}
