using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen
{
    public static float Noise3D(int[] position)
    {
        float multiplier = 0.1f;
        float xy = Mathf.PerlinNoise(position[0]* multiplier, position[1]* multiplier);
        float yz = Mathf.PerlinNoise(position[1]* multiplier, position[2]* multiplier);
        float xz = Mathf.PerlinNoise(position[0]* multiplier, position[2]* multiplier);

        float yx = Mathf.PerlinNoise(position[1]* multiplier, position[0]* multiplier);
        float zy = Mathf.PerlinNoise(position[2], position[1]* multiplier);
        float zx = Mathf.PerlinNoise(position[2]* multiplier, position[0]* multiplier);

        float xyz = xy + yz + xz + yx + zy + zx;
        return xyz / 6f;
    }
}
