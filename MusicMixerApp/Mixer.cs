using System;
using System.Collections.Generic;
using System.IO;

namespace MusicMixerApp
{
    public class Mixer
    {
        public string MixTracks(List<string> inputFilePaths)
        {
            // TODO: в будущем — тут будет реальное смешивание аудио

            // Проверка входных данных
            if (inputFilePaths == null || inputFilePaths.Count == 0)
                throw new ArgumentException("Список входных файлов пуст.");

            foreach (var file in inputFilePaths)
            {
                if (!File.Exists(file))
                    throw new FileNotFoundException($"Файл не найден: {file}");
            }

            // Эмуляция работы
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "mix_stub.wav");

            // Заглушка — создаем пустой файл
            File.WriteAllText(outputPath, "Эмуляция микса."); // можно заменить на byte[] позже

            return outputPath;
        }
    }
}