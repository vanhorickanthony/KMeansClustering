using System.Collections.Generic;

namespace KMeansClustering.InputReader
{
    public class JsonFormat
    {
        public IList<double> x;
        public IList<double> y;

        public JsonFormat()
        {
            this.x = new List<double>();
            this.y = new List<double>();
        }
    }
}