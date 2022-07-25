using System.CommandLine;
using System.Speech.Synthesis;

public static class Program
{
    public static void Main(string[] args)
    {
        var rootCommand = new RootCommand();

        var testCommand = new Command("test", "read out a test message");
        testCommand.SetHandler(() =>
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new NotSupportedException();
            }

            var synth = new SpeechSynthesizer();

            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.  
            synth.Speak("This example demonstrates a basic use of Speech Synthesizer");
        });
        rootCommand.AddCommand(testCommand);

        rootCommand.Invoke(args);
    }
}