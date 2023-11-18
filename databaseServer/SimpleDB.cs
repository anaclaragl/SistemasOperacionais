using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
class SimpleDB{ //classe do database do programa
    private Dictionary<int, string> database; //uso de dicionario para um banco de dados simples
    private string filePath;
    public SimpleDB(string fPath){ //metodo construtor
        this.filePath = fPath;
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
        List<string> lines = new List<string>();
        foreach (var entry in database){
            lines.Add(entry.Key + "," + entry.Value);
        }
        File.WriteAllLines(filePath, lines);
    }
    private void LoadData(){ //metodo que carrega os dados do arquivo para o dicionario
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
}