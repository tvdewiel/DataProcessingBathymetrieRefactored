using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetManager
{
    public class GridDataSet
    {
        public GridDataSet(XYBoundary xyBoundary, double delta)
        {
            XYBoundary = xyBoundary;
            Delta = delta;
            NX = (int)(xyBoundary.DX / delta) + 1;//als niet juist deelbaar
            NY = (int)(xyBoundary.DY / delta) + 1;
            GridData = new List<XYZ>[NX][];
            for (int i = 0; i < NX; i++)
            {
                GridData[i] = new List<XYZ>[NY];
                for (int j = 0; j < NY; j++) GridData[i][j] = new List<XYZ>();
            }
        }
        public GridDataSet(XYBoundary xyBoundary, double delta, List<XYZ> data) : this(xyBoundary, delta)
        {
            foreach (XYZ point in data) AddXYZ(point);
        }
        public double Delta { get; private set; }
        public List<XYZ>[][] GridData { get; private set; }
        public void AddXYZ(XYZ point)
        {
            if ((point.X < XYBoundary.MinX) || (point.X > XYBoundary.MaxX) || (point.Y < XYBoundary.MinY) || (point.Y > XYBoundary.MaxY))
                throw new DataSetManagerException("GridDataSet - AddXYZ - out of bounds");
            int i = (int)((point.X - XYBoundary.MinX) / Delta);
            int j = (int)((point.Y - XYBoundary.MinY) / Delta);
            if (i == NX) i--;
            if (j == NY) j--;
            GridData[i][j].Add(point);
        }
        public XYBoundary XYBoundary { get; private set; }
        public int NX { get; private set; }
        public int NY { get; private set; }
    }
}
