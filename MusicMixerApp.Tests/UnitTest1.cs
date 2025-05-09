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

            // ������ ��� ��������� WAV-����� �� 1 ������� ������
            for (int i = 0; i < 2; i++)
            {
                string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.wav");
                CreateSilentWav(path);
                tempFiles.Add(path);
            }

            // Act
            string result = mixer.MixTracks(tempFiles);

            // Assert
            Assert.True(File.Exists(result), "�������, ��� ����-���� ����� ������");
            Assert.EndsWith(".wav", result, StringComparison.OrdinalIgnoreCase);

            var fileInfo = new FileInfo(result);
            Assert.True(fileInfo.Length > 44, "WAV-���� �� ������ ���� ������ (��������� 44 �����)");

            // Cleanup
            foreach (var f in tempFiles) File.Delete(f);
            File.Delete(result);
        }

        // ��������������� �����
        private void CreateSilentWav(string path)
        {
            var waveFormat = new WaveFormat(44100, 16, 1); // Mono
            using (var writer = new WaveFileWriter(path, waveFormat))
            {
                byte[] silence = new byte[44100 * 2]; // 1 ������� ������ (16 ��� = 2 �����)
                writer.Write(silence, 0, silence.Length);
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