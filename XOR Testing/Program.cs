using flappyBird;

public class Program
{
    static void Main(string[] args)
    {
        double[][] inputs = new double[][]
              {
                new double[]{0,0},
                new double[]{1,0},
                new double[]{0,1},
                new double[]{1,1}
              };
        double[][] outputs = new double[][]
        {
                new double[]{0},
                new double[]{1},
                new double[]{1},
                new double[]{0}
        };
        Random random = new Random();

        
        int[] neuronsPerLayer = new int[] { 2, 2, 1 };

        (Network net, double fitness)[] population = new (Network net, double fitness)[1000];
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = (new Network(ActivationAndErrorFunction.TanHActivationFunction, neuronsPerLayer), 0);
            population[i].net.Randomize(random, -2, 2);
        }
        while (true)
        {
            Console.SetCursorPosition(0, 0);
            for (int x = 0; x < population.Length; x++)
            {
                double totalError = 0;
                for (int i = 0; i < inputs.Length; i++)
                {
                    var output = population[x].net.Compute(inputs[i]);
                    totalError += ActivationAndErrorFunction.MeanSquaredError(output[0], outputs[i][0]);
                }
                population[x].fitness = totalError;
            }
            GeneticLearning.Train(population, random, 0.01, true);
            double error = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                Console.Write("Inputs: ");
                for (int j = 0; j < inputs[i].Length; j++)
                {
                    if (j != 0)
                    {
                        Console.Write(", ");
                    }
                    Console.Write(inputs[i][j]);
                }
                var output = population[0].net.Compute(inputs[i]);
                error += ActivationAndErrorFunction.MeanSquaredError(output[0], outputs[i][0]);
                Console.Write(" Output: " + Math.Round(output[0], 3));
                Console.WriteLine();
            }
            
            Console.WriteLine("Error: " + Math.Round(error/4, 3));
        }


    }

}