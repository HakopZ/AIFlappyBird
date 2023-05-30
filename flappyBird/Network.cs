using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace flappyBird
{
    public class Network
    {
        public Layer[] layers;
        public double[] Output { get; set; }
        public int InputCount => layers[0].Neurons.Length;
        public int OutputCount => layers[layers.Length - 1].Neurons.Length;
        
        public Network(ActivationFunction activation, params int[] neuronsPerLayer)
        {
            
            layers = new Layer[neuronsPerLayer.Length];
            Layer previousLayer = null;
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(activation, neuronsPerLayer[i], previousLayer);
                previousLayer = layers[i];
            }
        }
        public void Randomize(Random rand, double min, double max)
        {
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].Randomize(rand, min, max);
            }
        }
        public double[] Compute(double[] inputs)
        {
            for (int i = 0; i < InputCount; i++)
            {
                layers[0].Neurons[i].Output = inputs[i];
            }
            Output = inputs;
            for (int i = 1; i < layers.Length; i++)
            {
                Output = layers[i].Compute();
            }
            return Output;
        }
        //public void Mutate(Random rand, double rate)
        //{
        //    foreach (Layer layer in layers)
        //    {
        //        foreach (Neuron neuron in layer.Neurons)
        //        {
        //            if (rand.NextDouble() < rate)
        //            {
        //                neuron.Bias *= (double)rand.NextDouble(0.5, 1.5) * rand.RandomSign();
        //            }
        //            for (int i = 0; i < neuron.Weights.Length; i++)
        //            {
        //                if (rand.NextDouble() < rate)
        //                {
        //                    neuron.Weights[i] *= (double)rand.NextDouble(0.5, 1.5) * rand.RandomSign();
        //                }
        //            }
        //        }
        //    }
        //}

    }
}