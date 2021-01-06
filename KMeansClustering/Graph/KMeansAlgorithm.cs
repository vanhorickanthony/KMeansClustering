using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using KMeansClustering.InputReader;

namespace KMeansClustering.Graph
{
    public class KMeansAlgorithm
    {
        private static Random _random = new Random();

        private List<DataNode> _dataNodes = new List<DataNode>();
        private List<CentroidNode> _centroidNodes = new List<CentroidNode>();
        
        private Dictionary<CentroidNode, List<DataNode>> _clusterMap = new Dictionary<CentroidNode, List<DataNode>>();

        private int _kValue = 0;

        public KMeansAlgorithm(List<DataNode> dataNodes, int kValue)
        {
            this._dataNodes = dataNodes;

            this._kValue = kValue;
            
            this.SelectRandomCentroids();
        }

        public void SetDataNodes(List<DataNode> dataNodes)
        {
            this._dataNodes = dataNodes;
        }

        public void SetKValue(int kValue)
        {
            this._kValue = kValue;
        }
        
        private void SelectRandomCentroids()
        {
            double xMin = double.PositiveInfinity;
            double yMin = double.PositiveInfinity;
            
            double xMax = double.NegativeInfinity;
            double yMax = double.NegativeInfinity;
            
            foreach (DataNode node in _dataNodes)
            {
                xMin = Math.Min(xMin, node.GetX());
                yMin = Math.Min(yMin, node.GetY());

                xMax = Math.Max(xMax, node.GetX());
                yMax = Math.Max(yMax, node.GetY());
            }

            for (int idx = 0; idx < _kValue; idx++)
            {
                _centroidNodes.Add(
                        new CentroidNode(RandomDouble(xMin, xMax), RandomDouble(yMin, yMax))
                        );
            }
        }

        private double RandomDouble(double lower, double upper)
        {
            return lower + (upper - lower) * _random.NextDouble();
        }

        public void PerformClustering()
        {
            this._clusterMap.Clear();
            
            double largestChange = Double.NegativeInfinity;

            for (int iter = 0; iter < Globals.MAX_ITER; iter++)
            {
                this._clusterMap.Clear();
                
                largestChange = Double.NegativeInfinity;

                /*
                 * There's probably a more elegant way of populating the clustermap...
                 */
                foreach (DataNode dataNode in _dataNodes)
                {
                    CentroidNode nearestCentroid = FindNearestCentroid(dataNode, _centroidNodes);

                    if (this._clusterMap.ContainsKey(nearestCentroid))
                    {
                        this._clusterMap[nearestCentroid].Add(dataNode);
                    }
                    else
                    {
                        this._clusterMap.Add(nearestCentroid, new List<DataNode>());
                        this._clusterMap[nearestCentroid].Add(dataNode);
                    }
                }
                
                foreach (var (centroidNode, dataNodes) in _clusterMap)
                {
                    double xTotal = 0;
                    double yTotal = 0;
                    
                    foreach (DataNode dataNode in dataNodes)
                    {
                        xTotal += dataNode.GetX();
                        yTotal += dataNode.GetY();
                    }
                    
                    double xMean = xTotal / dataNodes.Count;
                    double yMean = yTotal / dataNodes.Count;
                    
                    /*
                     * Use a helper-node to assist in calculating the largest change in this current iteration.
                     */

                    CentroidNode helperNode = new CentroidNode(xMean, yMean);

                    double currentDistance = GetDistance(centroidNode, helperNode);

                    if (currentDistance > largestChange)
                    {
                        largestChange = currentDistance;
                    }

                    centroidNode.Move(xMean, yMean);
                }

                if (largestChange < Globals.EPSILON)
                {
                    break;
                }
            }
        }

        public void PrettyPrintClusters()
        {
            int i = 1;
            
            Console.WriteLine("Clustering results: ");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~");
            foreach(var (centroidNode, dataNodes) in _clusterMap)
            {
                Console.WriteLine("Cluster " + i  + " of " + _kValue);
                
                Console.WriteLine("\tCentroid: ");
                Console.WriteLine("\t" + centroidNode.ToString());

                JsonFormat json = NodesToJson(dataNodes);

                string jsonString = JsonConvert.SerializeObject(json);

                Console.WriteLine("\t" + jsonString);
                
                Console.WriteLine("--------------------------");

                i++;
            }

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~");

        }

        public JsonFormat NodesToJson(List<DataNode> dataNodes)
        {
            JsonFormat json = new JsonFormat();

            foreach (DataNode dataNode in dataNodes)
            {
                json.x.Add(dataNode.GetX());
                json.y.Add(dataNode.GetY());
            }

            return json;
        }

        private CentroidNode FindNearestCentroid(DataNode dataNode, IEnumerable<CentroidNode> centroidNodes)
        {
            CentroidNode nearestCentroidNode = null;

            double minDistance = double.PositiveInfinity;
            
            foreach (CentroidNode centroidNode in centroidNodes)
            {
                double currentDistance = GetDistance(dataNode, centroidNode);
                
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    nearestCentroidNode = centroidNode;
                }
            }

            return nearestCentroidNode;
        }

        private double GetDistance(BaseNode firstNode, BaseNode secondNode)
        {
            return Math.Sqrt(Math.Pow(firstNode.GetX() - secondNode.GetX(), 2) + Math.Pow(firstNode.GetY() - secondNode.GetY(), 2));
        }

        public Dictionary<CentroidNode, List<DataNode>> GetClusters()
        {
            return this._clusterMap;
        }
    }
}