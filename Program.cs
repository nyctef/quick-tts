using System.CommandLine;

public static class Program
{
    public static void Main(string[] args)
    {
        

        var rootCommand = new RootCommand();

        var testCommand = new Command("test", "read out a test message");
        testCommand.SetHandler(async () =>
        {
            await SpeechWrapper.Create("en-US-JennyNeural").Say("This example demonstrates a basic use of Speech Synthesizer");
        });
        rootCommand.AddCommand(testCommand);

        var listCommand = new Command("list", "list out available voices");
        listCommand.SetHandler(async () =>
        {
            var voices = await SpeechWrapper.Create("en-US-JennyNeural").GetAvailableVoices();
            foreach (var voice in voices)
            {
                Console.WriteLine(voice);
            }
        });
        rootCommand.AddCommand(listCommand);

        var sayCommand = new Command("say", "send some TTS to the default speaker");
        var textArgument = new Argument<string>("text");
        sayCommand.AddArgument(textArgument);
        sayCommand.SetHandler(async (string text) =>
        {
            await SpeechWrapper.Create("ja-JP-NanamiNeural").Say(text);
            
        }, textArgument);
        rootCommand.AddCommand(sayCommand);

        rootCommand.Invoke(args);
    }
}