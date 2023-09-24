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

                break;
            case "remove":
            
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
    public void Insert(int key, string value){

    }
    public void Remove(int key){
        
    }
    public void Search(string[] args){
        /*Console.WriteLine("Commad format: search <key>");

        int keySearch = int.Parse(args[1]);

        if(database.ContainsKey(keySearch)){

        }*/
    }
    public void Update(int key){
        
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