using UnityEngine;

public class UsableTool
{
    public int Id { get; private set; }
    public int Tier { get; private set; }
    public string DictionaryName { get; private set; }
    private float currentDurability;
    private float maxDurability;
    private bool isUsable;
    private bool isUpgradeable;

    public UsableTool(int newId, int newTier)
    {
        Id = newId;
        SetTier(newTier);
    }

    public bool UseTool()
    {
        if (isUsable)
        {
            currentDurability--;
            if (currentDurability < 0)
            {
                currentDurability = 0;
                isUsable = false;
                return false;
            }
            else
            {
                isUsable = currentDurability > 0;
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public bool UseTool(float value)
    {
        if (isUsable)
        {
            currentDurability -= value;
            if (currentDurability < 0)
            {
                currentDurability = 0;
                isUsable = false;
                return false;
            }
            else
            {
                isUsable = currentDurability > 0;
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void Repair()
    {
        currentDurability = maxDurability;
        isUsable = currentDurability > 0f;
    }

    public void Repair(float value)
    {
        currentDurability += value;
        if (currentDurability > maxDurability) currentDurability = maxDurability;
        isUsable = currentDurability > 0f;
    }

    public bool Upgrade()
    {
        if (isUpgradeable)
        {
            SetTier(Tier + 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetTier(int newTier)
    {
        Tier = newTier;
        DictionaryName = ItemModel.instance.toolData[Id].tierDictionaryName[Tier];
        maxDurability = ItemModel.instance.toolData[Id].GetMaxDurabilityByTier(Tier);
        isUpgradeable = ItemModel.instance.toolData[Id].IsUpgradeable(Tier);
        Repair();
    }

    public bool AreTiersMatching(int newTier)
    {
        return Tier == newTier;
    }

    public bool MinimumTierReached(int newTier)
    {
        return Tier >= newTier;
    }

    public string GetDurability()
    {
        return Mathf.Ceil(currentDurability).ToString() + " / " + maxDurability.ToString();
    }

    public float GetDurabilityClipped()
    {
        return maxDurability > 0f ? currentDurability / maxDurability : 0f;
    }
}