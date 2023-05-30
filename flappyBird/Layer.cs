using System;
using System.Collections.Generic;
using System.Text;

namespace flappyBird
{
    public class Layer
    {
        public Neuron[] Neurons { get; }
        public double[] Outputs { get; }
        //public double[] Output { get; set; }
        public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer)
        {
            Neurons = new Neuron[neuronCount];
            Neuron[] previousNeurons = null;
            if (previousLayer != null)
            {
                previousNeurons = previousLayer.Neurons;
            }
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(activation, previousNeurons);
            }
            Outputs = new double[Neurons.Length];
        }
        public void Randomize(Random rand, double min, double max)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Randomize(rand, min, max);
            }
        }
        public double[] Compute()
        {
           

            for (int i = 0; i < Neurons.Length; i++)
            {
                Outputs[i] = Neurons[i].Compute();
            }

            return Outputs;
        }
    }
}