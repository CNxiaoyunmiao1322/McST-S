using System;
using System.IO;
using System.Text.Json;
using McST_S.Shared.Models;

namespace McST_S.Client.Services
{
    public class ConfigService
    {
        private const string ConfigFileName = "config.json";
        private const string AppFolderName = "McST-S";
        private readonly string _configPath;

        public ServerConfig Config { get; private set; }

        public ConfigService()
        {
            // 配置文件放在应用程序所在目录的 McST-S 子文件夹中
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var configDirectory = Path.Combine(appDirectory, AppFolderName);

            // 确保目录存在
            Directory.CreateDirectory(configDirectory);

            _configPath = Path.Combine(configDirectory, ConfigFileName);
            Config = new ServerConfig();
        }

        // 其余方法保持不变...
        public bool ConfigExists()
        {
            return File.Exists(_configPath);
        }

        public void LoadConfig()
        {
            if (!ConfigExists())
            {
                Config = new ServerConfig();
                return;
            }

            try
            {
                var json = File.ReadAllText(_configPath);
                Config = JsonSerializer.Deserialize<ServerConfig>(json) ?? new ServerConfig();
            }
            catch (Exception)
            {
                // 处理配置文件读取错误
                Config = new ServerConfig();
            }
        }

        public void SaveConfig()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(Config, options);
                File.WriteAllText(_configPath, json);
            }
            catch (Exception)
            {
                // 处理配置文件保存错误
            }
        }

        public bool ValidateConfig()
        {
            // 验证服务端地址格式
            if (string.IsNullOrWhiteSpace(Config.ServerUrl) ||
                !Uri.TryCreate(Config.ServerUrl, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return false;
            }

            // 验证版本号格式 (x.x.x.x)
            if (string.IsNullOrWhiteSpace(Config.CurrentVersion) ||
                !System.Text.RegularExpressions.Regex.IsMatch(Config.CurrentVersion, @"^\d+(\.\d+){3}$"))
            {
                return false;
            }

            return true;
        }
    }
}