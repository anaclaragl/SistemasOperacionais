using System;
using MSMQ.Messaging;

namespace client{

    class Program{
        
        const string mqPathClient = ".\\Private$\\ClientQueue"; //queue do cliente
        const string mqPathServer = ".\\Private$\\SimpleDBQueue"; //queue do server
        static void Main(string[] args){

            //verifica a existencia da queue do cliente
            if(!MessageQueue.Exists(mqPathClient)){
                MessageQueue.Create(mqPathClient);
            }

                Console.Write("//simpledb> ");

                //comandos do cliente
                string? entry = Console.ReadLine();
                string[] inputs = entry.Split(' '); //inputs[0] == comando; inputs[1] == chave,valor

                string action = inputs[0].ToLower();

                Command command = new Command(); //nova instancia de um comando pra definir a acao

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

                //ja define a key do comando, pois todo comando precisa de uma chave
                string[] separateInput = inputs[1].Split(',');
                command.key = Convert.ToInt32(separateInput[0]);

                //checa se o o input tambem tem o valor e define o value do comando
                if(separateInput.Length > 1){
                    command.value = separateInput[1];
                }

                //abertura e tipificacao das filas do servidor e cliente
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

                    string cliMsg = (string)messageQueueCli.Receive().Body; //trasforma em string o body(que era do tipo Message)
                    Console.WriteLine("Response recieved!");

                    messageQueueCli.Close();
                    messageQueueServer.Close();

                    Console.WriteLine(cliMsg);
                }catch (Exception e){
                    Console.WriteLine("error: " + e.Message);
                }

                //deleta a fila do cliente
                if (MessageQueue.Exists(mqPathClient)){
                    MessageQueue.Delete(mqPathClient);
                }
        }
    }
}