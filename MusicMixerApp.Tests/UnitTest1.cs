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

            // Создаём два временных WAV-файла по 10 секунд тишины
            for (int i = 0; i < 2; i++)
            {
                string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.wav");
                CreateSilentWav(path);
                tempFiles.Add(path);
            }

            // Act
            string result = mixer.MixTracks(tempFiles);

            // Assert
            Assert.True(File.Exists(result), "Ожидали, что файл-микс будет создан");
            Assert.EndsWith(".wav", result, StringComparison.OrdinalIgnoreCase);

            var fileInfo = new FileInfo(result);
            Assert.True(fileInfo.Length > 44, "WAV-файл не должен быть пустым (заголовок 44 байта)");

            // Cleanup
            foreach (var f in tempFiles) File.Delete(f);
            File.Delete(result);
        }

        // Вспомогательный метод
        private void CreateSilentWav(string path, int durationInSeconds = 10)
        {
            var waveFormat = new WaveFormat(44100, 16, 1); // Mono
            using (var writer = new WaveFileWriter(path, waveFormat))
            {
                int sampleCount = 44100 * durationInSeconds; // Количество сэмплов на заданную длительность
                short[] silence = new short[sampleCount]; // Массив с тишиной (в 16-битном формате)
                writer.WriteSamples(silence, 0, silence.Length); // Запись тишины
            }
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
    }
}