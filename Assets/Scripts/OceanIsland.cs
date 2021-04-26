using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanIsland : InteractableObject
{
    public IslandTypes islandType;
    public int value;
    public SurfaceIsland surfaceIsland;

    public override void Interact()
    {
        GameController.instance.surfaceMovement.SetPosition(surfaceIsland.entry.position);
        GameController.instance.ChangeGameMod(false);
    }
}