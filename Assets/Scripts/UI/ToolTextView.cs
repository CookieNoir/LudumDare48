using UnityEngine;
using UnityEngine.UI;

public class ToolTextView : ToolView
{
    public Text durabilityText;

    public override void ResetDurabilityValue()
    {
        durabilityText.text = tool.GetDurability();
    }
}