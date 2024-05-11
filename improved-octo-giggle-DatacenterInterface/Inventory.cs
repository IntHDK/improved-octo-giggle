using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static improved_octo_giggle_DatacenterInterface.DataCenter;

namespace improved_octo_giggle_DatacenterInterface
{
    public partial interface DataCenter
    {
        //Base Inventory CRUD
        public class GetInventory_Result : DataActionResultBase
        {
            public ICollection<Inventory> Inventories { get; set; }
        }
        public class AddInventory_Result : DataActionResultBase { }
        public class UpdateInventory_Result : DataActionResultBase { }
        public class DeleteInventory_Result : DataActionResultBase { }
        public GetInventory_Result GetInventoryByID(string ID);
        public GetInventory_Result GetInventoriesByOwnerID(string OwnerID);
        public AddInventory_Result AddInventory(Inventory inventory);
        public UpdateInventory_Result UpdateInventory(Inventory inventory);
        public DeleteInventory_Result DeleteInventory(string ID);
        // Inventory atomic actions
        public bool TransferItemInInventory(string ItemID, string TargetInventoryID);

        //Base Item CRUD
        public class GetItem_Result : DataActionResultBase
        {
            public ICollection<Item> Items { get; set; }
        }
        public class AddItem_Result : DataActionResultBase { }
        public class UpdateItem_Result : DataActionResultBase { }
        public class DeleteItem_Result : DataActionResultBase { }
        public GetItem_Result GetItemByID(string ID);
        public GetItem_Result GetItemsByInventoryID(string InventoryID);
        public AddItem_Result AddItem(Item item);
        public UpdateItem_Result UpdateItem(Item item);
        public DeleteItem_Result DeleteItem(string ID);
        // Item atomic actions
        public class UpdateItemQuantity_Result : DataActionResultBase { }
        public class ModifyItemQuantity_Result : DataActionResultBase { }
        public UpdateItemQuantity_Result UpdateItemQuantity(string ItemID, int NewItemQuantity);
        public ModifyItemQuantity_Result ModifyItemQuantity(string ItemID, int ItemQuantityModifier);
    }
}
