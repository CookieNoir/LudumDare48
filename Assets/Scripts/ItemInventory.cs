public class ItemInventory
{
    private int[] itemsCount;

    public ItemInventory()
    {
        itemsCount = new int[ItemModel.instance.itemData.Count];
    }

    public int GetItemCountById(int id)
    {
        return itemsCount[id];
    }

    public void PickUpItemWithId(int id, int quantity = 1)
    {
        itemsCount[id] += quantity;
    }

    public bool GiveItemWithId(int id, int quantity = 1)
    {
        if (AreEnoughItems(id, quantity))
        {
            itemsCount[id] -= quantity;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ContainsItemWithId(int id)
    {
        return itemsCount[id] > 0;
    }

    public bool AreEnoughItems(int id, int quantity)
    {
        return itemsCount[id] >= quantity;
    }
}