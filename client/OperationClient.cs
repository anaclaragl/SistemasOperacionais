namespace client{
    public enum Operation{ //operacoes possiveis do cliente para o servidor
        Insert,
        Remove,
        Search,
        Update
    }

    public class Command{
        public Operation op; //tipo de operacao
        public int key; //chave, sempre requisitada
        public string? value; //valor, as vezes requisitado
    }

}