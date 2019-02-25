using System;
using System.Collections.Generic;
using System.Text;

namespace flappyBird
{
    public static class Activations
    {
        public static float BinaryStep(float x)
        {
            return x < 0 ? 0 : 1;
        }
        public static float Segma(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }
    }
}