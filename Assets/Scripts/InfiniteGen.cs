using System.Collections.Generic;
using UnityEngine;

public class InfiniteGen : MonoBehaviour
{
    [SerializeField] private int chunkRange;
    [HideInInspector] public Vector3 chunkPos;
    [SerializeField] private GameObject chunkPrefab;

    private readonly int chunkSize = WorldGenData.chunkSize;
    private List<Vector3> activeChunkPositions = new List<Vector3>();
    private List<GameObject> activeChunks = new List<GameObject>();
    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        for (int z = -chunkRange; z <= chunkRange; z++)
        {
            for (int x = -chunkRange; x <= chunkRange; x++)
            {
                GameObject newTile = Instantiate(chunkPrefab, new Vector3(x, 0, z) * chunkSize, Quaternion.identity);
                activeChunks.Add(newTile);
                activeChunkPositions.Add(newTile.transform.position);
            }
        }
    }

    void Update()
    {
        chunkPos = Vector3Int.RoundToInt((player.position - new Vector3(chunkSize, 0, chunkSize) / 2) / chunkSize);
        chunkPos.y = 0;

        for (int i = 0; i < activeChunks.Count; i++)
        {
            if (Vector3.SqrMagnitude(chunkPos - activeChunkPositions[i]) > 2 * chunkRange * chunkRange)
            {
                Destroy(activeChunks[i]);
                activeChunks.RemoveAt(i);
                activeChunkPositions.RemoveAt(i);
            }
        }
        for (int z = -chunkRange; z <= chunkRange; z++)
        {
            for (int x = -chunkRange; x <= chunkRange; x++)
            {
                if (!activeChunkPositions.Contains(chunkPos + new Vector3(x, 0, z)))
                {
                    activeChunks.Add(Instantiate(chunkPrefab, (chunkPos + new Vector3(x, 0, z)) * chunkSize, Quaternion.identity));
                    activeChunkPositions.Add((chunkPos + new Vector3(x, 0, z)));
                }
            }
        }
    }
}
