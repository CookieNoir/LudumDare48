using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWindow : MonoBehaviour
{
    public Transform content;
    public GameObject recipeButtonPrefab;
    public Sprite placeholderIcon;
    public Image resultIcon;

    public Color colorMatch;
    public Color colorDontMatch;

    public Transform componentListContent;
    public GameObject craftingComponentPrefab;
    public GameObject craftButton;
    private RecipeButton[] cookingRecipes;
    private RecipeButton[] workbenchRecipes;
    private RecipeButton[] forgeRecipes;
    private RecipeButton[] workingArray;

    private CraftingComponent[] craftingComponents;
    private CraftingComponentUI[] craftingComponentUIs;

    private Recipe selectedRecipe;
    private int craftingWindowType;
    private PlayerResources playerResources;

    public void CreateCraftingWindow(PlayerResources newPlayerResources)
    {
        cookingRecipes = new RecipeButton[ItemModel.instance.cookingRecipes.Count];
        for (int i = 0; i < ItemModel.instance.cookingRecipes.Count; ++i)
        {
            GameObject newButton = Instantiate(recipeButtonPrefab, content);
            RecipeButton recipeButton = newButton.GetComponent<RecipeButton>();

            recipeButton.SetRecipe(i, 0);
            recipeButton.button.onClick.AddListener(() => SelectItem(recipeButton.Id));
            cookingRecipes[i] = recipeButton;
            cookingRecipes[i].gameObject.SetActive(false);
        }
        workbenchRecipes = new RecipeButton[ItemModel.instance.workbenchRecipes.Count];
        for (int i = 0; i < ItemModel.instance.workbenchRecipes.Count; ++i)
        {
            GameObject newButton = Instantiate(recipeButtonPrefab, content);
            RecipeButton recipeButton = newButton.GetComponent<RecipeButton>();

            recipeButton.SetRecipe(i, 1);
            recipeButton.button.onClick.AddListener(() => SelectItem(recipeButton.Id));
            workbenchRecipes[i] = recipeButton;
            workbenchRecipes[i].gameObject.SetActive(false);
        }
        forgeRecipes = new RecipeButton[ItemModel.instance.forgeRecipes.Count];
        for (int i = 0; i < ItemModel.instance.forgeRecipes.Count; ++i)
        {
            GameObject newButton = Instantiate(recipeButtonPrefab, content);
            RecipeButton recipeButton = newButton.GetComponent<RecipeButton>();

            recipeButton.SetRecipe(i, 2);
            recipeButton.button.onClick.AddListener(() => SelectItem(recipeButton.Id));
            forgeRecipes[i] = recipeButton;
            forgeRecipes[i].gameObject.SetActive(false);
        }
        playerResources = newPlayerResources;
    }

    public void SelectItem(int id)
    {
        ClearComponentsList();
        craftingComponents = null;

        selectedRecipe = workingArray[id].Recipe;
        craftingComponents = workingArray[id].GetCraftingComponents();
        resultIcon.sprite = workingArray[id].ResultIcon;

        int matchingCount = 0;
        craftingComponentUIs = new CraftingComponentUI[craftingComponents.Length];
        for (int i = 0; i < craftingComponents.Length; ++i)
        {
            bool matching = IsComponentEnough(craftingComponents[i]);
            GameObject newComponentUI = Instantiate(craftingComponentPrefab, componentListContent);
            craftingComponentUIs[i] = newComponentUI.GetComponent<CraftingComponentUI>();
            if (matching)
            {
                matchingCount++;
                craftingComponentUIs[i].SetContent(craftingComponents[i], colorMatch);
            }
            else
            {
                craftingComponentUIs[i].SetContent(craftingComponents[i], colorDontMatch);
            }
        }
        craftButton.SetActive(matchingCount == craftingComponents.Length);
    }

    public void SetActive(bool value, int newCraftingWindowType)
    {
        craftingWindowType = newCraftingWindowType;
        if (value)
        {
            resultIcon.sprite = placeholderIcon;
            craftButton.SetActive(false);
            switch (craftingWindowType)
            {
                case 0:
                    {
                        workingArray = cookingRecipes;
                        break;
                    }
                case 1:
                    {
                        workingArray = workbenchRecipes;
                        playerResources.RepairAllTools();
                        GameController.instance.RefreshUI();
                        break;
                    }
                case 2:
                    {
                        workingArray = forgeRecipes;
                        break;
                    }
            }
            RedrawRecipeList();
        }
        else
        {
            if (workingArray != null)
            {
                for (int i = 0; i < workingArray.Length; ++i)
                {
                    workingArray[i].gameObject.SetActive(false);
                }
            }
            selectedRecipe = null;
            workingArray = null;
        }
    }

    private bool IsComponentEnough(CraftingComponent craftingComponent)
    {
        bool matching = false;
        if (craftingComponent.isTool)
        {
            matching = playerResources.tools[craftingComponent.id].AreTiersMatching(craftingComponent.value);
        }
        else
        {
            matching = playerResources.itemInventory.AreEnoughItems(craftingComponent.id, craftingComponent.value);
        }
        return matching;
    }

    public void Craft()
    {
        if (selectedRecipe.result.isTool)
        {
            playerResources.tools[selectedRecipe.result.id].Upgrade();
            GameController.instance.toolViews[selectedRecipe.result.id].ResetIcon();
        }
        else
        {
            playerResources.itemInventory.PickUpItemWithId(selectedRecipe.result.id, selectedRecipe.result.value);

        }
        for (int i = 0; i < craftingComponents.Length; ++i)
        {
            if (!craftingComponents[i].isTool)
            {
                playerResources.itemInventory.GiveItemWithId(craftingComponents[i].id, craftingComponents[i].value);
            }
        }
        GameController.instance.RefreshUI();
        GameController.instance.RefreshInventory();
        RedrawRecipeList();
        RedrawComponentListAndCraftButton();
    }

    private void RedrawRecipeList()
    {
        for (int i = 0; i < workingArray.Length; ++i)
        {
            CraftingComponent[] components = workingArray[i].GetCraftingComponents();
            int matching = 0;
            for (int j = 0; j < components.Length; ++j)
            {
                if (IsComponentEnough(components[j])) matching++;
            }
            workingArray[i].background.color = matching == components.Length ? colorMatch : colorDontMatch;
            workingArray[i].gameObject.SetActive(true);
        }
    }

    private void RedrawComponentListAndCraftButton()
    {
        int matchingCount = 0;
        for (int i = 0; i < craftingComponents.Length; ++i)
        {
            bool matching = IsComponentEnough(craftingComponents[i]);
            if (matching)
            {
                matchingCount++;
                craftingComponentUIs[i].SetColor(colorMatch);
            }
            else
            {
                craftingComponentUIs[i].SetColor(colorDontMatch);
            }
        }
        craftButton.SetActive(matchingCount == craftingComponents.Length);
    }

    private void ClearComponentsList()
    {
        if (craftingComponentUIs != null)
        {
            int length = craftingComponentUIs.Length;
            for (int i = 0; i < length; ++i)
            {
                Destroy(craftingComponentUIs[i].gameObject);
            }
        }
    }
}