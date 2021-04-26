using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OceanMapBorders : MonoBehaviour
{
    public Vector2 borderSize;
    public Vector2 lineSize;
    public float ceilSizeMultiplier;
    [Range(0f, 1f)] public float lineRelativePosition;
    private Mesh mapMesh;
    private MeshFilter meshFilter;

    private void Awake()
    {
        mapMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
    }

    public void CreateMesh(Vector2 lowerBorder, Vector2 upperBorder, Vector2 ceilSize)
    {
        Vector2 offset = ceilSizeMultiplier * ceilSize;
        Vector3[] vertices = new Vector3[28];
        Vector2[] uvs = new Vector2[28];
        Color[] colors = Enumerable.Repeat(Color.green, 28).ToArray();
        int[] triangles1 = new int[72];

        uvs[0] = new Vector2(lowerBorder.x - borderSize.x, upperBorder.y + borderSize.y);
        uvs[1] = new Vector2(upperBorder.x + borderSize.x, upperBorder.y + borderSize.y);
        uvs[2] = new Vector2(upperBorder.x + borderSize.x, lowerBorder.y - borderSize.y);
        uvs[3] = new Vector2(lowerBorder.x - borderSize.x, lowerBorder.y - borderSize.y);

        uvs[4] = new Vector2(lowerBorder.x, upperBorder.y);
        uvs[5] = new Vector2(upperBorder.x, upperBorder.y);
        uvs[6] = new Vector2(upperBorder.x, lowerBorder.y);
        uvs[7] = new Vector2(lowerBorder.x, lowerBorder.y);

        uvs[8] = new Vector2(lowerBorder.x + offset.x, upperBorder.y - offset.y);
        uvs[9] = new Vector2(upperBorder.x - offset.x, upperBorder.y - offset.y);
        uvs[10] = new Vector2(upperBorder.x - offset.x, lowerBorder.y + offset.y);
        uvs[11] = new Vector2(lowerBorder.x + offset.x, lowerBorder.y + offset.y);

        uvs[12] = new Vector2(lowerBorder.x - borderSize.x - offset.x, upperBorder.y + borderSize.y + offset.y);
        uvs[13] = new Vector2(upperBorder.x + borderSize.x + offset.x, upperBorder.y + borderSize.y + offset.y);
        uvs[14] = new Vector2(upperBorder.x + borderSize.x + offset.x, lowerBorder.y - borderSize.y - offset.y);
        uvs[15] = new Vector2(lowerBorder.x - borderSize.x - offset.x, lowerBorder.y - borderSize.y - offset.y);

        uvs[16] = uvs[4] * 0.5f;
        uvs[17] = uvs[5] * 0.5f;
        uvs[18] = uvs[6] * 0.5f;
        uvs[19] = uvs[7] * 0.5f;

        for (int i = 0; i < 16; ++i)
        {
            vertices[i] = uvs[i];
        }

        for (int i = 0; i < 8; ++i)
        {
            colors[i] += Color.red;
        }

        for (int i = 8; i < 12; ++i)
        {
            colors[i] = Color.black;
        }

        vertices[16] = vertices[4] + Vector3.forward;
        vertices[17] = vertices[5] + Vector3.forward;
        vertices[18] = vertices[6] + Vector3.forward;
        vertices[19] = vertices[7] + Vector3.forward;

        int i18;
        for (int i = 0; i < 3; ++i)
        {
            i18 = 18 * i;
            triangles1[i18] = i;
            triangles1[i18 + 1] = i + 1;
            triangles1[i18 + 2] = i + 4;
            triangles1[i18 + 3] = i + 1;
            triangles1[i18 + 4] = i + 5;
            triangles1[i18 + 5] = i + 4;

            triangles1[i18 + 6] = i + 4;
            triangles1[i18 + 7] = i + 9;
            triangles1[i18 + 8] = i + 8;
            triangles1[i18 + 9] = i + 4;
            triangles1[i18 + 10] = i + 5;
            triangles1[i18 + 11] = i + 9;

            triangles1[i18 + 12] = i;
            triangles1[i18 + 13] = i + 12;
            triangles1[i18 + 14] = i + 13;
            triangles1[i18 + 15] = i;
            triangles1[i18 + 16] = i + 13;
            triangles1[i18 + 17] = i + 1;
        }

        i18 = 18 * 3;
        triangles1[i18] = 3;
        triangles1[i18 + 1] = 4;
        triangles1[i18 + 2] = 7;
        triangles1[i18 + 3] = 3;
        triangles1[i18 + 4] = 0;
        triangles1[i18 + 5] = 4;

        triangles1[i18 + 6] = 7;
        triangles1[i18 + 7] = 8;
        triangles1[i18 + 8] = 11;
        triangles1[i18 + 9] = 7;
        triangles1[i18 + 10] = 4;
        triangles1[i18 + 11] = 8;

        triangles1[i18 + 12] = 3;
        triangles1[i18 + 13] = 12;
        triangles1[i18 + 14] = 0;
        triangles1[i18 + 15] = 15;
        triangles1[i18 + 16] = 12;
        triangles1[i18 + 17] = 3;

        int[] triangles2 = new int[6];
        triangles2[0] = 16;
        triangles2[1] = 17;
        triangles2[2] = 19;
        triangles2[3] = 17;
        triangles2[4] = 18;
        triangles2[5] = 19;

        Vector2 halfSize = lineRelativePosition * borderSize;

        uvs[20] = new Vector2(lowerBorder.x - halfSize.x - lineSize.x, upperBorder.y + halfSize.y + lineSize.y);
        uvs[21] = new Vector2(upperBorder.x + halfSize.x + lineSize.x, upperBorder.y + halfSize.y + lineSize.y);
        uvs[22] = new Vector2(upperBorder.x + halfSize.x + lineSize.x, lowerBorder.y - halfSize.y - lineSize.y);
        uvs[23] = new Vector2(lowerBorder.x - halfSize.x - lineSize.x, lowerBorder.y - halfSize.y - lineSize.y);
        uvs[24] = new Vector2(lowerBorder.x - halfSize.x + lineSize.x, upperBorder.y + halfSize.y - lineSize.y);
        uvs[25] = new Vector2(upperBorder.x + halfSize.x - lineSize.x, upperBorder.y + halfSize.y - lineSize.y);
        uvs[26] = new Vector2(upperBorder.x + halfSize.x - lineSize.x, lowerBorder.y - halfSize.y + lineSize.y);
        uvs[27] = new Vector2(lowerBorder.x - halfSize.x + lineSize.x, lowerBorder.y - halfSize.y + lineSize.y);

        for (int i = 20; i < 28; ++i)
        {
            vertices[i] = new Vector3(uvs[i].x, uvs[i].y, -1f);
        }

        for (int i = 24; i < 28; ++i)
        {
            colors[i] = Color.black;
        }

        int[] triangles3 = new int[24];

        for (int i = 0; i < 3; ++i)
        {
            i18 = 6 * i;
            triangles3[i18] = i + 20;
            triangles3[i18 + 1] = i + 21;
            triangles3[i18 + 2] = i + 24;
            triangles3[i18 + 3] = i + 21;
            triangles3[i18 + 4] = i + 25;
            triangles3[i18 + 5] = i + 24;
        }

        i18 = 18;
        triangles3[i18] = 23;
        triangles3[i18 + 1] = 24;
        triangles3[i18 + 2] = 27;
        triangles3[i18 + 3] = 23;
        triangles3[i18 + 4] = 20;
        triangles3[i18 + 5] = 24;

        mapMesh.subMeshCount = 3;
        mapMesh.vertices = vertices;
        mapMesh.uv = uvs;
        mapMesh.colors = colors;
        mapMesh.SetTriangles(triangles1, 0);
        mapMesh.SetTriangles(triangles2, 1);
        mapMesh.SetTriangles(triangles3, 2);
        meshFilter.mesh = mapMesh;
    }
}