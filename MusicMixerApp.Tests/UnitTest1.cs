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
                File.WriteAllText(path, $"Τΰιλ {i}");
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
    }
}