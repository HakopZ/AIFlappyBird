using System;
using System.Collections.Generic;
using System.Text;

namespace flappyBird
{
    public class Neuron
    {
        public double Bias { get; set; }
        public Dendrite[] Dendrites { get; set; }
        public double Output { get; set; }
        public double Input { get; private set; }
        public double Delta { get; set; }
        public ActivationFunction Activation { get; set; }

        public Neuron(ActivationFunction activation, Neuron[] previousNerons)
        {
            Activation = activation;
            if (previousNerons != null)
            {
                Dendrites = new Dendrite[previousNerons.Length];
                for (int i = 0; i < Dendrites.Length; i++)
                {
                    Dendrites[i] = new Dendrite(previousNerons[i], this, 0);
                }
            }
            else
            {
                Dendrites = null;
            }
        }

        public void Randomize(Random random, double min, double max)
        {
            Bias = random.NextDouble(min, max);
            foreach (var dendrite in Dendrites)
            {
                dendrite.Weight = random.NextDouble(min, max);
            }
        }

        public double Compute()
        {
            Input = Bias;
            for (int i = 0; i < Dendrites.Length; i++)
            {
                Input += Dendrites[i].Compute();
            }
            Output = Activation.Function(Input);
            return Output;
        }

    }
}