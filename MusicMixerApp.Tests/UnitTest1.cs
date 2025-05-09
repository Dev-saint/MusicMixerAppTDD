using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using MusicMixerApp;

namespace MusicMixerApp.Tests
{
    public class MixerTests
    {
        [Fact]
        public void MixTracks_ReturnsOutputPath_WhenGivenValidFiles()
        {
            // Arrange
            var mixer = new Mixer();

            var tempFiles = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                string path = Path.GetTempFileName();
                File.WriteAllText(path, $"Файл {i}");
                tempFiles.Add(path);
            }

            // Act
            var output = mixer.MixTracks(tempFiles);

            // Assert
            Assert.True(File.Exists(output));
            Assert.Contains("mix_stub.wav", Path.GetFileName(output));

            // Cleanup
            foreach (var file in tempFiles)
                File.Delete(file);
            File.Delete(output);
        }

        [Fact]
        public void MixTracks_ThrowsException_WhenListIsNull()
        {
            // Arrange
            var mixer = new Mixer();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => mixer.MixTracks(null));
        }

        [Fact]
        public void MixTracks_ThrowsException_WhenFileNotFound()
        {
            // Arrange
            var mixer = new Mixer();
            var files = new List<string> { "non_existent_file.wav" };

            // Act + Assert
            Assert.Throws<FileNotFoundException>(() => mixer.MixTracks(files));
        }

        [Fact]
        public void MixTracks_CreatesNonEmptyOutput_WhenGivenValidWavFiles()
        {
            // Arrange
            var mixer = new Mixer();
            var tempFiles = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                string path = Path.GetTempFileName().Replace(".tmp", ".wav");
                File.WriteAllBytes(path, GenerateSilentWavBytes(1)); // 1 секунда тишины
                tempFiles.Add(path);
            }

            // Act
            var output = mixer.MixTracks(tempFiles);

            // Assert
            Assert.True(File.Exists(output));
            Assert.True(new FileInfo(output).Length > 100); // Должен быть не пустым

            // Cleanup
            foreach (var file in tempFiles)
                File.Delete(file);
            File.Delete(output);
        }

        // Вспомогательный метод
        private byte[] GenerateSilentWavBytes(int seconds)
        {
            using var ms = new MemoryStream();
            int sampleRate = 44100;
            int bytesPerSample = 2;
            int numSamples = sampleRate * seconds;
            int byteRate = sampleRate * bytesPerSample;

            using var writer = new BinaryWriter(ms);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(36 + numSamples * bytesPerSample);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
            writer.Write(16);
            writer.Write((short)1); // PCM
            writer.Write((short)1); // mono
            writer.Write(sampleRate);
            writer.Write(byteRate);
            writer.Write((short)(bytesPerSample));
            writer.Write((short)(bytesPerSample * 8));
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            writer.Write(numSamples * bytesPerSample);
            for (int i = 0; i < numSamples; i++) writer.Write((short)0);
            return ms.ToArray();
        }
    }
}