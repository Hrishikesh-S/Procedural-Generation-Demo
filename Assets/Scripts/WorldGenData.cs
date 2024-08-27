using UnityEngine;

public class WorldGenData : MonoBehaviour
{
    public static int chunkSize = 384;
    public static int simplificationFactor = 8;
    public static float perlinScale = .0015f;
    public static float heightMultiplier = 300;

    public AnimationCurve heightCurve;
    private static AnimationCurve _heightCurve;

    private static readonly int octaves = 6;
    private static readonly float persistance = 0.5f;
    private static readonly float lacunarity = 2;

    private void Awake()
    {
        _heightCurve = heightCurve;
    }

    public static float HeightMap(float x, float z)
    {
        float y = 0;

        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            y += amplitude * (Mathf.PerlinNoise(x * perlinScale * frequency, z * perlinScale * frequency) * 2 - 1);
            amplitude *= persistance;
            frequency *= lacunarity;
        }

        y *= _heightCurve.Evaluate(y) * heightMultiplier;
        return y;
    }
}
