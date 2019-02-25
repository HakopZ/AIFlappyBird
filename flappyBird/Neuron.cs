using System;
using System.Collections.Generic;
using System.Text;

namespace flappyBird
{
    class Neuron
    {
        public float[] Weights;
        public float Bias;
        public float Output;
        public Func<float, float> Activation;
        public Neuron(Func<float, float> activation, int inputCount)
        {
            Activation = activation;
            Weights = new float[inputCount];
        }
        public void Randomize(Random rand)
        {
            Bias = (float)(rand.NextDouble() * (0.5 + 0.5) - 0.5);
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = (float)(rand.NextDouble() * (0.5 + 0.5) - 0.5);
            }
        }
        public float Compute(float[] inputs)
        {
            float sum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            Output = Activation(sum + Bias);
            return Output;
        }
    }
}