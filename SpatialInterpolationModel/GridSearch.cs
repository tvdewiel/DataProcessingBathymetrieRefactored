using DataSetManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialInterpolationModel
{
    public class GridSearch
    {
        private GridDataSet dataSet;

        public GridSearch(GridDataSet dataSet)
        {
            this.dataSet = dataSet;
        }
        private List<XYZ> ListFromSortedList(SortedList<double, List<XYZ>> nn)
        {
            List<XYZ> list = new List<XYZ>();
            foreach (List<XYZ> l in nn.Values)
            {
                foreach (XYZ v in l) list.Add(v);
            }
            return list;
        }
        private (int, int) FindCel(double x, double y)
        {
            if (!dataSet.XYBoundary.WithinBounds(x, y))
                throw new SpatialInterpolationModelException("FindCel - out of bounds");
            int i = (int)((x - dataSet.XYBoundary.MinX) / dataSet.dx);
            int j = (int)((y - dataSet.XYBoundary.MinY) / dataSet.dy);
            if (i == dataSet.NX) i--;
            if (j == dataSet.NY) j--;
            return (i, j);
        }
        private bool IsValidCell(int i, int j)
        {
            if ((i < 0) || (i >= dataSet.NX)) return false;
            if ((j < 0) || (j >= dataSet.NY)) return false;
            return true;
        }
        private void ProcessCell(SortedList<double, List<XYZ>> nn, int i, int j, double x, double y, int n)
        {
            foreach (XYZ p in dataSet.GridData[i][j])
            {
                double dsquare = Math.Pow(p.X - x, 2) + Math.Pow(p.Y - y, 2);
                if ((nn.Count < n) || (dsquare < nn.Keys[nn.Count - 1]))
                {
                    if (nn.ContainsKey(dsquare)) nn[dsquare].Add(p);
                    else nn.Add(dsquare, new List<XYZ>() { p });
                }
            }
        }
        private void ProcessRing(int i,int j, int ring,SortedList<double,List<XYZ>> nn,double x, double y,int n)
        {
            for (int gx = i - ring; gx <= i + ring; gx++)
            {
                //onderste rij
                int gy = j - ring;
                if (IsValidCell(gx, gy)) ProcessCell(nn, gx, gy, x, y, n);
                //bovenste rij
                gy = j + ring;
                if (IsValidCell(gx, gy)) ProcessCell(nn, gx, gy, x, y, n);
            }
            for (int gy = j - ring + 1; gy <= j + ring - 1; gy++)
            {
                //linker kolom
                int gx = i - ring;
                if (IsValidCell(gx, gy)) ProcessCell(nn, gx, gy, x, y, n);
                //rechter kolom
                gx = i + ring;
                if (IsValidCell(gx, gy)) ProcessCell(nn, gx, gy, x, y, n);
            }
        }
        public List<XYZ> FindNearestNeighbours(double x, double y, int n)
        {
            try
            {
                SortedList<double, List<XYZ>> nn = new SortedList<double, List<XYZ>>();
                (int i, int j) = FindCel(x, y);
                ProcessCell(nn, i, j, x, y, n);
                int ring = 0;
                while (nn.Count < n)
                {
                    //ring
                    ring++;
                    ProcessRing(i,j,ring,nn,x,y,n);
                }
                ProcessRing(i,j,ring+1,nn, x,y,n); //correcties
                return (List<XYZ>)ListFromSortedList(nn).Take(n).ToList();
            }
            catch (Exception ex)
            {
                throw new SpatialInterpolationModelException("FindNearestNeighbours", ex);
            }
        }
    }
}
