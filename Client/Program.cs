using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Text.RegularExpressions;

namespace Client
{
    class Program
    {
        public class Options
        {
            [Option('a', "audio", Required = true, HelpText = "Audio file or youtube, facebook, twitter, dailymotion video link to be processed")]
            public string AudioFile { get; set; }

            [Option('o', "output", Required = false, HelpText = "Output filename (will print to terminal if not specified)")]
            public string Output { get; set; }

            [Option('i', "id", Required = true, HelpText = "Your transcriber API ID")]
            public string UserId { get; set; }

            [Option('k', "token", Required = true, HelpText = "Your transcriber API Token")]
            public string ApiToken { get; set; }

            [Option('l', "lang", Required = true, HelpText = "Code of language to use (e.g., en-US, ru-RU, tr-TR)")]
            public string Language { get; set; }
        }
        static void Main(string[] args)
        {
            string audioFile = null;
            string output = null;
            string userId = null;
            string token = null;
            string lang = null;
            try
            {
                // Parsing commandline arguments
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => {
                        audioFile = opts.AudioFile;
                        output = opts.Output;
                        userId = opts.UserId;
                        token = opts.ApiToken;
                        lang = opts.Language;
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown while parsing arguments: " + ex.Message);
                return;
            }

            Regex rx = new Regex(@"\b(https?://)?(www\.)?(youtube|youtu|youtube-nocookie|facebook|dailymotion|twitter)\.(com|be)\b",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Transcriber transcriber = new Transcriber(userId, token, lang);
            Task<string> resultTask = null;
            if(rx.Matches(audioFile).Count > 0)
            {
                resultTask = transcriber.TranscribeVideoLink(audioFile);
            }
            else
            {
                resultTask = transcriber.Transcribe(audioFile);
            }
            if (resultTask == null)
            {
                Console.WriteLine("Transcibing process failed.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            if (resultTask.IsCanceled || resultTask.IsFaulted)
            {
                Console.WriteLine("Transcibing process failed.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            if (!resultTask.IsCompleted)
            {
                resultTask.Wait();
            }
            string result = resultTask.Result;
            if (result == null)
            {
                Console.WriteLine("Transcibing process failed.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            if (output == null)
            {
                Console.WriteLine(result);
            }
            else
            {
                StreamWriter writer = new StreamWriter(output, false, Encoding.UTF8);
                writer.Write(result);
                writer.Close();
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
