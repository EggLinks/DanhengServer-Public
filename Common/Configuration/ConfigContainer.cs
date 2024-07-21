using System.Collections.Generic;

namespace EggLink.DanhengServer.Configuration
{
    public class ConfigContainer
    {
        public HttpServerConfig HttpServer { get; set; } = new HttpServerConfig();
        public KeyStoreConfig KeyStore { get; set; } = new KeyStoreConfig();
        public GameServerConfig GameServer { get; set; } = new GameServerConfig();
        public PathConfig Path { get; set; } = new PathConfig();
        public DatabaseConfig Database { get; set; } = new DatabaseConfig();
        public ServerOption ServerOption { get; set; } = new ServerOption();
        public DownloadUrlConfig DownloadUrl { get; set; } = new DownloadUrlConfig();
        public MuipServerConfig MuipServer { get; set; } = new MuipServerConfig();
    }

    public class HttpServerConfig
    {
        public string PublicAddress { get; set; } = "server.example.com";
        public string BindAddress { get; set; } = "0.0.0.0";
        public int PublicPort { get; set; } = 443;
        public int BindPort { get; set; } = 60000;
        public bool BindUseSSL { get; set; } = false;
        public bool AccessUseSSL { get; set; } = false;

        public string GetDisplayAddress()
        {
            return (AccessUseSSL ? "https" : "http") + "://" + PublicAddress + ":" + PublicPort;
        }
        public string GetBindAddress()
        {
            return (BindUseSSL ? "https" : "http") + "://" + BindAddress + ":" + BindPort;
        }
    }

    public class KeyStoreConfig
    {
        public string KeyStorePath { get; set; } = "certificate.p12";
        public string KeyStorePassword { get; set; } = "123456";
    }

    public class GameServerConfig
    {
        public string PublicAddress { get; set; } = "server.example.com";
        public string BindAddress { get; set; } = "0.0.0.0";
        public uint PublicPort { get; set; } = 23301;
        public uint BindPort { get; set; } = 23301;
        public string GameServerId { get; set; } = "dan_heng";
        public string GameServerName { get; set; } = "DanhengServer";
        public string GameServerDescription { get; set; } = "A re-implementation of StarRail server";
        public int KcpInterval { get; set; } = 40;
        public string GetDisplayAddress()
        {
            return PublicAddress + ":" + PublicPort;
        }
    }

    public class PathConfig
    {
        public string ResourcePath { get; set; } = "Resources";
        public string ConfigPath { get; set; } = "Config";
        public string DatabasePath { get; set; } = "Config/Database";
        public string LogPath { get; set; } = "Logs";
        public string PluginPath { get; set; } = "Plugins";
        public string PluginConfigPath { get; set; } = "Plugins/Config";
    }

    public class DatabaseConfig
    {
        public string DatabaseType { get; set; } = "sqlite";
        public string DatabaseName { get; set; } = "danheng.db";
        public string MySqlHost { get; set; } = "127.0.0.1";
        public int MySqlPort { get; set; } = 3306;
        public string MySqlUser { get ; set; } = "root";
        public string MySqlPassword { get; set; } = "123456";
        public string MySqlDatabase { get; set; } = "danheng";
    }

    public class ServerOption
    {
        public int StartTrailblazerLevel { get; set; } = 1;
        public bool AutoUpgradeWorldLevel { get; set; } = true;
        public bool EnableMission { get; set; } = true;  // experimental
        public bool AutoLightSection { get; set; } = true;
        public string Language { get; set; } = "EN";
        public List<string> DefaultPermissions { get; set; } = ["*"];
        public ServerAnnounce ServerAnnounce { get; set; } = new ServerAnnounce();
        public ServerProfile ServerProfile { get; set; } = new ServerProfile();
        public bool AutoCreateUser { get; set; } = true;
        public bool SavePersonalDebugFile { get; set; } = false;
    }

    public class ServerAnnounce
    {
        public bool EnableAnnounce { get; set; } = true;
        public string AnnounceContent { get; set; } = "Welcome to danhengserver!";
    }

    public class ServerProfile
    {
        public string Name { get; set; } = "Server";
        public int Uid { get; set; } = 80;
        public string Signature { get; set; } = "Type /help for a list of commands";
        public int Level { get; set; } = 1;
        public int HeadIcon { get; set; } = 200105;
        public int ChatBubbleId { get; set; } = 220001;
        public int DisplayAvatarId { get; set; } = 1001;
        public int DisplayAvatarLevel { get; set; } = 1;
    }

    public class DownloadUrlConfig
    {
        public string? AssetBundleUrl { get; set; } = null;
        public string? ExResourceUrl { get; set; } = null;
        public string? LuaUrl { get; set; } = null;
        public string? IfixUrl { get; set; } = null;
    }

    public class MuipServerConfig
    {
        public string AdminKey { get; set; } = "None";
    }
}
