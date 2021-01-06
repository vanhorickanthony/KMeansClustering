namespace KMeansClustering.Graph
{
    public class CentroidNode: BaseNode
    {

        public CentroidNode(double x, double y): base(x, y)
        {
            
        }

        public CentroidNode(DataNode dataNode) : base(dataNode.GetX(), dataNode.GetY())
        {
            
        }

        public void Move(double x, double y)
        {
            this.SetX(x);
            this.SetY(y);
        }
    }
}