﻿using System;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using DiscBotJSONManager;
//
//    var config = DiscBotConfig.FromJson(jsonString);

namespace DiscBotJSONManager
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GameDayMap
    {
        [JsonProperty("0")]
        public GameDayEntry Day0 { get; set; }

        [JsonProperty("1")]
        public GameDayEntry Day1 { get; set; }

        [JsonProperty("2")]
        public GameDayEntry Day2 { get; set; }

        [JsonProperty("3")]
        public GameDayEntry Day3 { get; set; }

        [JsonProperty("4")]
        public GameDayEntry Day4 { get; set; }

        [JsonProperty("5")]
        public GameDayEntry Day5 { get; set; }

        [JsonProperty("6")]
        public GameDayEntry Day6 { get; set; }

        [JsonProperty("active")]
        public long Active { get; set; } = 0;

    }
    public partial class GameDayEntry
    {
        [JsonProperty("label")]
        public string Label { get; set; } = "";

        [JsonProperty("profile")]
        public string Profile { get; set; } = "";
    }

    public partial class cloudLogConfigEntry
    {

        [JsonProperty("source")]
        public string Source { get; set; } = "/path/to/module";

        [JsonProperty("submit")]
        public string Submit { get; set; } = "cloudLogSubmit";

        [JsonProperty("init")]
        public string Init { get; set; } = "cloudLogInit";

        [JsonProperty("token")]
        public string Token { get; set; } = "authtokenhere";

        [JsonProperty("host")]
        public string Host { get; set; } = "websitehostnameorip";

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; } = "/path/to/submit";

        [JsonProperty("port")]
        public long Port { get; set; } = 443;

        [JsonProperty("enabled")]
        public long Enabled { get; set; } = 0;
    }
    public partial class DiscBotConfig
    {
        [JsonProperty("token")]
        public string Token { get; set; } = "";

        [JsonProperty("ownerID")]
        public string OwnerId { get; set; } = "anyone";

        [JsonProperty("ownerHandle")]
        public string OwnerHandle { get; set; } = "everyone";

        [JsonProperty("Channel")]
        public string Channel { get; set; } = "farms";

        [JsonProperty("GlobalChannel")]
        public string GlobalChannel { get; set; } = "allfarms";

        [JsonProperty("Quip")]
        public string Quip { get; set; } = "Something something something";

        [JsonProperty("Status")]
        public string Status { get; set; } = "Busy";

        [JsonProperty("Announcement")]
        public string Announcement { get; set; } = "As you wish";

        [JsonProperty("MEMUInstances")]
        public string MemuInstances { get; set; } = ".MemuHyperv/MemuHyperv.xml";

        [JsonProperty("MEMUPath")]
        public string MemuPath { get; set; } = "C:/Program Files/Microvirt/MEmu/MemuHyperv VMs";

        [JsonProperty("MEMUC")]
        public string Memuc { get; set; } = "C:/Program Files/Microvirt/MEmu/memuc.exe";

        [JsonProperty("GNBotSettings")]
        public string GnBotSettings { get; set; } = "Desktop/GNLauncher/settings.json";

        [JsonProperty("GNBotProfile")]
        public string GnBotProfile { get; set; } = "Desktop/GNLauncher/profiles/actions/LssBot/default.json";

        [JsonProperty("GNBotDir")]
        public string GnBotDir { get; set; } = "Desktop/GNLauncher/";

        [JsonProperty("GNBotLogMask")]
        public string GnBotLogMask { get; set; } = "log_{N}.txt";

        [JsonProperty("GNBotLogMain")]
        public string GnBotLogMain { get; set; } = "log_main.txt";

        [JsonProperty("DuplicateLog")]
        public string DuplicateLog { get; set; } = "LssSessions.log";

        [JsonProperty("process_main")]
        public long ProcessMainLog { get; set; } = 0;

        [JsonProperty("saveMyLogs")]
        public long SaveMyLogs { get; set; } = 0;

        [JsonProperty("checkforAPK")]
        public long CheckForAPK { get; set; } = 0;

        [JsonProperty("apkStart")]
        public string apkStartURL { get; set; } = "https://www.gnbots.com/apk";

        [JsonProperty("apkPath")]
        public string apkPath { get; set; } = "Last%20Shelter%20Survival/game.apk";

        [JsonProperty("apkDest")]
        public string apkDest { get; set; } = "./downloaded.apk";

        [JsonProperty("apkStatsFile")]
        public string apkStatsFile { get; set; } = "./apkstats.json";

        [JsonProperty("gatherCSV")]
        public string GatherCsv { get; set; } = "";

        [JsonProperty("BackupDir")]
        public string BackupDir { get; set; } = "Backup/";

        [JsonProperty("ConfigsDir")]
        public string ConfigsDir { get; set; } = "Configs/";

        [JsonProperty("screenshot")]
        public long Screenshot { get; set; } = 0;

        [JsonProperty("screenshotDir")]
        public string ScreenshotDir { get; set; } = "Screenshots/";

        [JsonProperty("nircmd")]
        public string Nircmd { get; set; } = "nircmd.exe";

        [JsonProperty("ffmpeg")]
        public string Ffmpeg { get; set; } = "ffmpeg.exe";

        [JsonProperty("Announce")]
        public long Announce { get; set; } = 1;

        [JsonProperty("debug")]
        public long Debug { get; set; } = 0;

        [JsonProperty("disabled")]
        public long Disabled { get; set; } = 0;

        [JsonProperty("processWatchTimer")]
        public long ProcessWatchTimer { get; set; } = 600;

        [JsonProperty("processLaunchDelay")]
        public long ProcessLaunchDelay { get; set; } = 30;

        [JsonProperty("offline")]
        public long Offline { get; set; } = 0;

        [JsonProperty("GNBotThreads")]
        public long GnBotThreads { get; set; } = 1;

        [JsonProperty("WatchThreads")]
        public long WatchThreads { get; set; } = 1;

        [JsonProperty("announceStatus")]
        public long AnnounceStatus { get; set; } = 60;

        [JsonProperty("postStatusScreenshots")]
        public long PostStatusScreenshots { get; set; } = 0;

        [JsonProperty("minimumCycleTime")]
        public long MinimumCycleTime { get; set; } = 120;

        [JsonProperty("activeProfile")]
        public string ActiveProfile { get; set; } = "default";

        [JsonProperty("SessionStore")]
        public string SessionStore { get; set; } = "off";

        [JsonProperty("Launcher")]
        public string Launcher { get; set; } = "GNLauncher.exe";

        [JsonProperty("StartLauncher")]
        public string StartLauncher { get; set; } = "-start";

        [JsonProperty("StopLauncher")]
        public string StopLauncher { get; set; } = "-close";

        [JsonProperty("processName")]
        public string ProcessName { get; set; } = "GNLauncher";

        [JsonProperty("memuProcessName")]
        public string MemuProcessName { get; set; } = "MEmuHeadless.exe";

        [JsonProperty("DupeLogMaxBytes")]
        public long DupeLogMaxBytes { get; set; } = 10485760;

        [JsonProperty("DupeLogMaxBytesTest")]
        public long DupeLogMaxBytesTest { get; set; } = 1024;

        [JsonProperty("MaxFailures")]
        public long MaxFailures { get; set; } = 8;

        [JsonProperty("FailureMinutes")]
        public long FailureMinutes { get; set; } = 1;

        [JsonProperty("prefix")]
        public string Prefix { get; set; } = "!";

        [JsonProperty("gametime")]
        public string Gametime { get; set; } = "Australia/Sydney";

        [JsonProperty("patternfile")]
        public string Patternfile { get; set; } = "patterns.json";

        [JsonProperty("reporting")]
        public string Reporting { get; set; } = "reporting.json";

        [JsonProperty("messages")]
        public string Messages { get; set; } = "messages.json";

        [JsonProperty("useGoogle")]
        public long UseGoogle { get; set; } = 0;

        [JsonProperty("googleSecretsFile")]
        public string GoogleSecretsFile { get; set; } = "credentials.json";

        [JsonProperty("googleTokenFile")]
        public string GoogleTokenFile { get; set; } = "token.json";

        [JsonProperty("googleSheetId")]
        public string GoogleSheetID { get; set; } = "";

        [JsonProperty("googleWorksheetName")]
        public string GoogleWorksheetName { get; set; } = "";

        [JsonProperty("googleAppName")]
        public string GoogleAppName { get; set; } = "";

        [JsonProperty("enableReboot")]
        public long enableReboot { get; set; } = 0;

        [JsonProperty("moveGNBotWindow")]
        public long[] MoveGNBotWindow { get; set; } = { 0, 0, 500, 500 };

        [JsonProperty("gameDayMap")]
        public GameDayMap GameDayMap { get; set; }

        [JsonProperty("cloudLogs")]
        public cloudLogConfigEntry cloudLogs { get; set; }

        [JsonProperty("GNBotRestartFullCycle")]
        public long GNBotRestartFullCycle { get; set; } = 0;
                
        [JsonProperty("GNBotRestartInterval")]
        public long GNBotRestartInterval { get; set; } = 0;

        [JsonProperty("manageActiveBasesTime")]
        public long ActiveBaseTimer { get; set; } = 0;

        [JsonProperty("killstop")]
        public long killstop { get; set; } = 0;

        [JsonProperty("PausedMaster")]
        public string PausedMaster { get; set; } = "paused";

        [JsonProperty("process")]
        public string[] Process { get; set; } = {
            "runtime",
            "modules",
            "errors",
            "dailies",
            "donation",
            "autoshield",
            "upgrades",
            "store",
            "gather"
         };

        [JsonProperty("watcherrors")]
        public string[] Watcherrors { get; set; } = {
            "authfailure",
            "starttimeout",
            "noexist",
            "notdefined",
            "failedwindow",
            "noinclude",
            "unexpected",
            "invalidparam",
            "waitingforqueue"
         };

        [JsonProperty("watcherrorthreshold")]
        public long Watcherrorthreshold { get; set; } = 0;

        [JsonProperty("autogenerated")]
        public string autogenerated { get; set; } = "Indeed it is";
    }

    public partial class DiscBotConfig
    {
        public static DiscBotConfig FromJson(string json) => JsonConvert.DeserializeObject<DiscBotConfig>(json, DiscBotJSONManager.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this DiscBotConfig self) => JsonConvert.SerializeObject(self, DiscBotJSONManager.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}

