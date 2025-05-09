using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using MusicMixerApp;
using NAudio.Wave;

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
        public void MixTracks_ReturnsOutputFilePath_WhenValidFiles()
        {
            // arrange
            var mixer = new Mixer();

            var tempFile1 = Path.Combine(Path.GetTempPath(), "test1.wav");
            var tempFile2 = Path.Combine(Path.GetTempPath(), "test2.wav");

            // Создаем dummy-файлы с тишиной
            CreateSilentWav(tempFile1);
            CreateSilentWav(tempFile2);

            // act
            var result = mixer.MixTracks(new List<string> { tempFile1, tempFile2 });

            // assert
            Assert.True(File.Exists(result));
        }

        // Вспомогательный метод
        private void CreateSilentWav(string path)
        {
            var waveFormat = new WaveFormat(44100, 16, 1); // Mono
            using (var writer = new WaveFileWriter(path, waveFormat))
            {
                byte[] silence = new byte[44100 * 2]; // 1 секунда тишины (16 бит = 2 байта)
                writer.Write(silence, 0, silence.Length);
            }
        }
    }
}