using System;
using KMeansClustering.Graph;

namespace KMeansClustering
{

    /**
     * Epsilon & max iterations values are provided as static variables.
     */
    public static class Globals
    {
        public static double EPSILON = 0.1;
        public static double MAX_ITER = 1000;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments.");
                System.Environment.Exit(-1);
            }

            InputReader.InputReader inputReader = new InputReader.InputReader(args);

            KMeansAlgorithm algorithm = new KMeansAlgorithm(inputReader.GetDataNodes(), inputReader.GetKValue());
            
            algorithm.PerformClustering();
            
            algorithm.PrettyPrintClusters();

        }
    }
}
