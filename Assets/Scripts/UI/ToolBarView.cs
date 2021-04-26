using UnityEngine;
using UnityEngine.UI;

public class ToolBarView : ToolView
{
    public Image durabilityBar;

    public override void ResetDurabilityValue()
    {
        durabilityBar.fillAmount = tool.GetDurabilityClipped();
    }
}