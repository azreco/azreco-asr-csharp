# AzReco Speech Recognition API C# example
Example project in c# .Net to help you integrate with our speech recognition API.

This is an example c# .Net project for uploading audio(.wav , .mp3 , .opus, .m4a) or video(.mp4 , .mkv) file and saving the transcript into a .json file.

# Supporting languages
AZERBAIJANI (az-AZ)

TURKISH  (tr-TR)

RUSSIAN  (ru-RU)

# Requirements

You will need to have the CommandLineParser and Microsoft.Net.Http module installed in your .Net environment.

For Windows please run commands below in terminal:

Install-Package Microsoft.Net.Http -Version 2.2.29

Install-Package CommandLineParser -Version 2.4.3
 
 
For .Net Core please run commands below in terminal:

dotnet add package Microsoft.Net.Http --version 2.2.29

dotnet add package CommandLineParser --version 2.4.3

# Usage example:
In Windows OS native .Net environment:

client.exe -a audio\\example-ru.wav -l ru-RU -i api_user_id -k api_token -o example-ru.json  

In .Net Core environment:

dotnet client.dll -a audio/example-ru.wav -l ru-RU -i api_user_id -k api_token -o example-ru.json 

In this example the application uploads 'example.wav', transcribes it using our ru-RU speech to text and saves the resulting transcription as 'example.json' when the transcribing process finished.


# How to get user id and token?

To get user id and API token, send a request to info@azreco.az.

To confirm your request, we will ask you some details about your purpose in using API.
