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
 
        //public LanguageCode Language{ get; set; }
   
        //public VoiceId Voice { get; set; }

        public string Language { get; set; }

        public string Voice { get; set; }
    }
}
