using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace LiteWhitelist
{
    public class LiteWhitelist : RocketPlugin
    {
        public Config config;
        public static LiteWhitelist Instance { get; private set; }
        Task Offload;

        protected override void Load()
        {
            Instance = this;
            config = Config.Load(Path.Combine(Directory, "Whitelist.json"));
            if (config.WhitelistEnabled)
            {
                Rocket.Unturned.U.Events.OnPlayerConnected += PC;
                Offload = Task.Run(() => StartCoroutine(nameof(loop)));
            }
            Rocket.Core.Logging.Logger.Log(config.WhitelistEnabled ? "Whitelist is enabled." : "Whitelist is disabled.");
            Rocket.Core.Logging.Logger.Log(config.WhitelistedUsers.Count.ToString() + " whitelisted players.");
            Rocket.Core.Logging.Logger.Log("Loaded.");
        }
        protected override void Unload()
        {
            if (config.WhitelistEnabled)
            {
                Rocket.Unturned.U.Events.OnPlayerConnected -= PC;
                Offload.Dispose();
            }
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

        private IEnumerator loop()
        {
            yield return new WaitForSeconds(config.QueueInterval);
            CheckQueue();
            StartCoroutine(nameof(loop));
        }
        void CheckQueue()
        {
            for (byte i = 0; i < SDG.Unturned.Provider.pending.Count; i++)
            {
                try
                {
                    if (SDG.Unturned.Provider.pending[i] == null || SDG.Unturned.Provider.pending[i].playerID.steamID == null) continue;
                    if (!config.WhitelistedUsers.Contains(SDG.Unturned.Provider.pending[i].playerID.steamID.m_SteamID))
                    {
                        SDG.Unturned.Provider.kick(SDG.Unturned.Provider.pending[i].playerID.steamID, config.DeniedMessage);
                    }
                }
                catch (System.Exception) { }
            }
        }

    }
}
