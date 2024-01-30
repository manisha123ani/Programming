using HelloWorldLibrary.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HelloWorldLibrary.BusinessLogic;
public class Messages : IMessages
{
    private readonly ILogger<Messages> _log;

    public Messages(ILogger<Messages> log)
    {
        _log = log;
    }

    public string Greeting(string language)
    {
        string output = LookUpCustomeText(key: "Greeting", language);
        return output;
    }

    private string LookUpCustomeText(string key, string language)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        try
        {
            List<CustomText>? messageSets = JsonSerializer.Deserialize<List<CustomText>>
            (
                File.ReadAllText(path: "CustomText.json"), options
            );
            CustomText? messages = messageSets?.Where(x => x.Language == language).First();

            if (messages is null)
            {
                throw new NullReferenceException(message: "The Specified language was not found in the Json file");
            }
            return messages.Translations[key];
        }
        catch (Exception ex)
        {
            _log.LogError(message: "Error looking up the custom text", ex);
            throw;
        }


    }

}
