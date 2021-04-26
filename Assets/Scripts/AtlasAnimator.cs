using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class AtlasAnimator : MonoBehaviour
{
    public int materialIndex;
    public Vector2Int atlasSize;
    public float timeStep;
    private Vector2 atlasSizeInverted;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().materials[materialIndex];
        atlasSizeInverted = new Vector2(1f / atlasSize.x, -1f / atlasSize.y);
    }

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int currentTile = 0;
        int maxTile = atlasSize.x * atlasSize.y;
        while (true)
        {
            yield return new WaitForSeconds(timeStep);
            currentTile = (currentTile + 1) % maxTile;
            int row = currentTile / atlasSize.x;
            int col = currentTile % atlasSize.x;
            material.mainTextureOffset = new Vector2(atlasSizeInverted.x * col, atlasSizeInverted.y * row);
        }
    }
}