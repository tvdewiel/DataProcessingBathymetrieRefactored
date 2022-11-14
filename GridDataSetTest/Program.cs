// See https://aka.ms/new-console-template for more information
using DataSetManager;
using PredictionStats;
using SpatialInterpolationModel;
using System.Diagnostics;

Console.WriteLine("Hello, World! grid");
var timer = new Stopwatch();
timer.Start();
var data = FileDataSetManager.ReadData(@"C:\data\belgica\belgica.txt");
timer.Stop();
TimeSpan timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
timer.Reset();
timer.Start();
var setsGrid = FileDataSetManager.MakeGridDataSetsWithTestSet(data,10000, new List<(int, double)> { (10000, 20), (20000, 20), (20000, 100), (100000, 10) });
timer.Stop();
timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
timer.Reset();
timer.Start();
var setsSeq = FileDataSetManager.MakeDataSetsWithTestSet(data.data, new List<int> { 10000,10000, 20000,20000,100000 });
timer.Stop();
timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));

timer.Reset();
timer.Start();
InverseDistanceInterpolation idg = new InverseDistanceInterpolation(2, 4, setsGrid.Item2[3]);
var predGS=idg.PredictGrid(setsGrid.Item1.data);
var resGS=CalculateStats.CalculateParameters(predGS);
Console.WriteLine(resGS);
timer.Stop();
timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));

timer.Reset();
timer.Start();
InverseDistanceInterpolation idb = new InverseDistanceInterpolation(2, 4, setsSeq[4]);
var predBF = idb.PredictBruteForce(setsGrid.Item1.data);
var resBF = CalculateStats.CalculateParameters(predBF);
Console.WriteLine(resBF);
timer.Stop();
timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));

