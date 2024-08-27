using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePlacer : MonoBehaviour
{
    [SerializeField] private GameObject[] resourcePrefabs;
    [SerializeField] private float[] minResourceHeights;
    [SerializeField] private float[] maxResourceHeights;
    [SerializeField] private float density;
    private readonly int chunkSize = WorldGenData.chunkSize;

    private void Start()
    {
        Random.InitState(100);
        AddResources();
    }
    private void AddResources()
    {
        for (float z = 0; z < chunkSize; z += 1f / density)
        {
            for (float x = 0; x < chunkSize; x += 1f / density)
            {
                Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-1f, 1f));
                if (!Physics.Raycast(new Vector3(x, 40, z) + offset + transform.position, Vector3.down, out RaycastHit hit, 400, 8))
                    continue;
                Vector3 placePos = hit.point;
                int resourceIndex = Random.Range(0, resourcePrefabs.Length);
                if (Random.Range(0f, 1f) > 0.95f && placePos.y < maxResourceHeights[resourceIndex] && placePos.y > minResourceHeights[resourceIndex])
                {
                    GameObject newResource = Instantiate(resourcePrefabs[resourceIndex], placePos, Quaternion.identity);
                    newResource.transform.parent = transform;
                }
            }
        }
    }
}
