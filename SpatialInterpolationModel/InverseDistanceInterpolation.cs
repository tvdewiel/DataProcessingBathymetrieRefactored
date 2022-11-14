﻿using DataSetManager;
using PredictionStats;

namespace SpatialInterpolationModel
{
    public class InverseDistanceInterpolation
    {
        public InverseDistanceInterpolation(int power, int numberOfPoints, DataSet dataSet)
        {
            Power = power;
            NumberOfPoints = numberOfPoints;
            this.dataSet = dataSet;
        }

        public InverseDistanceInterpolation(int power, int numberOfPoints, GridDataSet gridDataSet)
        {
            Power = power;
            NumberOfPoints = numberOfPoints;
            GridDataSet = gridDataSet;
        }

        public int Power { get; set; }
        public int NumberOfPoints { get; set; }
        public DataSet dataSet { get; set; }
        public GridDataSet GridDataSet { get; set; }
        public double ZGrid(double x, double y)
        {
            try
            {
                double sumW = 0.0;
                double z = 0;
                foreach (XYZ point in FindNearestNeighboursGrid(x, y, NumberOfPoints))
                {
                    double d = Math.Sqrt(Math.Pow(x - point.X, 2) + Math.Pow(y - point.Y, 2));
                    if (d == 0) return point.Z;
                    double w = 1.0 / Math.Pow(d, Power);
                    sumW += w;
                    z += w * point.Z;
                }
                return z / sumW;
            }
            catch (Exception ex)
            {
                SpatialInterpolationModelException simex = new("Z", ex);
                simex.Data.Add("Power", Power);
                simex.Data.Add("NumberOfPoints", NumberOfPoints);
                throw simex;
            }
        }
        public double ZBruteForce(double x, double y)
        {
            try
            {
                double sumW = 0.0;
                double z = 0;
                foreach (XYZ point in FindNearestNeighboursBruteForce(x, y, NumberOfPoints))
                {
                    double d = Math.Sqrt(Math.Pow(x - point.X, 2) + Math.Pow(y - point.Y, 2));
                    if (d == 0) return point.Z;
                    double w = 1.0 / Math.Pow(d, Power);
                    sumW += w;
                    z += w * point.Z;
                }
                return z / sumW;
            }
            catch (Exception ex)
            {
                SpatialInterpolationModelException simex = new("Z", ex);
                simex.Data.Add("Power", Power);
                simex.Data.Add("NumberOfPoints", NumberOfPoints);
                throw simex;
            }
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
        public List<XYZ> FindNearestNeighboursBruteForce(double x, double y, int n)
        {
            try
            {
                SortedList<double, List<XYZ>> nn = new SortedList<double, List<XYZ>>();
                double dsquare;
                double dmin;
                dsquare = Math.Pow(dataSet.data[0].X - x, 2) + Math.Pow(dataSet.data[0].Y - y, 2);
                dmin = dsquare;
                nn.Add(dsquare, new List<XYZ>() { dataSet.data[0] });
                for (int i = 1; i < n; i++)
                {
                    dsquare = Math.Pow(dataSet.data[i].X - x, 2) + Math.Pow(dataSet.data[i].Y - y, 2);
                    if (nn.ContainsKey(dsquare)) nn[dsquare].Add(dataSet.data[i]);
                    else nn.Add(dsquare, new List<XYZ>() { dataSet.data[i] });
                    if (dsquare > dmin) dmin = dsquare;
                }
                for (int i = n; i < dataSet.data.Count; i++)
                {
                    dsquare = Math.Pow(dataSet.data[i].X - x, 2) + Math.Pow(dataSet.data[i].Y - y, 2);
                    if (dsquare < dmin)
                    {
                        if (nn.ContainsKey(dsquare)) nn[dsquare].Add(dataSet.data[i]);
                        else nn.Add(dsquare, new List<XYZ>() { dataSet.data[i] });
                        if (nn.Count > n) nn.RemoveAt(n);
                        dmin = nn.Keys[n - 1];
                    }
                }
                return (List<XYZ>)ListFromSortedList(nn).Take(n).ToList();
            }
            catch (Exception ex)
            {
                throw new SpatialInterpolationModelException("FindNearestNeighbours", ex);
            }
        }
        public List<XYZoZp> PredictGrid(List<XYZ> toPredict)
        {
            List<XYZoZp> pred = new List<XYZoZp>();
            try
            {
                foreach (XYZ p in toPredict)
                {
                    pred.Add(new XYZoZp(p.X, p.Y, p.Z, ZGrid(p.X, p.Y)));
                }
                return pred;
            }
            catch (Exception ex) { throw new SpatialInterpolationModelException("Predict", ex); }
        }
        public List<XYZoZp> PredictBruteForce(List<XYZ> toPredict)
        {
            List<XYZoZp> pred = new List<XYZoZp>();
            try
            {
                foreach (XYZ p in toPredict)
                {
                    pred.Add(new XYZoZp(p.X, p.Y, p.Z, ZBruteForce(p.X, p.Y)));
                }
                return pred;
            }
            catch (Exception ex) { throw new SpatialInterpolationModelException("Predict", ex); }
        }
        private (int, int) FindCell(double x, double y)
        {
            if (!GridDataSet.XYBoundary.WithinBounds(x, y))
                throw new SpatialInterpolationModelException("FindCel - out of bounds");
            int i = (int)((x - GridDataSet.XYBoundary.MinX) / GridDataSet.Delta);
            int j = (int)((y - GridDataSet.XYBoundary.MinY) / GridDataSet.Delta);
            if (i == GridDataSet.NX) i--;
            if (j == GridDataSet.NY) j--;
            return (i, j);
        }
        private bool IsValidCell(int i, int j)
        {
            if ((i < 0) || (i >= GridDataSet.NX)) return false;
            if ((j < 0) || (j >= GridDataSet.NY)) return false;
            return true;
        }
        private void ProcessCell(SortedList<double, List<XYZ>> nn, int i, int j, double x, double y, int n)
        {
            foreach (XYZ p in GridDataSet.GridData[i][j])
            {
                double dsquare = Math.Pow(p.X - x, 2) + Math.Pow(p.Y - y, 2);
                if ((nn.Count < n) || (dsquare < nn.Keys[nn.Count - 1]))
                {
                    if (nn.ContainsKey(dsquare)) nn[dsquare].Add(p);
                    else nn.Add(dsquare, new List<XYZ>() { p });
                }
            }
        }
        private void ProcessRing(int i, int j, int ring, SortedList<double, List<XYZ>> nn, double x, double y, int n)
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
        public List<XYZ> FindNearestNeighboursGrid(double x, double y, int n)
        {
            try
            {
                SortedList<double, List<XYZ>> nn = new SortedList<double, List<XYZ>>();
                (int i, int j) = FindCell(x, y);
                ProcessCell(nn, i, j, x, y, n);
                int ring = 0;
                while (nn.Count < n)
                {
                    ring++;
                    ProcessRing(i, j, ring, nn, x, y, n);
                }
                ProcessRing(i, j, ring + 1, nn, x, y, n); //correcties
                return (List<XYZ>)ListFromSortedList(nn).Take(n).ToList();
            }
            catch (Exception ex)
            {
                throw new SpatialInterpolationModelException("FindNearestNeighbours", ex);
            }
        }
    }
}