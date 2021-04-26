using UnityEngine;
using UnityEngine.UI;

public class CraftingComponentUI : MonoBehaviour
{
    public MaskableGraphic background;
    public Text componentNameText;
    public Text amountText;

    public void SetContent(CraftingComponent craftingComponent, Color backgroundColor)
    {
        if (craftingComponent.isTool)
        {
            componentNameText.text = ItemModel.instance.toolData[craftingComponent.id].tierDictionaryName[craftingComponent.value];
            amountText.text = "";
        }
        else
        {
            componentNameText.text = ItemModel.instance.itemData[craftingComponent.id].dictionaryName;
            amountText.text = craftingComponent.value.ToString();
        }
        SetColor(backgroundColor);
    }

    public void SetColor(Color backgroundColor)
    {
        background.color = backgroundColor;
    }
}