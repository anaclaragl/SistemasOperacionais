using System;
using System.Collections.Generic;
using System.IO;
<<<<<<< HEAD
using System.Text;
using databaseServer;
using MSMQ.Messaging;

/*CICLO DE FUNCIONAMENTO DO MSMQ
1. cliente chama um comando
2. cliente manda pro mq
3. server recebe o mq
4. server executa o mq
5. server manda a resposta de volta pro cliente
*/

/* Dessa forma, o banco de dados precisa ter um metodo pra
executar os comandos;
o cliente deve ter uma string que recebe a resposta e mostra no console
respostas devem ser a situacao da execução
*/

=======
using System.Threading;
>>>>>>> d092c2c5511acf3a893afddfd547cc0feb5fe1bf
class SimpleDB{ //classe do database do programa
    private Dictionary<int, string> database; //uso de dicionario para um banco de dados simples
    private string filePath;
    public SimpleDB(string fPath){ //metodo construtor
        this.filePath = fPath;
<<<<<<< HEAD
        LoadDataFromTxt(); //cada vez que o construtor for instanciada, ele coleta a data ja salva no documento
    }

    public string Insert(int key, string value) //metodo inserir no banco de dados
    {
        if (!database.ContainsKey(key))
        {
            database[key] = value;
            SaveDataToTxt();
            return "inserted";
        }
        else
        {
            return "key already exists. Try using the update method";
        }
    }

    public string Remove(int key) //metodo remover uma chave no banco de dados (removendo o valor tambem)
    {
            if (database.ContainsKey(key))
            {
                database.Remove(key);
                SaveDataToTxt();
                return "removed";
            }
            else
            {
                return "key not found.";
            }
    }

    public string Update(int key, string value) //metodo update, recebendo a chave a ser atualizada e o novo valor
    {
            if (database.ContainsKey(key))
            {
                database[key] = value;
                SaveDataToTxt();
                return "updated";
            }
            else
            {
                return "key not found";
            }
    }

    public string Search(int key) //metodo search se uma chave existe no banco de dados
    {
            if (database.ContainsKey(key))
            {
                return "Found object: " + database[key];
            }
            else
            {
                return "object key not found";
            }
    }
    private void SaveDataToTxt(){ //metodo que salva novos dados no banco
=======
        LoadData(); //cada vez que o construtor eh instanciada, ele coleta a data ja salva no documento
    }
    public void Insert(int key, string value){ //metodo inserir no banco de dados
        if (!database.ContainsKey(key)){
            database[key] = value;
            SaveData();
            Console.WriteLine("inserted");
        }else{
            Console.WriteLine("key already exists. Try using the update method");
        }
    }

    public void Remove(int key){ //metodo remover uma chave no banco de dados (removendo o valor tambem)
        if (database.ContainsKey(key)){
            database.Remove(key);
            SaveData();
            Console.WriteLine("removed");
        }else{
            Console.WriteLine("key not found.");
        }
    }
    public void Update(int key, string value){ //metodo update, recebendo a chave a ser atualizada e o novo valor
        if (database.ContainsKey(key)){
            database[key] = value;
            SaveData();
            Console.WriteLine("updated");
        }else{
            Console.WriteLine("key not found");
        }
    }
    public void Search(int key){ //metodo search se uma chave existe no banco de dados
        if (database.ContainsKey(key)){
            Console.WriteLine("Found object: " + database[key]);
        }else{
            Console.WriteLine("object key not found");
        }
    }
    private void SaveData(){ //metodo que salva novos dados no banco
>>>>>>> d092c2c5511acf3a893afddfd547cc0feb5fe1bf
        List<string> lines = new List<string>();
        foreach (var entry in database){
            lines.Add(entry.Key + "," + entry.Value);
        }
        File.WriteAllLines(filePath, lines);
    }
<<<<<<< HEAD
    private void LoadDataFromTxt(){ //metodo que carrega os dados do arquivo para o dicionario
=======
    private void LoadData(){ //metodo que carrega os dados do arquivo para o dicionario
>>>>>>> d092c2c5511acf3a893afddfd547cc0feb5fe1bf
        database = new Dictionary<int, string>(); //databse recebe um novo dicionario para preencher com os dados ja existentes

        if (File.Exists(filePath)){
            string[] lines = File.ReadAllLines(filePath); //uma array pra receber cada linha do metodo File.ReadAllLines
            foreach (string line in lines){
                string[] parts = line.Split(',');
                if (parts.Length == 2){
                    int key = Convert.ToInt32(parts[0]);
                    string value = parts[1];
                    database[key] = value;
                }
            }
        }
    }
<<<<<<< HEAD

    public string MessageExecute(Command command){
        switch(command.op){
            case Operation.Insert:
                return Insert(command.key, command.value);
            case Operation.Remove:
                return Remove(command.key);
            case Operation.Search:
                return Search(command.key);
            case Operation.Update:
                return Update(command.key, command.value);
            default:
                return "Not a valid command";
            //break;
        }


    }

=======
>>>>>>> d092c2c5511acf3a893afddfd547cc0feb5fe1bf
}