using System.CommandLine;
using System.Speech.Synthesis;

public static class Program
{
    public static void Main(string[] args)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new NotSupportedException();
        }

        var rootCommand = new RootCommand();

        var testCommand = new Command("test", "read out a test message");
        testCommand.SetHandler(() =>
        {
            var synth = new SpeechSynthesizer();

            synth.SetOutputToDefaultAudioDevice();

            synth.Speak("This example demonstrates a basic use of Speech Synthesizer");
        });
        rootCommand.AddCommand(testCommand);

        var listCommand = new Command("list", "list out available voices");
        listCommand.SetHandler(() =>
        {
            var voices = new SpeechSynthesizer().GetInstalledVoices().Where(x => x.Enabled);
            Console.WriteLine("Available voices:");
            foreach (var voice in voices)
            {
                Console.WriteLine($"  {voice.VoiceInfo.Name}: {voice.VoiceInfo.Description}");
            }
        });
        rootCommand.AddCommand(listCommand);

        var sayCommand = new Command("say", "send some TTS to the default speaker");
        var textArgument = new Argument<string>("text");
        sayCommand.AddArgument(textArgument);
        sayCommand.SetHandler((string text) =>
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoice("Microsoft Haruka Desktop");

            synth.SetOutputToDefaultAudioDevice();

            synth.Speak(text);
        }, textArgument);
        rootCommand.AddCommand(sayCommand);

        rootCommand.Invoke(args);
    }
}