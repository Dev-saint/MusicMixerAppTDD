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

            // Используем IEEE float (32 бита на сэмпл) для корректной работы с MixingSampleProvider
            var targetFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2); // 32 бита на сэмпл, стерео
            var readers = new List<AudioFileReader>();
            var providers = new List<IWaveProvider>();

            try
            {
                // Загрузка всех файлов
                foreach (var file in inputFiles)
                {
                    if (!File.Exists(file))
                        throw new FileNotFoundException($"Файл не найден: {file}");

                    var reader = new AudioFileReader(file);

                    // Преобразуем в нужный формат (IEEE float)
                    var resampled = new WdlResamplingSampleProvider(reader, targetFormat.SampleRate);
                    var stereo = resampled.ToStereo();

                    // Преобразуем ISampleProvider в IWaveProvider
                    var waveProvider = new SampleToWaveProvider(stereo);
                    providers.Add(waveProvider);
                    readers.Add(reader);
                }

                var mixer = new MixingSampleProvider(targetFormat); // Миксер теперь в формате IEEE float
                mixer.ReadFully = true;

                // Добавляем каждый IWaveProvider по очереди
                foreach (var provider in providers)
                {
                    mixer.AddMixerInput(provider);
                }

                var outputPath = Path.Combine(Path.GetTempPath(), $"mix_{Guid.NewGuid()}.wav");

                Console.WriteLine("Начинаем запись в файл...");

                using (var writer = new WaveFileWriter(outputPath, mixer.WaveFormat))
                {
                    float[] buffer = new float[1024];  // Буфер для записи 32-битных сэмплов
                    int read;
                    int maxIterations = 10000;  // Ограничение на количество итераций для предотвращения бесконечного цикла
                    int iterationCount = 0;

                    // Логирование количества считанных сэмплов
                    while ((read = mixer.Read(buffer, 0, buffer.Length)) > 0 && iterationCount < maxIterations)
                    {
                        Console.WriteLine($"Чтение данных: {read} сэмплов.");
                        writer.WriteSamples(buffer, 0, read);
                        iterationCount++;
                    }

                    if (iterationCount >= maxIterations)
                    {
                        Console.WriteLine("Превышено максимальное количество итераций. Микширование может быть неполным.");
                    }
                }

                Console.WriteLine("Запись завершена.");
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
            finally
            {
                foreach (var r in readers)
                    r.Dispose();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Список для хранения путей к файлам
            List<string> inputFiles = new List<string>();

            Console.WriteLine("Введите пути к WAV файлам для микширования.");
            Console.WriteLine("Для завершения ввода введите 'stop'.");

            while (true)
            {
                // Запрос пути до файла
                Console.Write("Введите путь к файлу: ");
                string filePath = Console.ReadLine();

                // Проверка на команду 'stop' для завершения ввода
                if (filePath.ToLower() == "stop")
                    break;

                // Проверка на существование файла
                if (File.Exists(filePath))
                {
                    inputFiles.Add(filePath);
                    Console.WriteLine($"Файл добавлен: {filePath}");
                }
                else
                {
                    Console.WriteLine($"Файл не найден: {filePath}. Попробуйте снова.");
                }
            }

            if (inputFiles.Count == 0)
            {
                Console.WriteLine("Не было добавлено ни одного файла. Завершаю программу.");
                return;
            }

            try
            {
                // Создаем экземпляр класса Mixer
                var mixer = new Mixer();

                // Вызываем метод MixTracks для создания микса
                string outputFilePath = mixer.MixTracks(inputFiles);

                // Печатаем путь к созданному файлу микса
                Console.WriteLine($"Микс успешно создан! Файл находится по пути: {outputFilePath}");
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение об ошибке
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
