using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

//1

//public interface IObserver
//{
//    void Update(FileChanges fileChanges);
//}

//public class FileChanges
//{
//    public List<string> Added { get; set; } = new List<string>();
//    public List<string> Removed { get; set; } = new List<string>();
//}

//public class FileSystemWatcher
//{
//    private readonly string _path;
//    private readonly List<IObserver> _observers = new List<IObserver>();
//    private HashSet<string> _previousFiles;
//    private bool _running;

//    public FileSystemWatcher(string path)
//    {
//        _path = path;
//        _previousFiles = new HashSet<string>(Directory.GetFiles(path).Select(Path.GetFileName));
//    }

//    public void AddObserver(IObserver observer)
//    {
//        _observers.Add(observer);
//    }

//    public void RemoveObserver(IObserver observer)
//    {
//        _observers.Remove(observer);
//    }

//    public void NotifyObservers(FileChanges fileChanges)
//    {
//        foreach (var observer in _observers)
//        {
//            observer.Update(fileChanges);
//        }
//    }

//    private void CheckDirectory()
//    {
//        while (_running)
//        {
//            var currentFiles = new HashSet<string>(Directory.GetFiles(_path).Select(Path.GetFileName));
//            var addedFiles = currentFiles.Except(_previousFiles).ToList();
//            var removedFiles = _previousFiles.Except(currentFiles).ToList();

//            if (addedFiles.Any() || removedFiles.Any())
//            {
//                var fileChanges = new FileChanges
//                {
//                    Added = addedFiles,
//                    Removed = removedFiles
//                };
//                NotifyObservers(fileChanges);
//                _previousFiles = currentFiles;
//            }

//            Thread.Sleep(1000);
//        }
//    }

//    public void StartWatching()
//    {
//        _running = true;
//        Task.Run(CheckDirectory);
//    }

//    public void StopWatching()
//    {
//        _running = false;
//    }
//}

//public class FileObserver : IObserver
//{
//    public void Update(FileChanges fileChanges)
//    {
//        if (fileChanges.Added.Any())
//        {
//            Console.WriteLine($"Добавлены файлы: {string.Join(", ", fileChanges.Added)}");
//        }
//        if (fileChanges.Removed.Any())
//        {
//            Console.WriteLine($"Удалены файлы: {string.Join(", ", fileChanges.Removed)}");
//        }
//    }
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        string pathToWatch = @"C:\Users\Nastya tyta\OneDrive\Рабочий стол\ая 3 сем"; 
//        var watcher = new FileSystemWatcher(pathToWatch);

//        var observer = new FileObserver();
//        watcher.AddObserver(observer);

//        watcher.StartWatching();

//        Console.WriteLine("Нажмите любую клавишу для остановки...");
//        Console.ReadKey();

//        watcher.StopWatching();
//    }
//}




//2


//public interface ILogRepository
//{
//    void Log(string message);
//}

//public class TextLogRepository : ILogRepository
//{
//    private readonly string _filePath;

//    public TextLogRepository(string filePath)
//    {
//        _filePath = filePath;
//    }

//    public void Log(string message)
//    {
//        using (var writer = new StreamWriter(_filePath, true))
//        {
//            writer.WriteLine(message);
//        }
//    }
//}

//public class JsonLogRepository : ILogRepository
//{
//    private readonly string _filePath;
//    private bool _isFirstLog = true;

//    public JsonLogRepository(string filePath)
//    {
//        _filePath = filePath;
//        if (!File.Exists(_filePath))
//        {
//            File.WriteAllText(_filePath, "[\n");
//        }
//    }

//    public void Log(string message)
//    {
//        var logEntry = new
//        {
//            Timestamp = DateTime.Now,
//            Message = message
//        };

//        using (var writer = new StreamWriter(_filePath, true))
//        {
//            if (_isFirstLog)
//            {
//                _isFirstLog = false;
//            }
//            else
//            {
//                writer.Write(",\n");
//            }
//            writer.WriteLine(JsonConvert.SerializeObject(logEntry, Newtonsoft.Json.Formatting.Indented));
//        }
//    }

//    public void Close()
//    {
//        using (var writer = new StreamWriter(_filePath, true))
//        {
//            writer.WriteLine("\n]");
//        }
//    }
//}
//public class MyLogger
//{
//    private readonly ILogRepository _repository;

//    public MyLogger(ILogRepository repository)
//    {
//        _repository = repository;
//    }

//    public void Log(string message)
//    {
//        _repository.Log(message);
//    }

//    public void Close()
//    {
//        if (_repository is JsonLogRepository jsonRepo)
//        {
//            jsonRepo.Close();
//        }
//    }
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        // Создаем логгер для текстовых файлов
//        var textLogger = new MyLogger(new TextLogRepository("log.txt"));
//        textLogger.Log("Это сообщение для текстового логирования.");

//        // Создаем логгер для JSON файлов
//        var jsonLogger = new MyLogger(new JsonLogRepository("log.json"));
//        jsonLogger.Log("Это сообщение для JSON логирования.");

//        // Закрываем JSON логгер, чтобы корректно завершить массив
//        jsonLogger.Close();

//        Console.WriteLine("Логирование завершено.");
//    }
//}


//3


public class SingleRandomizer
{
    private static readonly Lazy<SingleRandomizer> instance = new Lazy<SingleRandomizer>(() => new SingleRandomizer());
    private Random random;

    // Приватный конструктор для предотвращения создания экземпляров класса извне
    private SingleRandomizer()
    {
        random = new Random();
    }

    // Свойство для получения единственного экземпляра класса
    public static SingleRandomizer Instance => instance.Value;

    // Метод для получения следующего случайного числа
    public int Next(int minValue, int maxValue)
    {
        lock (random) // Блокировка для потокобезопасности
        {
            return random.Next(minValue, maxValue);
        }
    }

    // Метод для получения следующего случайного числа
    public int Next()
    {
        lock (random) // Блокировка для потокобезопасности
        {
            return random.Next();
        }
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        // Пример многопоточного доступа к генератору случайных чисел
        Parallel.For(0, 10, i =>
        {
            Console.WriteLine(SingleRandomizer.Instance.Next(1, 100));
        });
    }
}
