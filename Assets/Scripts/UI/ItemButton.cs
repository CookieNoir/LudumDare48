using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public int Id { get; protected set; }
    public Text itemName;
    public Button button;
    public Text amount;

    public void SetItem(int id)
    {
        Id = id;
        itemName.text = ItemModel.instance.itemData[id].dictionaryName;
        button.interactable = ItemModel.instance.itemData[id].IsUsable();
    }

    public void SetAmountText(int newAmount)
    {
        amount.text = newAmount.ToString();
    }
}