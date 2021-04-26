using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolView : MonoBehaviour
{
    public Image iconSlot;
    protected UsableTool tool;

    public void SetTrackableTool(UsableTool newTool)
    {
        tool = newTool;
        ResetIcon();
        ResetDurabilityValue();
    }

    public void ResetIcon()
    {
        iconSlot.sprite = ItemModel.instance.GetToolIcon(tool);
    }

    public virtual void ResetDurabilityValue()
    {

    }
}