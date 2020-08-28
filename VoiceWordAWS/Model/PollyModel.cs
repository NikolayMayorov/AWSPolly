using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;

namespace VoiceWordAWS.Model
{
    public class PollyModel
    {

        private readonly BasicAWSCredentials awsCredentials;

     //   private  string pathFolder = String.Empty;

        public PollyModel(string accessKey, string secretKey)
        {
            // awsCredentials = new BasicAWSCredentials("AKIAJQIYI4VDPPE43BEA", "akoHSIHS7otYQRNv42CTFkTtOgAtAdxxaYv8es0W");
            awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
        }

        public void VoiсeWord(string word, string pathFolder)
        {
            using (var client = new AmazonPollyClient(awsCredentials, Amazon.RegionEndpoint.USEast1))
            {
                var request = new Amazon.Polly.Model.SynthesizeSpeechRequest();
                request.Text = word;
                request.OutputFormat = OutputFormat.Mp3;
                request.LanguageCode = LanguageCode.EnUS;
                request.VoiceId = VoiceId.Astrid;
                var response = client.SynthesizeSpeechAsync(request).GetAwaiter().GetResult();

                //string path = @"C:\SomeDir";
                string path = pathFolder;
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                string outpuName = pathFolder + "/" + word + ".mp3";
                FileStream output = File.Open(outpuName, FileMode.OpenOrCreate);
                response.AudioStream.CopyTo(output);
                output.Close();
                //  output.Flush();  //!!!!!!!
            }
        }

    }
}
