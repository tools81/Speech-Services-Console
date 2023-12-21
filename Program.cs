//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//

// <code>
using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Speech_Services_Console
{
    class Program
    {
        private static SpeechConfig Config = SpeechConfig.FromSubscription("2a2765f65f4f417c8be9dd0dc41cae9c", "centralus");
        
        static async Task Main()
        {
            Console.WriteLine("Test Text to Speech(1) or Speech to Text(2)?");
            string text = Console.ReadLine();

            switch(text)
            {
                case "1":
                    await SynthesisToSpeakerAsync();
                    break;
                case "2":
                    await SynthesisFromSpeakerAsync();
                    break;
                default:
                    Console.WriteLine("No valid selection given");
                    break;
            }
        }

        public static async Task SynthesisToSpeakerAsync()
        {
            Config.SpeechSynthesisVoiceName = "en-US-JasonNeural";

            using (var synthesizer = new SpeechSynthesizer(Config))
            {
                Console.WriteLine("Type some text that you want to speak...");
                Console.Write("> ");
                string text = Console.ReadLine();

                using (var result = await synthesizer.SpeakTextAsync(text))
                {
                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                    }
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }        

        public static async Task SynthesisFromSpeakerAsync()
        {
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(Config, audioConfig);

            Console.WriteLine("Speak into your microphone.");
            var result = await speechRecognizer.RecognizeOnceAsync();
            Console.WriteLine($"RECOGNIZED: Text={result.Text}");
        }
    }
}
// </code>