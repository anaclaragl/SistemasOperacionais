using System;
using System.Collections.Generic;
using System.IO;
using databaseServer;
using MSMQ.Messaging;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

/*CICLO DE FUNCIONAMENTO DO MSMQ
1. cliente chama um comando
2. cliente manda pro mq
3. server recebe o mq
4. server executa o mq
5. server manda a resposta de volta pro cliente
*/

class SimpleDB{ //classe do database do programa
    private Dictionary<int, string> database; //uso de dicionario para um banco de dados simples
    private string filePath;
    private Mutex mutex;

    private const string CacheKeyPrefix = "DBCache_"; //o prefixo que vem antes de cada dado pra evitar conflito
    private IMemoryCache cache; //interface que permite a manipulação na memoria cache
    
    public SimpleDB(string fPath){ //metodo construtor
        filePath = fPath;
        mutex = new Mutex();
        LoadDataFromTxt(); //cada vez que o construtor for instanciada, ele coleta a data ja salva no documento

        cache = new MemoryCache(new MemoryCacheOptions()); //instnacia do memory cache
    }
    
    public string Insert(int key, string value){ //metodo inserir no banco de dados
        mutex.WaitOne(); //bloqueia a thread ate receber um sinal

        try{
            if (!database.ContainsKey(key)){
                database[key] = value;
                SaveDataToTxt();
                return "inserted";
            }else{
                return "key already exists. Try using the update method";
            }
        }finally{
            mutex.ReleaseMutex(); //libera o mutex pro proximo comando
        }
    }

    public string Remove(int key){ //metodo remover uma chave no banco de dados (removendo o valor tambem)
        mutex.WaitOne();

        try{
            if (database.ContainsKey(key)){
                database.Remove(key);
                SaveDataToTxt();
                return "removed";
            }else{
                return "key not found.";
            }
        }finally{
            mutex.ReleaseMutex();
        }
    }

    public string Update(int key, string value){ //metodo update, recebendo a chave a ser atualizada e o novo valor
        mutex.WaitOne();

        try{
            if (database.ContainsKey(key)){
                database[key] = value;
                SaveDataToTxt();
                return "updated";
            }else{
                return "key not found";
            }
        }finally{
            mutex.ReleaseMutex();
        }
    }

    public string Search(int key){ //metodo search se uma chave existe no banco de dados
        mutex.WaitOne();
        try{
            if (database.ContainsKey(key)){
                return "Found object: " + database[key];
            }else{
                return "object key not found";
            }
        }finally{
            mutex.ReleaseMutex();
        }
    }
    private void SaveDataToTxt(){ //metodo que salva novos dados no banco
        mutex.WaitOne();

        try{
            List<string> lines = new List<string>();

            foreach (var entry in database){
                lines.Add(entry.Key + "," + entry.Value);
            }

            File.WriteAllLines(filePath, lines);
        }finally{
            mutex.ReleaseMutex();
        }
    }
    private void LoadDataFromTxt(){ //metodo que carrega os dados do arquivo para o dicionario
        mutex.WaitOne();

        try{
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
        }finally{
            mutex.ReleaseMutex();
        }
    }    
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
        }
    }
}