using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceIslandPattern : MonoBehaviour
{
    public Transform[] entries;

    public SurfaceIsland CreateIsland()
    {
        SurfaceIsland surfaceIsland = new SurfaceIsland();
        surfaceIsland.entry = entries[Random.Range(0, entries.Length)];
        return surfaceIsland;
    }
}