namespace client{
    public enum Operation{
        Insert,
        Remove,
        Search,
        Update
    }

    public class Command{
        public Operation op;
        public int key;
        public string? value;
    }

}