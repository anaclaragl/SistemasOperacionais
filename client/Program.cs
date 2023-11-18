<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.IO;
using MSMQ.Messaging;
using System.Threading;
using System.Text;

namespace client{

    class Program{
        
        const string mqPathClient = ".\\Private$\\ClientQueue"; 
        const string mqPathServer = ".\\Private$\\SimpleDBQueue";
        static void Main(string[] args){

            //verifica a existencia da queue do cliente
            if(!MessageQueue.Exists(mqPathClient)){
                MessageQueue.Create(mqPathClient);
            }

            while(true){

                /*if (args.Length == 0){
                    return;
                }*/

                Console.Write("//simpledb> ");

                string? entry = Console.ReadLine();
                string[] inputs = entry.Split(' ');

                string action = inputs[0].ToLower();

                Command command = new Command();

                switch (action){
                    case "--insert":
                        command.op = Operation.Insert;
                        break;
                    case "remove":
                        command.op = Operation.Remove;
                        break;
                    case "search":
                        command.op = Operation.Search;
                        break;
                    case "update":
                        command.op = Operation.Update;
                        break;
                    case "quit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                        break;
                }

                string[] separateInput = inputs[1].Split(',');
                command.key = Convert.ToInt32(separateInput[0]);

                if(separateInput.Length > 1){
                    command.value = separateInput[1];
                }


                MessageQueue messageQueueCli = new MessageQueue(mqPathClient){
                    Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
                };

                MessageQueue messageQueueServer = new MessageQueue(mqPathServer){
                    Formatter = new XmlMessageFormatter(new Type[] { typeof(Command) })
                };

                try{
                    Message msg = new Message(command);
                    Console.WriteLine("Sending command...");
                    messageQueueServer.Send(msg);
                    Console.WriteLine("Sent!");

                    string cliMsg = (string)messageQueueCli.Receive().Body;
                    Console.WriteLine("Response recieved!");

                    messageQueueCli.Close();
                    messageQueueServer.Close();

                    Console.WriteLine(cliMsg);
                }catch (Exception e){
                    Console.WriteLine("error: " + e.Message);
                }

                //deleta a fila do cliente
                if (MessageQueue.Exists(mqPathClient))
                {
                    MessageQueue.Delete(mqPathClient);
                }

            }
=======
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
>>>>>>> d092c2c5511acf3a893afddfd547cc0feb5fe1bf
        }
    }
}