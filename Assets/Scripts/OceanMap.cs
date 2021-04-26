using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMap : MonoBehaviour
{
    public Vector2 ceilSize;
    public Vector2Int mapSize;
    public Vector2Int startCeil;
    public Vector2Int finalIslandCeil;
    public OceanIsland[] presetIslands;
    public OceanIsland finalIsland;
    public OceanMapBorders oceanMapBorders;
    private int[,] matrix;

    public Vector3 GenerateMap()
    {
        Vector2 ceilSizeXbounds = new Vector2(0.15f * ceilSize.x, 0.85f * ceilSize.x);
        Vector2 ceilSizeYbounds = new Vector2(0.15f * ceilSize.y, 0.85f * ceilSize.y);

        matrix = new int[mapSize.x + 2, mapSize.y + 2];
        int presetIslandsIndex = presetIslands.Length;
        int x = finalIslandCeil.x + 1;
        int y = finalIslandCeil.y + 1;
        SetQuad(presetIslandsIndex + 1, x, y);
        finalIsland.transform.position =
            new Vector3(
                x * ceilSize.x - Random.Range(ceilSizeXbounds.x, ceilSizeXbounds.y),
                y * ceilSize.y - Random.Range(ceilSizeYbounds.x, ceilSizeYbounds.y),
                0f);

        while (presetIslandsIndex > 0)
        {
            x = Random.Range(1, mapSize.x);
            y = Random.Range(1, mapSize.y);
            if (GetNeighborCount(x, y) == 0)
            {
                SetQuad(presetIslandsIndex, x, y);
                presetIslandsIndex--;
                presetIslands[presetIslandsIndex].transform.position = 
                    new Vector3(
                        x * ceilSize.x - Random.Range(ceilSizeXbounds.x, ceilSizeXbounds.y),
                        y * ceilSize.y - Random.Range(ceilSizeYbounds.x, ceilSizeYbounds.y),
                        0f);
            }
        }
        oceanMapBorders.CreateMesh(new Vector2(ceilSize.x * -1f, ceilSize.y * -1f), new Vector2(ceilSize.x * (mapSize.x + 1), ceilSize.y * (mapSize.y + 1)), ceilSize);
        return new Vector3(
            startCeil.x * ceilSize.x - ceilSize.x * 0.5f,
            startCeil.y * ceilSize.y - ceilSize.y * 0.5f,
            0f);
    }

    private int GetNeighborCount(int x, int y)
    {
        int count = 0;
        count += matrix[x - 1, y - 1];
        count += matrix[x    , y - 1];
        count += matrix[x + 1, y - 1];
        count += matrix[x - 1, y    ];
        count += matrix[x    , y    ];
        count += matrix[x + 1, y    ];
        count += matrix[x - 1, y + 1];
        count += matrix[x    , y + 1];
        count += matrix[x + 1, y + 1];
        return count;
    }

    private void SetQuad(int value, int x, int y)
    {
        matrix[x - 1, y - 1] = value;
        matrix[x    , y - 1] = value;
        matrix[x + 1, y - 1] = value;
        matrix[x - 1, y    ] = value;
        matrix[x    , y    ] = value;
        matrix[x + 1, y    ] = value;
        matrix[x - 1, y + 1] = value;
        matrix[x    , y + 1] = value;
        matrix[x + 1, y + 1] = value;
    }

    public Vector3 GetBorder()
    {
        return new Vector3(mapSize.x * ceilSize.x, mapSize.y * ceilSize.y, 0f);
    }

    public OceanIsland GetIslandByCoordinates(Vector3 coordinates)
    {
        int x = Mathf.CeilToInt(coordinates.x / ceilSize.x);
        int y = Mathf.CeilToInt(coordinates.y / ceilSize.y);
        int index = matrix[x, y] - 1;
        if (index < 0) return null;
        else
        {
            if (index < presetIslands.Length) return presetIslands[index];
            else return finalIsland;
        }
    }
}