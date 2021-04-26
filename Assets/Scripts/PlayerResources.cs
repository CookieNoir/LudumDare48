public class PlayerResources
{
    public ItemInventory itemInventory;
    public UsableTool[] tools;
    public int Money { get; private set; }
    public int GearScore { get; private set; }

    public PlayerResources()
    {
        itemInventory = new ItemInventory();
        itemInventory.PickUpItemWithId(0, 10); // 10 Berries
        tools = new UsableTool[ItemModel.instance.toolData.Count];
        for (int i = 0; i < ItemModel.instance.toolData.Count; ++i)
        {
            tools[i] = new UsableTool(i, 0); // 0 - Boat, 1 - Food
        }
        Money = 0;
        RecalculateGearScore();
    }

    public void EarnMoney(int value)
    {
        Money += value;
    }

    public bool SpendMoney(int value)
    {
        if (Money >= value)
        {
            Money -= value;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RecalculateGearScore()
    {
        int score = 0;
        for (int i = 0; i < tools.Length; ++i)
        {
            score += tools[i].Tier;
        }
        GearScore = score;
    }

    public int UseItemWithId(int itemId)
    {
        if (ItemModel.instance.itemData[itemId].isRepairResource)
        {
            tools[0].Repair(ItemModel.instance.itemData[itemId].value);
        }

        if (ItemModel.instance.itemData[itemId].isFood)
        {
            tools[1].Repair(ItemModel.instance.itemData[itemId].value);
        }

        itemInventory.GiveItemWithId(itemId);
        return itemInventory.GetItemCountById(itemId);
    }

    public int HarvestItem(int itemId, int quantity, int toolId, int toolMinimumTier)
    {
        if (toolId > -1)
        {
            if (tools[toolId].MinimumTierReached(toolMinimumTier))
            {
                bool used = tools[toolId].UseTool();
                if (used)
                {
                    itemInventory.PickUpItemWithId(itemId, quantity);
                    return 0;
                }
                else
                {
                    return 1; // No durability
                }
            }
            else
            {
                return 2; // Tool is low
            }
        }
        else
        {
            itemInventory.PickUpItemWithId(itemId, quantity);
            return 0;
        }     
    }

    public void RepairAllTools()
    {
        for (int i = 2; i < tools.Length; ++i)
        {
            tools[i].Repair();
        }
    }
}