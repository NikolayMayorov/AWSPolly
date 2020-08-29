using System;
using System.Collections.Generic;
using System.Text;
using VoiceWordAWS.Model;

namespace VoiceWordAWS.Sevices.Abstract
{
    public interface ISettingsService
    {
        void SaveSettings(Settings settings);
        Settings GetSettings();
    }
}
