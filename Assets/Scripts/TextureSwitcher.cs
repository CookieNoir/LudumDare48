using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class TextureSwitcher : MonoBehaviour
{
    public Texture[] textures;
    public int materialIndex;
    public float timeStep;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().materials[materialIndex];
    }

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int currentTex = 0;
        while (true)
        {
            yield return new WaitForSeconds(timeStep);
            currentTex = (currentTex + 1) % textures.Length;
            material.mainTexture = textures[currentTex];
        }
    }
}