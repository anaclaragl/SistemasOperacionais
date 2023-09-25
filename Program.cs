using System;
using System.Collections.Generic;
using System.IO;

class Program{
    static void Main(string[] args){
        Dictionary<int, Object> database = new Dictionary<int, Object>();

        if (args.Length == 0)
        {
            Console.WriteLine("Avaliable Commands: --insert, remove, search, update");
            return;
        }

        string command = args[0].ToLower();
        //ToLower = deixa o que foi digitado em lower case

        switch(command){
            case "--insert":
                Console.WriteLine("Command format: insert <key> <value>");

                int keyIns = int.Parse(args[1]);
                string valueIns = args[2];
                database[keyIns] = new Object(valueIns);
                Console.WriteLine("inserted");
                SaveData();
                break;
            case "remove":
                Console.WriteLine("Command format: remove <key>");

                int keyRem = int.Parse(args[1]);
                if (database.ContainsKey(keyRem))
                {
                    database.Remove(keyRem);
                    Console.WriteLine("removed");
                }
                else
                {
                    Console.WriteLine("key not found");
                }
                SaveData();
                break;
            case "search":
                int keySea = int.Parse(args[1]);
                if (database.ContainsKey(keySea))
                {
                    Console.WriteLine("Found object: " + database[keySea]);
                }
                else
                {
                    Console.WriteLine("key not found");
                }
                SaveData();
                break;
            case "update":
                int keyUp = int.Parse(args[1]);
                string valueUp = args[2];

                if (database.ContainsKey(keyUp))
                {
                    database[keyUp] = new Object(valueUp);
                    Console.WriteLine("updated");
                }
                else
                {
                    Console.WriteLine("key not found");
                }
                SaveData();
                break;
            default:
                Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                break;
        }
    }

/*Prototipo/Tentativa do LoadData

    /private void LoadData(string dbilePath){
        database = new Dictionary<int, Object>();

        if (File.Exists(dbfilePath)){
            string[] writer = File.ReadAllLines(dbfilePath);

            foreach (string entry in writer){

            }
        }
    }*/

    private static void SaveData(){
        List<string> writer = new List<string>();
        foreach (var process in database){
            writer.Add(process.Key + "," + process.Value);
        }
        File.WriteAllLines(filePath, writer);
    }

}
class Object
{
    public string value { get; set; }

    public Object(string Value)
    {
        value = Value;
    }

    public override string ToString()
    {
        return value;
    }
    
}