using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen
{
    public static float Noise3D(float x, float y, float z, float scale, float offset)
    {
        float xy = Noise(x, y, scale, offset);
        float yz = Noise(y, z, scale, offset);
        float xz = Noise(x, z, scale, offset);

        float yx = Noise(y, x, scale, offset);
        float zy = Noise(z, y, scale, offset);
        float zx = Noise(z, x, scale, offset);

        float xyz = xy + yz + xz + yx + zy + zx;
        return xyz / 6f;
    }

    public static float Noise(float x, float y, float scale, float offset)
    {
        float staticOffset = 9999.9f;
        return Mathf.PerlinNoise(x / scale + staticOffset + offset, y / scale + staticOffset + offset);
    }
}
