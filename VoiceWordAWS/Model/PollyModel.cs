using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;

namespace VoiceWordAWS.Model
{
    public class PollyModel
    {

        private readonly BasicAWSCredentials awsCredentials;


        public PollyModel(string accessKey, string secretKey)
        {
            awsCredentials = new BasicAWSCredentials(accessKey: accessKey, secretKey: secretKey);
        }

        public void VoiсeWord(string word, string pathFolder, string voiceID, string langId)
        {
            using (var client = new AmazonPollyClient(credentials: awsCredentials, region: RegionEndpoint.USEast1))

            {
                var request = new SynthesizeSpeechRequest();
                request.Text = word;
                request.OutputFormat = OutputFormat.Mp3;
                request.LanguageCode = langId;
                request.VoiceId = voiceID;
                var resp = client.DescribeVoicesAsync(new DescribeVoicesRequest()).GetAwaiter().GetResult();


                var resp2 = client.ListLexiconsAsync(new ListLexiconsRequest()).GetAwaiter().GetResult();


                var resp5 = client.ListSpeechSynthesisTasksAsync(new ListSpeechSynthesisTasksRequest()).GetAwaiter()
                    .GetResult();


                var response = client.SynthesizeSpeechAsync(request: request).GetAwaiter().GetResult();


                var path = pathFolder;
                var dirInfo = new DirectoryInfo(path: path);
                if (!dirInfo.Exists) dirInfo.Create();
                var outpuName = pathFolder + "/" + word + ".mp3";
                var output = File.Open(path: outpuName, mode: FileMode.OpenOrCreate);
                response.AudioStream.CopyTo(destination: output);
                output.Close();
                //  output.Flush();  //!!!!!!!
            }
        }

        public List<VoicesLang> GetVoices()
        {
            var voicesLang = new List<VoicesLang>();
            using (var client = new AmazonPollyClient(credentials: awsCredentials, region: RegionEndpoint.USEast1))
            {
                var resp = client.DescribeVoicesAsync(new DescribeVoicesRequest()).GetAwaiter().GetResult();

                foreach (var item in resp.Voices)
                    voicesLang.Add(new VoicesLang()
                    {
                        Lang = item.LanguageCode,
                        Voice = item.Id
                    });
            }

            return voicesLang;
        }
    }
}