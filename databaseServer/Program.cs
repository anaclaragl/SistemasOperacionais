using System;
using System.Collections.Generic;
using System.IO;
using MSMQ.Messaging;
using System.Threading;

namespace databaseServer{
    class Program{
    const string mqPathServer = ".\\Private$\\SimpleDBQueue"; //queue do servidor
    const string mqPathClient = ".\\Private$\\ClientQueue"; //queue do cliente
    private static bool keepRunning = true; //booleano para substituir o while(true)
    static void Main(string[] args){

        Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) { //eveneto que permite o servidor dar crtl+c e fechar o programa
            e.Cancel = true;
            Program.keepRunning = false; //deixa falso a variavel que esta rodando o programa
        };

        SimpleDB database = new SimpleDB(10,"simpledb.txt"); //nova instancia do banco de dados, capacidade de dados é 10

        if(args.Length > 0){ //apenas para quando for executar algum comando pelo servidor
            ReadLineArgs(args, database);
            return;
        }

        //verifica se o queue existe, se nao: cria uma nova fila
        if(!MessageQueue.Exists(mqPathServer)){
            MessageQueue.Create(mqPathServer);
        }

        //abertura e tipificacao do message queue do servidor
        MessageQueue messageQueueServer = new MessageQueue(mqPathServer){
            Formatter = new XmlMessageFormatter(new Type[] { typeof(Command) })
        };

        Console.WriteLine("In progress...");
        Console.WriteLine("Waiting for command...");

        while(Program.keepRunning){
            /*receber
            executar
            fechar*/


            Thread thread = new Thread(() =>
            {
                Message msg = messageQueueServer.Receive();
                Console.WriteLine("Command recieved!");
                Command command = (Command)msg.Body; //pega o body do comando pra definir a execucao

                Console.WriteLine("Sending to client...");
                SendToClient(command, database);

                messageQueueServer.Close();
            });

            thread.Start(); //inicializacao da thread

        }
        Console.WriteLine("server exited");
        Environment.Exit(0); //fecha o processo do terminal
    }

    static public void SendToClient(Command command, SimpleDB database){ //funcao para executar e receber a resposta de volta

        string answer = database.MessageExecute(command);
        Console.WriteLine("Executing...");
        Message msg = new Message(answer);

        //abertura e tipificacao do message queue do client
        MessageQueue messageQueueClient = new MessageQueue(mqPathClient){
            Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
        };
        
        messageQueueClient.Send(msg); //enviando a resposta pro cliente
        messageQueueClient.Close();
    }
        

    static public void ReadLineArgs(string[] args, SimpleDB database){ //classe de leitura caso precise inserir algo direto do servidor

        string action = args[0].ToLower(); //variavel que recebe os comandos
        string[] inputs = args[1].Split(','); //divisao entre chave-valor, [0] -> chave, [1] -> valor

        Command command = new Command();

        command.key = Convert.ToInt32(inputs[0]); //ja define a key, pois todos os comandos requisitam a chave

        switch (action){
                case "--insert":
                    if (inputs.Length != 2){
                        Console.WriteLine("Wrong insert format. Try using: --insert key,value");
                    }else{
                        command.value = inputs[1];
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
                        command.value = inputs[1];
                        command.op = Operation.Update;
                    }
                    break;
                case "quit":
                    //deleta a fila do cliente
                    if (MessageQueue.Exists(mqPathServer))
                    {
                        MessageQueue.Delete(mqPathServer);
                    }
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Command. Avaliable Commands: --insert, remove, search, update");
                    break;
            }
            string answer = database.MessageExecute(command);
            Console.WriteLine(answer);
        }
    }
}
