using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public InteractionTypes interactionType;
    public HintTypes hintType;

    public virtual void Interact()
    {
    }
}