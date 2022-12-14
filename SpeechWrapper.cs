using Microsoft.CognitiveServices.Speech;

internal class SpeechWrapper
{
    private readonly SpeechConfig _speechConfig;

    public SpeechWrapper(SpeechConfig speechConfig)
    {
        _speechConfig = speechConfig;
    }

    public static SpeechWrapper Create(string voice)
    {
        try
        {
            var apiKey = File.ReadAllText("api-key.txt");
            var speechConfig = SpeechConfig.FromSubscription(apiKey, "uksouth");
            speechConfig.SpeechSynthesisVoiceName = voice;
            return new SpeechWrapper(speechConfig);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create speech config - did you put your api key in `api-key.txt`?", e);
        }
    }

    public async Task Say(string text)
    {
        using var speechSynthesizer = new SpeechSynthesizer(_speechConfig);

        var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
        if (speechSynthesisResult.Reason != ResultReason.SynthesizingAudioCompleted)
        {
            throw new Exception(speechSynthesisResult.Reason.ToString());
        }
    }

    public async Task<string[]> GetAvailableVoices()
    {
        using var speechSynthesizer = new SpeechSynthesizer(_speechConfig);

        using var voicesResult = await speechSynthesizer.GetVoicesAsync();

        return voicesResult.Voices.Select(x => x.ShortName).ToArray();
    }
}