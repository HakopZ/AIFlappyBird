using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace flappyBird
{
    class Network
    {
        public Layer[] layers;
        public float[] Output { get; set; }
        public Network(Func<float, float> activation, int inputCount, params int[] neuronsPerLayer)
        {
            layers = new Layer[neuronsPerLayer.Length];
            layers[0] = new Layer(activation, inputCount, neuronsPerLayer[0]);
            for (int i = 1; i < neuronsPerLayer.Length; i++)
            {
                layers[i] = new Layer(activation, layers[i - 1].Neurons.Length, neuronsPerLayer[i]);
            }
        }
        public void Randomize(Random rand)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].Randomize(rand);
            }
        }
        public float[] Compute(float[] input)
        {
            float[] output = input;
            for (int i = 0; i < layers.Length; i++)
            {
                output = layers[i].Compute(output);
            }
            return output;
        }
        public void Mutate(Random rand, double rate)
        {
            foreach (Layer layer in layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    if (rand.NextDouble() < rate)
                    {
                        neuron.Bias *= (float)rand.NextDouble(0.5, 1.5) * rand.RandomSign();
                    }
                    for (int i = 0; i < neuron.Weights.Length; i++)
                    {
                        if (rand.NextDouble() < rate)
                        {
                            neuron.Weights[i] *= (float)rand.NextDouble(0.5, 1.5) * rand.RandomSign();
                        }
                    }
                }
            }
        }

    }
}