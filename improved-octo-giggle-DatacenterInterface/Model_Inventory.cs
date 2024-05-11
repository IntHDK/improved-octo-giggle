using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace improved_octo_giggle_DatacenterInterface
{
    public class Inventory
    {
        public string ID { get; set; }
        public string OwnerID { get; set; }
        public ICollection<string> Item_IDs { get; set; }
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

        public string Owner_InventoryID { get; set; }
    }
}
