using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour
{
    public int Id { get; private set; }
    public Recipe Recipe { get; private set; }
    public Sprite ResultIcon { get; private set; }
    public Text itemName;
    public Image background;
    public Button button;

    public void SetRecipe(int id, int type)
    {
        Id = id;
        switch (type)
        {
            case 0:
                {
                    Recipe = ItemModel.instance.cookingRecipes[id];
                    break;
                }
            case 1:
                {
                    Recipe = ItemModel.instance.workbenchRecipes[id];
                    break;
                }
            case 2:
                {
                    Recipe = ItemModel.instance.forgeRecipes[id];
                    break;
                }
        }

        CraftingComponent result = Recipe.result;

        if (Recipe.result.isTool)
        {
            itemName.text = ItemModel.instance.toolData[result.id].tierDictionaryName[result.value];
            ResultIcon = ItemModel.instance.GetToolIcon(result.id, result.value);
        }
        else
        {
            itemName.text = itemName.text = ItemModel.instance.itemData[result.id].dictionaryName;
            ResultIcon = ItemModel.instance.GetItemIcon(result.id);
        }
    }

    public CraftingComponent[] GetCraftingComponents()
    {
        return Recipe.components.ToArray();
    }
}