using UnityEngine;

public class LeaveIsland : InteractableObject
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
        GameController.instance.ChangeGameMod(true);
    }
}