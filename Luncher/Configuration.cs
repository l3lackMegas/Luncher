﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Luncher
{
    public static class Configuration
    {
        public static readonly Dictionary<string, object> Main = new Dictionary<string, object>
        {
            {"lang", ""}
        };

        public static readonly Dictionary<string, object> Logging = new Dictionary<string, object>
        {
            {"enableGameLogging", true},
            {"useGamePrefix", true}
        };

        public static readonly Dictionary<string, object> Updates = new Dictionary<string, object>
        {
            {"checkVersionsUpdate", true},
            {"checkProgramUpdate", true},
            {"enableMinecraftUpdateAlerts", true}
        };

        public static readonly Dictionary<string, object> Resources = new Dictionary<string, object>
        {
            {"enableReconstruction", true},
            {"assetsDir", "${AppData}\\.minecraft\\assets\\"}
        };

        public static void Load()
        {
            try
            {
                var filename = Path.Combine(Program.Minecraft, "luncher", "configuration.cfg");
                if (File.Exists(filename))
                {
                    var text = File.ReadAllText(filename);
                    var jo = JObject.Parse(text);
                    Main["lang"] = jo["main"]["lang"].ToString();
                    Logging["enableGameLogging"] = Convert.ToBoolean(jo["logging"]["enableGameLogging"].ToString());
                    Logging["useGamePrefix"] = Convert.ToBoolean(jo["logging"]["useGamePrefix"].ToString());
                    Updates["checkVersionsUpdate"] = Convert.ToBoolean(jo["updates"]["checkVersionsUpdate"].ToString());
                    Updates["checkProgramUpdate"] = Convert.ToBoolean(jo["updates"]["checkProgramUpdate"].ToString());
                    Updates["enableMinecraftUpdateAlerts"] =
                        Convert.ToBoolean(jo["updates"]["enableMinecraftUpdateAlerts"].ToString());
                    Resources["enableReconstruction"] =
                        Convert.ToBoolean(jo["resources"]["enableReconstruction"].ToString());
                    Resources["assetsDir"] = jo["resources"]["assetsDir"].ToString();
                }
                else
                    SaveDefault();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Data);
                SaveDefault();
            }
        }

        private static void SaveDefault()
        {
            var mainObject = new JObject
                {
                    {"lang", (string) Main["lang"]},
                };
            var loggingObject = new JObject
                {
                    {"enableGameLogging", (bool) Logging["enableGameLogging"]},
                    {"useGamePrefix", (bool) Logging["useGamePrefix"]}
                };
            var updatesObject = new JObject
                {
                    {"checkVersionsUpdate", (bool) Updates["checkVersionsUpdate"]},
                    {"checkProgramUpdate", (bool) Updates["checkProgramUpdate"]},
                    {"enableMinecraftUpdateAlerts", (bool) Updates["enableMinecraftUpdateAlerts"]}
                };
            var resourcesObject = new JObject
                {
                    {"enableReconstruction", (bool) Resources["enableReconstruction"]},
                    {"assetsDir", (string) Resources["assetsDir"]}
                };
            var jo = new JObject
                {
                    {"main", mainObject},
                    {"logging", loggingObject},
                    {"updates", updatesObject},
                    {"resources", resourcesObject}
                };
            var path = Path.Combine(Program.Minecraft, "luncher");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "configuration.cfg"), jo.ToString());
        }
    }
}
