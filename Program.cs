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
                break;
            case "search":
            
                break;
            case "update":
            
                break;
            default:
                Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                break;
        }
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

    private void SaveData(){

    }

    private void LoadData(){

    }
    
}