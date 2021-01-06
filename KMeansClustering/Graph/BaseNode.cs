namespace KMeansClustering.Graph
{
    public abstract class BaseNode
    {
        private double _x;
        private double _y;

        protected BaseNode(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public double GetX()
        {
            return this._x;
        }

        public double GetY()
        {
            return this._y;
        }

        protected void SetX(double x)
        {
            this._x = x;
        }
        
        protected void SetY(double y)
        {
            this._y = y;
        }

        public override string ToString()
        {
            return "{ " + _x + ", " + _y + " }";
        }
    }
}