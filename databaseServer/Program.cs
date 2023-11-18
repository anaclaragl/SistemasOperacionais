<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.IO;
using MSMQ.Messaging;

namespace databaseServer{
    class Program{
    const string mqPathServer = ".\\Private$\\SimpleDBQueue";
    const string mqPathClient = ".\\Private$\\ClientQueue";
    static void Main(string[] args){

        SimpleDB database = new SimpleDB("simpledb.txt");

        if(args.Length > 0){
            ReadLineArgs(args, database);
            return;
        }

        //verifica se o queue existe, se nao: cria uma nova fila
        if(!MessageQueue.Exists(mqPathServer)){
            MessageQueue.Create(mqPathServer);
        }

        //abertura do message queue do servidor
        MessageQueue messageQueueServer = new MessageQueue(mqPathServer){
            Formatter = new XmlMessageFormatter(new Type[] { typeof(Command) })
        };

        //while(true){
            Console.WriteLine("In progress...");
             //receber
            //executar
            //fechar
            Console.WriteLine("Recieving command...");
            Message msg = messageQueueServer.Receive();
            Console.WriteLine("Command recieved!");
            Command command = (Command)msg.Body;

            Console.WriteLine("Sending to client...");
            SendToClient(command, database);

            messageQueueServer.Close();
        //}
        //falta deletar a queue

    }

    static public void SendToClient(Command command, SimpleDB database){

        Console.WriteLine("Executing...");
        string answer = database.MessageExecute(command);
        Message msg = new Message(answer);

        //abertura do message queue do client
        MessageQueue messageQueueClient = new MessageQueue(mqPathClient){
            Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
        };
        
        messageQueueClient.Send(msg);
        messageQueueClient.Close();
    }
        

    static public void ReadLineArgs(string[] args, SimpleDB database){

        string action = args[0].ToLower(); //variavel que recebe os comandos
        string[] inputs = args[1].Split(','); //divisao entre chave-valor, [0] -> chave, [1] -> valor

        Command command = new Command();
        command.key = Convert.ToInt32(inputs[0]);

        switch (action){
                case "--insert":
                    if (inputs.Length != 2){
                        Console.WriteLine("Wrong insert format. Try using: --insert key,value");
                    }else{
                        string value = inputs[1];
                        command.op = Operation.Insert;
                    }
                    break;
                case "remove":
                    if (inputs.Length != 1){
                        Console.WriteLine("Wrong insert format. Try using: remove key");
                    }else{
                        command.op = Operation.Remove;
                    }
                    break;
                case "search":
                    if (inputs.Length != 1){
                        Console.WriteLine("Wrong insert format. Try using: search key");
                    }else{
                        command.op = Operation.Search;
                    }
                    break;
                case "update":
                    if (inputs.Length != 2){
                        Console.WriteLine("Wrong insert format. Try using: update key,newValue");
                    }else{
                        string value = inputs[1];
                        command.op = Operation.Update;
                    }
                    break;
                case "quit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                    break;
            }
=======
using System;
using System.Collections.Generic;
using System.IO;

namespace databaseServer{
    class Program{

        static void Main(string[] args){
            //Classe Main do Servidor
>>>>>>> d092c2c5511acf3a893afddfd547cc0feb5fe1bf
        }
    }
}