using System.IO;
using Newtonsoft.Json;
using VoiceWordAWS.Model;
using VoiceWordAWS.Sevices.Abstract;

namespace VoiceWordAWS.Sevices
{
    public class SettingsService : ISettingsService
    {
        private readonly string _path = "settings.json";

        public void SaveSettings(Settings settings)
        {
            var jsonString = JsonConvert.SerializeObject(value: settings);


            File.WriteAllText(path: _path, contents: jsonString);
        }

        public Settings GetSettings()
        {
            Settings settings = null;
            if (File.Exists(path: _path))
            {
                var json = File.ReadAllText(path: _path);
                settings = JsonConvert.DeserializeObject<Settings>(value: json);
            }


            return settings;
        }
    }
}