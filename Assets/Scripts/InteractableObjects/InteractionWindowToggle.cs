using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWindowToggle : InteractableObject
{
    private void OnTriggerEnter(Collider other)
    {
        GameController.instance.SetInteractableObject(this);
    }

    private void OnTriggerExit(Collider other)
    {
        GameController.instance.DropInteractableObject(this);
    }

    public override void Interact()
    {
        GameController.instance.ToggleWindowVisibility();
    }
}