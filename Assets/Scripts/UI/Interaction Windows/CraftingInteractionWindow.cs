using UnityEngine;

public class CraftingInteractionWindow : InteractionWindow
{
    public CraftingWindow craftingWindow;
    [Range(0, 2)] public int craftingWindowType; // 0 - Cooking, 1 - Workbench, 2 - Forge

    protected override void SetValues()
    {
        craftingWindow.SetActive(true, craftingWindowType);
    }

    protected override void DropValues()
    {
        craftingWindow.SetActive(false, craftingWindowType);
    }
}