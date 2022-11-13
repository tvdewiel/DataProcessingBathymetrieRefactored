using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetManager
{
    public class GridDataSet
    {
        public GridDataSet(XYBoundary xyBoundary, int nX, int nY)
        {
            XYBoundary = xyBoundary;
            NX = nX;
            NY = nY;
            GridData = new List<XYZ>[NX][];
            for (int i = 0; i < NX; i++)
            {
                GridData[i] = new List<XYZ>[NY];
                for (int j = 0; j < NY; j++) GridData[i][j] = new List<XYZ>();
            }
            dx = xyBoundary.DX / NX;
            dy = xyBoundary.DY / NY;
        }
        public GridDataSet(XYBoundary xyBoundary, int nX, int nY, List<XYZ>[][] gridData)
            : this(xyBoundary, nX, nY)
        {
            GridData = gridData;
        }
        public GridDataSet(XYBoundary xyBoundary, int nX, int nY, DataSet dataSet) : this(xyBoundary, nX, nY)
        {
            foreach (XYZ point in dataSet.data) AddXYZ(point);
        }
        public double dx { get; private set; }
        public double dy { get; private set; }
        public List<XYZ>[][] GridData { get; private set; }
        public void AddXYZ(XYZ point)
        {
            if ((point.X < XYBoundary.MinX) || (point.X > XYBoundary.MaxX) || (point.Y < XYBoundary.MinY) || (point.Y > XYBoundary.MaxY))
                throw new DataSetManagerException("GridDataSet - AddXYZ - out of bounds");
            int i = (int)((point.X - XYBoundary.MinX) / dx);
            int j = (int)((point.Y - XYBoundary.MinY) / dy);
            if (i == NX) i--;
            if (j == NY) j--;
            GridData[i][j].Add(point);
        }
        public XYBoundary XYBoundary { get; private set; }
        public int NX { get; private set; }
        public int NY { get; private set; }
    }
}
