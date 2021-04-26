using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string dictionaryName;
    public string iconPath;
    public bool isFood;
    public bool isRepairResource;
    public float value;
    public int price;

    public bool IsUsable()
    {
        return isFood || isRepairResource;
    }
}

[System.Serializable]
public class ToolData
{
    public List<string> tierDictionaryName;
    public List<string> tierIconPath;
    public List<float> tierDurability;
    public List<int> tierPrice;

    public float GetMaxDurabilityByTier(int tier)
    {
        return tierDurability[tier];
    }

    public bool IsUpgradeable(int tier)
    {
        return tier < tierDurability.Count - 1;
    }
}

[System.Serializable]
public class CraftingComponent
{
    public bool isTool;
    public int id;
    public int value; // Quantity for items and tier for tools
}

[System.Serializable]
public class Recipe
{
    public CraftingComponent result;
    public List<CraftingComponent> components;
}

[System.Serializable]
public class Catch
{
    public int itedId;
    public int minTier;
    public float weight;
}

public class ItemModel : MonoBehaviour
{
    public List<ItemData> itemData;
    public List<ToolData> toolData;

    public List<Recipe> cookingRecipes;
    public List<Recipe> workbenchRecipes;
    public List<Recipe> forgeRecipes;
    public List<Catch> catchList;
    public List<Vector2Int> toolsOnSale;

    public static ItemModel instance;
    public Sprite defaultIcon;
    private Dictionary<string, Sprite> icons;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            PreloadResources();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void PreloadResources()
    {
        icons = new Dictionary<string, Sprite>();
        for (int i = 0; i < itemData.Count; ++i)
        {
            Sprite sprite = Resources.Load<Sprite>(itemData[i].iconPath);
            icons.Add(itemData[i].dictionaryName, sprite ? sprite : defaultIcon);
        }
        for (int i = 0; i < toolData.Count; ++i)
        {
            for (int j = 0; j < toolData[i].tierDictionaryName.Count; ++j)
            {
                Sprite sprite = Resources.Load<Sprite>(toolData[i].tierIconPath[j]);
                icons.Add(toolData[i].tierDictionaryName[j], sprite ? sprite : defaultIcon);
            }
        }
    }

    public Sprite GetItemIcon(int itemId)
    {
        return icons[itemData[itemId].dictionaryName];
    }

    public Sprite GetToolIcon(UsableTool tool)
    {
        return icons[toolData[tool.Id].tierDictionaryName[tool.Tier]];
    }

    public Sprite GetToolIcon(int toolId, int tier)
    {
        return icons[toolData[toolId].tierDictionaryName[tier]];
    }

    public float GetItemFlagsAndValue(int itemId, ref bool isFood, ref bool isRepairResource)
    {
        isFood = itemData[itemId].isFood;
        isRepairResource = itemData[itemId].isRepairResource;
        return itemData[itemId].value;
    }
}