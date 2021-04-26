using UnityEngine;

public class InteractionWindow : MonoBehaviour
{
    public void ToggleWindowVisibility(bool value)
    {
        gameObject.SetActive(value);
        if (value)
        {
            SetValues();
        }
        else
        {
            DropValues();
        }
    }

    public void ToggleWindowVisibility()
    {
        bool value = !gameObject.activeSelf;
        gameObject.SetActive(value);
        if (value)
        {
            SetValues();
        }
        else
        {
            DropValues();
        }
    }

    protected virtual void SetValues()
    {

    }

    protected virtual void DropValues()
    {

    }
}