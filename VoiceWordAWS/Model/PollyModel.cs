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
    class PollyModel
    {

        private void TestPolly()
        {
            BasicAWSCredentials awsCredentials = new BasicAWSCredentials("AKIAJQIYI4VDPPE43BEA", "akoHSIHS7otYQRNv42CTFkTtOgAtAdxxaYv8es0W");


            using (var client = new AmazonPollyClient(awsCredentials, Amazon.RegionEndpoint.USEast1))
            {
                var request = new Amazon.Polly.Model.SynthesizeSpeechRequest();
                request.Text = "Home sweet home";
                request.OutputFormat = OutputFormat.Mp3;
                request.LanguageCode = LanguageCode.EnUS;
                request.VoiceId = VoiceId.Astrid;
                var response = client.SynthesizeSpeechAsync(request).GetAwaiter().GetResult();


                string outpuName = "testName.mp3";
                FileStream output = File.Open(outpuName, FileMode.OpenOrCreate);
                response.AudioStream.CopyTo(output);
                output.Close();
                //  output.Flush();  //!!!!!!!
            }
        }
    }
}
