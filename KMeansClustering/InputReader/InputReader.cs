using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using KMeansClustering.Graph;
using Newtonsoft.Json;

namespace KMeansClustering.InputReader
{
    public class InputReader
    {
        private List<DataNode> _dataNodes = new List<DataNode>();

        private int _k = 0;
        
        public InputReader(string[] args)
        {
            ImportJson(args[0]);

            _k = Int32.Parse(args[1]);

            Console.WriteLine(this._dataNodes.Count);
        }

        private void ImportJson(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string content = reader.ReadToEnd();

                JsonFormat jsonInput = JsonConvert.DeserializeObject<JsonFormat>(content);

                Console.WriteLine(content);

                Console.WriteLine(jsonInput.x.Count);

                if (jsonInput.x.Count == jsonInput.y.Count && jsonInput.x.Count > 0)
                {
                    _dataNodes.Clear();
                    
                    for (int idx = 0; idx < jsonInput.x.Count; idx++)
                    {
                        _dataNodes.Add(new DataNode(jsonInput.x[idx], jsonInput.y[idx]));
                    }
                }
                else
                {
                    Console.Error.WriteLine("Invalid JSON input");
                }
            }

        }

        public List<DataNode> GetDataNodes()
        {
            return this._dataNodes;
        }

        public int GetKValue()
        {
            return this._k;
        }
    }
}