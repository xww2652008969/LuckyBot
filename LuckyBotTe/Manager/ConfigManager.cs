using System.Text;

namespace LuckyBotTe.Manager;

public static class ConfigManager
{
    private static string BasePath;
    private static string DataPath;
    private static string ConfigPath;

    public static void Init()
    {
        BasePath = AppDomain.CurrentDomain.BaseDirectory;
        DataPath = Path.Combine(BasePath, "data", "plugindata");
        ConfigPath = Path.Combine(BasePath, "data", "pluginconfig");
        try
        {
            Directory.CreateDirectory(DataPath);
            Directory.CreateDirectory(ConfigPath);
        }
        catch (IOException e)
        {
            throw new Exception("创建目录失败", e);
        }
    }

    public static PluginPath CreatePluginPath(string pluginname)
    {
        var c = Path.Combine(ConfigPath, pluginname);
        var d = Path.Combine(DataPath, pluginname);
        try
        {
            Directory.CreateDirectory(c);
            Directory.CreateDirectory(d);
        }
        catch (IOException e)
        {
            Console.WriteLine($"[ERROR] {e}");
        }

        return new PluginPath(c, d);
    }

    public class PluginPath(string configPath, string dataPath)
    {
        public string ConfigPath { get; } = configPath;
        public string DataPath { get; } = dataPath;

        public string? ReadConfigString(string filename)
        {
            var path = Path.Combine(ConfigPath, filename);
            try
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }
            catch (IOException)
            {
                return null;
            }
        }

        public byte[]? ReadConfigBytes(string filename)
        {
            var path = Path.Combine(ConfigPath, filename);
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (IOException)
            {
                return null;
            }
        }

        public string? ReadDataString(string filename)
        {
            var path = Path.Combine(DataPath, filename);
            try
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }
            catch (IOException)
            {
                return null;
            }
        }

        public byte[]? ReadDataBytes(string filename)
        {
            var path = Path.Combine(DataPath, filename);
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (IOException)
            {
                return null;
            }
        }

        public bool WriteConfigBytes(string filename, byte[] data)
        {
            var path = Path.Combine(ConfigPath, filename);
            try
            {
                File.WriteAllBytes(path, data);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool WriteDataBytes(string filename, byte[] data)
        {
            var path = Path.Combine(DataPath, filename);
            try
            {
                File.WriteAllBytes(path, data);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool WriteConfigString(string filename, string data)
        {
            var path = Path.Combine(ConfigPath, filename);
            try
            {
                File.WriteAllText(path, data, Encoding.UTF8);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool WriteDataString(string filename, string data)
        {
            var path = Path.Combine(DataPath, filename);
            try
            {
                File.WriteAllText(path, data, Encoding.UTF8);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
