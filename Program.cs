using System;
using System.Collections.Generic;
using System.IO;

class Program{
    static Dictionary<int, string> database = new Dictionary<int, string>();

    static string filePath = "simpledb.txt";

    static void Main(string[] args)
    {

        if (args.Length == 0)
        {
            Console.WriteLine("Avaliable Commands: --insert, remove, search, update");
            return;
        }

        string command = args[0].ToLower();
        string[] inputs = args[1].Split(',');
        int key = Convert.ToInt32(inputs[0]);
        string value = inputs[1];

        switch (command)
        {
            case "--insert":
                if (key >= 0 && value != null)
                {

                    Insert(key, value);
                }
                break;
            case "remove":
                if (key >= 0)
                {
                    Remove(key);
                }
                break;
            case "search":
                if (key >= 0)
                {
                    Search(key);
                }
                break;
            case "update":
                if (key >= 0)
                {
                    Update(key, value);
                }
                break;
            default:
                Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                break;
        }
    }

    static void Insert(int key, string value)
    {
        database.Add(key, value);
        SaveData(filePath, database);
        Console.WriteLine("inserted");
    }

    static void Remove(int key)
    {
        if (database.ContainsKey(key))
        {
            database.Remove(key);
            Console.WriteLine("removed");
        }
        else
        {
            Console.WriteLine("key not found");
        }
    }

    static void Search(int key)
    {
        if (database.ContainsKey(key))
        {
            Console.WriteLine("Found object: " + database[key]);
        }
        else
        {
            Console.WriteLine("key not found");
        }
    }

    static void Update(int key, string newValue)
    {
        if (database.ContainsKey(key))
        {
            database[key] = newValue;
            Console.WriteLine("updated");
        }
        else
        {
            Console.WriteLine("key not found");
        }
    }
}