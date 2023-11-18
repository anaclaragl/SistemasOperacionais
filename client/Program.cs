using System;
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
        }
    }
}