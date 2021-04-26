using UnityEngine;

public class ResourceSpot : InteractableObject
{
    public int resourceId;
    public Vector2Int range;
    public int toolId;
    public int toolMinimumTier;

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
        GameController.instance.HarvestItem(resourceId, range, toolId, toolMinimumTier);
    }
}