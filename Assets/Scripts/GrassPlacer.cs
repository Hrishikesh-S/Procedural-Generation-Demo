using UnityEngine;

public class GrassPlacer : MonoBehaviour
{
    [SerializeField] private int grassSpacing;
    [SerializeField] private int grassRowCount;
    [SerializeField] private Mesh grass;
    [SerializeField] private Material grassMaterial;

    private Matrix4x4[] matrices;
    private Transform player;

    private Vector3 grassOrigin;
    private Vector3 chunkPos;
    private readonly int chunkSize = WorldGenData.chunkSize;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        grassOrigin = player.position;

        chunkPos.x = Mathf.RoundToInt(transform.position.x / chunkSize);
        chunkPos.z = Mathf.RoundToInt(transform.position.z / chunkSize);
    }

    void Update()
    {
        matrices = new Matrix4x4[grassRowCount * grassRowCount];

        int i = 0;
        for (int x = 0; x < grassRowCount; x++)
        {
            for (int z = 0; z < grassRowCount; z++)
            {
                int grassX = (int)grassOrigin.x + x * grassSpacing;
                int grassZ = (int)grassOrigin.z + z * grassSpacing;
                float grassY = WorldGenData.HeightMap(grassX, grassZ);
                if (grassY > -10 && grassY < 10 && Mathf.PerlinNoise(grassX * 1.3f, grassZ * 1.3f) > 0.55f)
                {
                    Vector3 grassPositionY = new Vector3(grassX - (chunkSize / 2), grassY, grassZ - (chunkSize / 2));
                    matrices[i] = Matrix4x4.TRS(grassPositionY, Quaternion.Euler(0,0,0), new Vector3(10,10,10));
                }
                i++;
            }
        }

        Graphics.DrawMeshInstanced(grass, 0, grassMaterial, matrices);

        grassOrigin.x = player.position.x - player.position.x % grassSpacing - (chunkSize / 2);
        grassOrigin.z = player.position.z - player.position.z % grassSpacing - (chunkSize / 2);
    }
}
