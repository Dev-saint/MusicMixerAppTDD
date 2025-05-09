using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MusicMixerApp
{
    public class Mixer
    {
        public string MixTracks(List<string> inputFiles)
        {
            if (inputFiles == null || inputFiles.Count == 0)
                throw new ArgumentException("Список файлов пуст.");

            var readers = inputFiles.Select(file => new AudioFileReader(file)).ToList();
            var mixer = new MixingSampleProvider(readers);
            mixer.ReadFully = true;

            var outputPath = Path.Combine(Path.GetTempPath(), $"mix_{Guid.NewGuid()}.wav");

            using (var fileWriter = new WaveFileWriter(outputPath, mixer.WaveFormat))
            {
                float[] buffer = new float[1024];
                int read;
                while ((read = mixer.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileWriter.WriteSamples(buffer, 0, read);
                }
            }

            // Освобождаем ресурсы
            foreach (var reader in readers)
            {
                reader.Dispose();
            }

            return outputPath;
        }
    }
}