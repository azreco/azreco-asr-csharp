using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System;

public class Transcriber
{
    private string userId = null;
    private string token = null;
    private string lang = null;

    public Transcriber(string userId, string token, string lang) {
        this.userId = userId;
        this.token = token;
        this.lang = lang;
    }

    public async Task<string> Transcribe(string audioFile)
    {
        string apiURL = "http://api.azreco.az/transcribe";
        HttpContent idContent = new StringContent(userId);
        HttpContent tokenContent = new StringContent(token); 
        HttpContent langContent = new StringContent(lang);
        FileStream fs = null;
        try 
        {
            fs = File.OpenRead(audioFile);
        }
        catch(IOException ex) 
        {
            Console.WriteLine("File IO error: " + ex.Message);
            return null;
        }
        string fileName = Path.GetFileName(audioFile);
        HttpContent streamContent = new StreamContent(fs); 

        // Making multipart form content and posting it to the server
        using(var client = new HttpClient())
        using(var formData = new MultipartFormDataContent()) 
        {
            formData.Add(idContent, "api_id");
            formData.Add(tokenContent, "api_token");
            formData.Add(langContent, "lang");
            formData.Add(streamContent, "file", fileName);
            var response = await client.PostAsync(apiURL, formData);
            if(!response.IsSuccessStatusCode) {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }
    }

    public async Task<string> TranscribeVideoLink(string audioFile)
    {
        string apiURL = "http://api.azreco.az/transcribe_video_link";
        HttpContent idContent = new StringContent(userId);
        HttpContent tokenContent = new StringContent(token);
        HttpContent langContent = new StringContent(lang);
        HttpContent linkContent = new StringContent(audioFile);
        // Making multipart form content and posting it to the server
        using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
            formData.Add(idContent, "api_id");
            formData.Add(tokenContent, "api_token");
            formData.Add(langContent, "lang");
            formData.Add(linkContent, "link");
            var response = await client.PostAsync(apiURL, formData);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}