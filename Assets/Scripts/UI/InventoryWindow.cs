using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    public Transform content;
    public RepairWindow repairWindow;
    public GameObject itemButtonPrefab;
    public bool IsActive { get; private set; }
    private ItemButton[] items;
    private int selectedItemId;
    private PlayerResources playerResources;

    public void CreateInventory(PlayerResources newPlayerResources)
    {
        items = new ItemButton[ItemModel.instance.itemData.Count];
        for (int i = 0; i < ItemModel.instance.itemData.Count; ++i)
        {
            GameObject newButton = Instantiate(itemButtonPrefab, content);
            ItemButton itemButton = newButton.GetComponent<ItemButton>();

            itemButton.SetItem(i);
            itemButton.button.onClick.AddListener(() => SelectItem(itemButton.Id));
            items[i] = itemButton;
        }
        playerResources = newPlayerResources;
        IsActive = true;
    }

    public void RefreshVisibilityAndAmount()
    {
        for (int i = 0; i < items.Length; ++i)
        {
            bool isVisible = playerResources.itemInventory.ContainsItemWithId(i);
            items[i].gameObject.SetActive(isVisible);
            if (isVisible)
            {
                items[i].SetAmountText(playerResources.itemInventory.GetItemCountById(i));
            }
        }
    }

    public void SetActive(bool value)
    {
        IsActive = value;
        gameObject.SetActive(IsActive);
        if (IsActive)
        {
            RefreshVisibilityAndAmount();
        }
        else
        {
            repairWindow.gameObject.SetActive(false);
            selectedItemId = -1;
        }
    }

    public void SelectItem(int id)
    {
        selectedItemId = id;
        repairWindow.gameObject.SetActive(true);
        bool isFood = false;
        bool isRepairResource = false;
        float value = ItemModel.instance.GetItemFlagsAndValue(selectedItemId, ref isFood, ref isRepairResource);
        repairWindow.RefreshData(ItemModel.instance.GetItemIcon(id), isRepairResource, value);
    }

    public void UseSelectedItem()
    {
        if (selectedItemId > -1)
        {
            int remainder = playerResources.UseItemWithId(selectedItemId);
            items[selectedItemId].SetAmountText(remainder);
            GameController.instance.RefreshUI();
            if (remainder < 1)
            {
                items[selectedItemId].gameObject.SetActive(false);
                repairWindow.gameObject.SetActive(false);
                selectedItemId = -1;
            }
        }
    }
}