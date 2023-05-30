using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flappyBird
{
    public static class GeneticLearning
    {
        public static void Mutate(Network net, Random random, double mutationRate)
        {
            //skip the input layer because it has no dendrites
            foreach (Layer layer in net.layers.Skip(1))
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    //Mutate the Weights
                    for (int i = 0; i < neuron.Dendrites.Length; i++)
                    {
                        if (random.NextDouble() < mutationRate)
                        {
                            if (random.Next(2) == 0)
                            {
                                neuron.Dendrites[i].Weight *= random.NextDouble(0.5, 1.5); //scale weight
                            }
                            else
                            {
                                neuron.Dendrites[i].Weight *= -1; //flip sign
                            }
                        }
                    }

                    //Mutate the Bias
                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.Bias *= random.NextDouble(0.5, 1.5); //scale weight
                        }
                        else
                        {
                            neuron.Bias *= -1; //flip sign
                        }
                    }
                }
            }
        }
        public static void Crossover(Network winner, Network loser, Random random)
        {
            for (int i = 1; i < winner.layers.Length; i++)
            {
                //References to the Layers
                Layer winLayer = winner.layers[i];
                Layer childLayer = loser.layers[i];

                int cutPoint = random.Next(winLayer.Neurons.Length); //calculate a cut point for the layer
                bool flip = random.Next(2) == 0; //randomly decide which side of the cut point will come from winner

                //Either copy from 0->cutPoint or cutPoint->Neurons.Length from the winner based on the flip variable
                for (int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winLayer.Neurons.Length); j++)
                {
                    //References to the Neurons
                    Neuron winNeuron = winLayer.Neurons[j];
                    Neuron childNeuron = childLayer.Neurons[j];

                    //Copy the winners Weights and Bias into the loser/child neuron
                    
                    childNeuron.Bias = winNeuron.Bias;
                    for(int z = 0; z < winNeuron.Dendrites.Length; z++)
                    {
                        childNeuron.Dendrites[z].Weight = winNeuron.Dendrites[z].Weight;

                    }
                }
            }
        }
        public static void Train(Bird[] population, Random random, double mutationRate)
        {
            Array.Sort(population, (a, b) => b.Fitness.CompareTo(a.Fitness));

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            //Notice that this process is only called on networks in the middle 80% of the array
            for (int i = start; i < end; i++)
            {
                Crossover(population[random.Next(start)].Brain, population[i].Brain, random);
                Mutate(population[i].Brain, random, mutationRate);
            }

            //Removes the worst performing networks
            for (int i = end; i < population.Length; i++)
            {
                population[i].Brain.Randomize(random, -6, 6);
            }
        }
    }
}
