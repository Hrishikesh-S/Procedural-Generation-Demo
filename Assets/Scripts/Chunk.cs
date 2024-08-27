using System.IO;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private readonly int chunkSize = WorldGenData.chunkSize;
    private readonly int simplificationFactor = WorldGenData.simplificationFactor;
    private readonly int verticesPerLine = WorldGenData.chunkSize / WorldGenData.simplificationFactor + 1;
    public Vector3 chunkPos;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        chunkPos = new Vector3(transform.position.x, 0, transform.position.z) / chunkSize;

        CreateShape();
        gameObject.AddComponent<MeshCollider>();
    }

    private void CreateShape()
    {
        vertices = new Vector3[verticesPerLine * verticesPerLine];

        int i = 0;
        for (int x = 0; x <= verticesPerLine - 1; x++)
        {
            for (int z = 0; z <= verticesPerLine - 1; z++)
            {
                float y = WorldGenData.HeightMap(x * simplificationFactor + transform.position.x, z * simplificationFactor + transform.position.z);
                vertices[i++] = new Vector3((x * simplificationFactor), y, (z * simplificationFactor));
            }
        }

        triangles = new int[6 * (verticesPerLine - 1) * (verticesPerLine - 1)];

        int vert = 0;
        int tris = 0;

        for (int x = 0; x < verticesPerLine - 1; x++)
        {
            for (int z = 0; z < verticesPerLine - 1; z++)
            {
                triangles[tris + 0] = vert + 1;
                triangles[tris + 1] = vert + verticesPerLine;
                triangles[tris + 2] = vert;
                triangles[tris + 3] = vert + verticesPerLine + 1;
                triangles[tris + 4] = vert + verticesPerLine;
                triangles[tris + 5] = vert + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}