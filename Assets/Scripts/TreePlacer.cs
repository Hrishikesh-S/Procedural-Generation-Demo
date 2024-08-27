using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TreePlacer : MonoBehaviour
{
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private int treesInChunk;
    private int chunkSize;
    private List<Transform> treeTransforms = new List<Transform>();

    private void Start()
    {
        chunkSize = WorldGenData.chunkSize;
        Random.InitState(100);
    }

    private void Update()
    {
        for (int i = 0; i < treeTransforms.Count; i++)
        {
            if (treeTransforms[i].GetComponent<Rigidbody>())
                treeTransforms.RemoveAt(i);
        }
    }

    public void AddTrees()
    {
        for (int i = 0; i < treesInChunk; i++)
        {
            int x = Random.Range(0, chunkSize);
            int z = Random.Range(0, chunkSize); 

            float y = WorldGenData.HeightMap((x + transform.position.x), (z + transform.position.z));
            if (y < -10 || y > 10)
            {
                i--;
                continue;
            }
            float treeX = (x + transform.position.x - chunkSize / 2);
            float treeZ = (z + transform.position.z - chunkSize / 2);
            GameObject tree = Instantiate(treePrefab, new Vector3(treeX, y, treeZ), Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
            treeTransforms.Add(tree.transform);
            tree.transform.localScale = Vector3.one * Random.Range(7f, 10f);
            tree.transform.parent = transform;
        }
    }

    public void SaveTrees(string chunkPath)
    {
        File.WriteAllText(chunkPath, "");
        using (StreamWriter writer = new StreamWriter(chunkPath))
        {
            foreach (Transform tree in treeTransforms)
                writer.WriteLine(tree.position.x + "\n" + tree.position.y + "\n" + tree.position.z);
            writer.Close();
            writer.Dispose();
        }
    }

    public void LoadTrees(string chunkPath)
    {
        using (StreamReader reader = new StreamReader(chunkPath))
        {
            while (!reader.EndOfStream)
            {
                int x = int.Parse(reader.ReadLine());
                float y = float.Parse(reader.ReadLine());
                int z = int.Parse(reader.ReadLine());
                GameObject tree = Instantiate(treePrefab, new Vector3(x, y, z), Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
                tree.transform.parent = transform;
                tree.transform.localScale = Vector3.one * Random.Range(7f, 10f);
                treeTransforms.Add(tree.transform);
            }
            reader.Close();
            reader.Dispose();
        }
    }
}
