using Microsoft.Extensions.Caching.Memory;
using MSMQ.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace databaseServer
{
    public class SimpleDB{ //classe do database do program
        private Dictionary<int, string> database; //uso do dicionario para um banco de dados simples
        private readonly MemoryCache cache; //agora usando a implementacao da anterior interface
        private readonly Queue<int> fifoQueue = new Queue<int>(); //queue do fifo
        private Mutex mutex = new Mutex();
        private const string CacheKeyPrefix = "DBCache_"; //o prefixo que vem antes de cada dado para evitar conflito
        private int capacity; //numero de dados que o sistema suporta
        private string filePath;

        public SimpleDB(int capacity, string filePath){ //metodo construtor
            this.capacity = capacity;
            this.filePath = filePath;
            LoadDataFromTxt(); //cada vez que o construtor for instaciado, ele coleta a data ja salva no documento
            
            var cacheOptions = new MemoryCacheOptions(); //
            cache = new MemoryCache(cacheOptions);
            database = new Dictionary<int, string>();
        }

        public string Insert(int key, string value){ //metodo inserir no banco de dados
            mutex.WaitOne(); //bloqueia a thread ate receber um sinal

            try{
                var cacheKey = CacheKeyPrefix + key;
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    Size = 1,
                    Priority = CacheItemPriority.NeverRemove
                };
                cache.Set(cacheKey, value, cacheEntryOptions); //inclui o novo dado na memoria cache

                if (!database.ContainsKey(key)){
                    database[key] = value;
                    fifoQueue.Enqueue(key);

                    if (fifoQueue.Count > capacity){ //fifo checa a capacidade, caso esteja cheio, ele retira o dado mais velho
                        var oldestKey = fifoQueue.Dequeue();
                        var oldestCacheKey = CacheKeyPrefix + oldestKey;
                        cache.Remove(oldestCacheKey);
                        database.Remove(oldestKey);
                    }

                    SaveDataToTxt();

                    return "inserted";
                }else{
                    return "key already exists. Try using the update method.";
                }
            }finally{
                mutex.ReleaseMutex(); //libera o mutex pro proximo comando
            }
        }

        public string Remove(int key){ //metodo remover uma chave no banco de dados(removendo o valor tambem)
            mutex.WaitOne();

            try{
                var cacheKey = CacheKeyPrefix + key;
                cache.Remove(cacheKey);

                if (database.ContainsKey(key)){
                    database.Remove(key);

                    if (fifoQueue.Contains(key)){
                        var newQueue = new Queue<int>(fifoQueue.Where(item => item != key)); //nova queue
                        fifoQueue.Clear(); //queue original se limpa
                        foreach (var item in newQueue){
                            fifoQueue.Enqueue(item); //queue original eh repreenchida
                        }
                        //tivemos que fazer essa nova queue pra prencher o fifo pq o fifo original Ã© readonly
                        //(so pode ser declarado uma vez), entao o new Queue Where nao funciona
                    }

                    SaveDataToTxt();

                    return "removed";
                }else{
                    return "key not found";
                }
            }finally{
                mutex.ReleaseMutex();
            }
        }

        public string Update(int key, string newValue){ //metodo update, recebendo a chave a ser atualizada e o novo valor
            mutex.WaitOne();

            try{
                var cacheKey = CacheKeyPrefix + key;
                if (cache.TryGetValue(cacheKey, out var cachedValue)){
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        Size = 1
                    };
                    cache.Set(cacheKey, newValue, cacheEntryOptions);
                }

                if (database.ContainsKey(key)){
                    database[key] = newValue; //atualiza no banco de dados

                    if (fifoQueue.Contains(key)){
                        var newQueue = new Queue<int>(fifoQueue.Where(item => item != key));
                        fifoQueue.Clear();
                        foreach (var item in newQueue){
                            fifoQueue.Enqueue(item);
                        }
                        fifoQueue.Enqueue(key);
                    }

                    SaveDataToTxt();

                    return "updated";
                }else{
                    return "key not found";
                }
            }finally{
                mutex.ReleaseMutex();
            }
        }
        public string Search(int key){ //metodo search se um chave existe no banco de dados
            mutex.WaitOne();

            try{
                var cacheKey = CacheKeyPrefix + key;
                if (cache.TryGetValue(cacheKey, out var cachedValue)){
                    return "found object in cache: " + cachedValue;
                }else{
                    LoadDataFromTxt();

                    if (database.ContainsKey(key)){
                        string value = database[key];
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            Size = 1
                        };
                        cache.Set(cacheKey, value, cacheEntryOptions);

                        return "found object in database: " + value;
                    }else{
                        return "object key not found";
                    }
                }
            }finally{
                mutex.ReleaseMutex();
            }
        }

        private void SaveDataToTxt(){ //metodo que salva novos dados no banco
            mutex.WaitOne();

            try{
                List<string> lines = new List<string>();

                foreach (var entry in database)
                {
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
                database.Clear(); //limpa o dicionario pra usalo dnv

                if (File.Exists(filePath)){
                    string[] lines = File.ReadAllLines(filePath); //uma array para receber cada linha do metodo File.ReadAllLines

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
        public string MessageExecute(Command command){ //metodo de execucao dos comandos vindos do cliente
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

}