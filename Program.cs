using System;
using System.Collections.Generic;
using System.IO;

class Program{ //classe pricipal do programa
    static void Main(string[] args){

        string filePath = "simpledb.txt"; //arquivo de armazenamento dos dados
        SimpleDB database = new SimpleDB(filePath); //nova instancia da classe SimpleDB

        if (args.Length == 0){
            return;
        }

        try{
            string command = args[0].ToLower(); //variavel que recebe os comandos
            string[] inputs = args[1].Split(','); //divisao entre chave-valor, [0] -> chave, [1] -> valor

            switch(command){
                /*uso de if para checar o tamanho correto de cada comando.
                  EX: o uso do insert: <--insert> <key,value>,
                  portanto inputs deve ter 2 de tamanho 
                */
                case "--insert": 
                    if(inputs.Length != 2){
                        Console.WriteLine("Wrong insert format. Try using: --insert key,value");
                    }else{
                        int key = Convert.ToInt32(inputs[0]);
                        string value = inputs[1];
                        database.Insert(key, value);
                    }
                    break;
                case "remove":
                    if(inputs.Length != 1){
                        Console.WriteLine("Wrong insert format. Try using: remove key");
                    }else{
                        int key = Convert.ToInt32(inputs[0]);
                        database.Remove(key);
                    }
                    break;
                case "search":
                    if(inputs.Length != 1){
                        Console.WriteLine("Wrong insert format. Try using: search key");
                    }else{
                        int key = Convert.ToInt32(inputs[0]);
                        database.Search(key);
                    }
                    break;
                case "update":
                    if(inputs.Length != 2){
                        Console.WriteLine("Wrong insert format. Try using: update key,newValue");
                    }else{
                        int key = Convert.ToInt32(inputs[0]);
                        string value = inputs[1];
                        database.Insert(key, value);
                    }
                    break;
                case "quit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                    break;
            }
        }catch(Exception e){
            Console.WriteLine("ERROR: " + e.Message);
        }
    }
}