using System;
using System.Collections.Generic;
using System.Text;

namespace flappyBird
{
    class Layer
    {
        public Neuron[] Neurons;
        //public double[] Output { get; set; }
        public Layer(Func<float, float> activation, int inputCount, int neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(activation, inputCount);
            }
        }
        public void Randomize(Random rand)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Randomize(rand);
            }
        }
        public float[] Compute(float[] input)
        {
            float[] output = new float[Neurons.Length];

            for (int i = 0; i < Neurons.Length; i++)
            {
                output[i] = Neurons[i].Compute(input);
            }

            return output;
        }
    }
}