using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;

namespace Bx
{
    public class RandomColorUtils
    {
        static Perlin noiseGenerator = null;

        public static Color GetRandomColor()
        {
            return new Color(Random.Range(0F, 1F), Random.Range(0F, 1F), Random.Range(0F, 1F));
        }

        public static Color GetRandomColorInRange(Color a, Color b)
        {
            return Color.Lerp(a, b, Random.Range(0F, 1F));
        }

        public static float Noise(Vector3 p)
        {
            if (noiseGenerator == null)
            {
                noiseGenerator = new Perlin();
            }
            return (float)noiseGenerator.GetValue(p.x, p.y, p.z);
        }

        public static Color GetRandomColorInRange(Color a, Color b, Vector3 pos)
        {
            return Color.Lerp(a, b, Noise(pos));
        }
    }
}