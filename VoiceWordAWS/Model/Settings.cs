using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Polly;

namespace VoiceWordAWS.Model
{
  
    public class Settings
    {
   

        public string PathFolderAudio { get; set; }
      //  [JsonConverter(typeof(LanguageCode))]
        public LanguageCode Language{ get; set; }
    //    [JsonConverter(typeof(VoiceId))]
        public VoiceId Voice { get; set; }
    }
}
