namespace improved_octo_giggle_server.Data.Entity
{
    public class Inventory
    {
        public string ID { get; set; }
        public Account Owner { get; set; }
        public ICollection<Item> Items { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class Item
    {
        public string ID { get; set; }
        public string MetaID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
        public int Quantity { get; set; }
        public bool Transferable { get; set; }
        public Dictionary<string, int> Value_Int { get; set; }
        public Dictionary<string, string> Value_String { get; set; }

        public Inventory Owner { get; set; }
    }
}
