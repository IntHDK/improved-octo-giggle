using improved_octo_giggle_DatacenterInterface;

namespace improved_octo_giggle_server.Services.DataCenter_LocalModule
{
    public partial class DataCenterLocal : DataCenter
    {
        public DataCenter.GetInventory_Result GetInventoryByID(string ID)
        {
            throw new NotImplementedException();
        }

        public DataCenter.GetInventory_Result GetInventoriesByOwnerID(string OwnerID)
        {
            throw new NotImplementedException();
        }

        public DataCenter.AddInventory_Result AddInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public DataCenter.UpdateInventory_Result UpdateInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public DataCenter.DeleteInventory_Result DeleteInventory(string ID)
        {
            throw new NotImplementedException();
        }

        public bool TransferItemInInventory(string ItemID, string TargetInventoryID)
        {
            throw new NotImplementedException();
        }

        public DataCenter.GetItem_Result GetItemByID(string ID)
        {
            throw new NotImplementedException();
        }

        public DataCenter.GetItem_Result GetItemsByInventoryID(string InventoryID)
        {
            throw new NotImplementedException();
        }

        public DataCenter.AddItem_Result AddItem(Item item)
        {
            throw new NotImplementedException();
        }

        public DataCenter.UpdateItem_Result UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }

        public DataCenter.DeleteItem_Result DeleteItem(string ID)
        {
            throw new NotImplementedException();
        }

        public DataCenter.UpdateItemQuantity_Result UpdateItemQuantity(string ItemID, int NewItemQuantity)
        {
            throw new NotImplementedException();
        }

        public DataCenter.ModifyItemQuantity_Result ModifyItemQuantity(string ItemID, int ItemQuantityModifier)
        {
            throw new NotImplementedException();
        }
    }
}
