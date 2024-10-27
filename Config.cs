using Newtonsoft.Json;
using Rocket.API;
using System.Collections.Generic;
using System.IO;


namespace LiteWhitelist
{
    public class Config
    {
        public bool WhitelistEnabled { get; set; }
        public string DeniedMessage { get; set; }
        public HashSet<ulong> WhitelistedUsers { get; set; }
        public void LoadDefaults() {
			this.WhitelistEnabled = true;
			this.DeniedMessage = "Server is currently only allowing Donators and Staff";
			this.WhitelistedUsers = new HashSet<ulong>(11)
			{
				76561199015319396UL,
				76561198860356692UL,
				76561198122135022UL,
				76561198343617672UL,
				76561198282562527UL,
				76561198322584057UL,
				76561198796248492UL,
				76561199216254915UL,
				76561198446113409UL,
				76561199648915892UL,
				76561198162135741UL
			};
		}
        public static Config Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var config = new Config();
                config.LoadDefaults();
                File.WriteAllText(filePath, JsonConvert.SerializeObject(config, Formatting.Indented));
                return config;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Config>(json);
        }

        public void Save(string filePath)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
