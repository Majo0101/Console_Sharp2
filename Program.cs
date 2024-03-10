//TODO Dictionary
using System.Net.Mail;

interface IMemory{
    public string ReadData();
    public string SaveData();
    public string AddData(string key, string value);
    public string FindWord(string key);
    public int CoutData();
}

class Memory : IMemory
{
    private Dictionary<string, string> words;
    private string filePath; 
    public Memory(){
        words = new Dictionary<string, string>();
        filePath = @"memory.csv";
    }

    public string ReadData(){
        try{
            if (File.Exists(filePath)){
                string[] lines = File.ReadAllLines(filePath);
                
                foreach (string line in lines){
                    string[] parts = line.Split('|');
                    if (parts.Length == 2){
                        words.Add(parts[0], parts[1]);
                    }
                }

                return "The memory was loaded";
            }
            else{
                return "Memory does not exists";
            }
        }
        catch (Exception ex){
            return ex.Message ;
        }
    }


    public string SaveData(){
        try{
            using var writer = new StreamWriter(filePath);

            foreach (var record in words){
                string line = $"{record.Key}|{record.Value}";
                writer.WriteLine(line);
            }

            return "Dictionary has been saved.";
        }
        catch (Exception ex){
            return "An error occurred during saving: " + ex.Message;
        }
    }


    public string AddData(string key, string value){
        if(words.ContainsKey(key)){
            return "The key already exist.";
        }else{
            words.Add(key, value);
            return "The key has been added";
        }
    }


    public string FindWord(string key){
        if(words.ContainsKey(key)){
            return $"Translate for {key} is {words[key]}.";
        }
        return "The key does not exist";
    }


    public int CoutData(){
        return words.Count;
    }
}

// TODO Menu

class App{
    Memory mem;

    public App(){
        mem = new();
        mem.ReadData();
    }

    public void Run(){
        bool exit = false;

        while (!exit){
            Console.WriteLine("\nDictionary Menu:");
            Console.WriteLine("1. Add Word");
            Console.WriteLine("2. Find Word");
            Console.WriteLine("3. Count Words");
            Console.WriteLine("4. Save Data (manual save)");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            switch (Console.ReadLine()){
                case "1":
                    Console.Write("Enter word: ");
                    string? key = Console.ReadLine();
                    Console.Write("Enter meaning: ");
                    string? value = Console.ReadLine();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)){
                        Console.WriteLine(mem.AddData(key, value));
                    }else{
                        Console.WriteLine("Both word and meaning must be provided.");
                    }
                    break;

                case "2":
                    Console.Write("Enter word to find: ");
                    key = Console.ReadLine();
                    if (!string.IsNullOrEmpty(key)){
                        Console.WriteLine(mem.FindWord(key));
                    }else{
                        Console.WriteLine("The word must be provided.");
                    }
                    break;

                case "3":
                    Console.WriteLine($"Total words: {mem.CoutData()}");
                    break;

                case "4":
                    Console.WriteLine(mem.SaveData());
                    break;

                case "5":
                    exit = true;
                    Console.WriteLine(mem.SaveData());
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}


class Program
{
    static void Main(string[] args){
        App app = new();
        app.Run();
    }
}