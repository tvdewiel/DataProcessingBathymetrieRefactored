// See https://aka.ms/new-console-template for more information
using DataSetManager;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
Console.WriteLine("Hello, World! grid");
var timer = new Stopwatch();
timer.Start();
var data = FileDataSetManager.ReadData(@"C:\data\belgica\belgica.txt");
timer.Stop();
TimeSpan timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
timer.Reset();
timer.Start();
var setsGrid = FileDataSetManager.MakeGridDataSetsWithTestSet(data,10000, new List<(int, double)> { (10000, 20), (20000, 20), (200000, 100), (500000, 100) });
timer.Stop();
timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
timer.Reset();
timer.Start();
var setsSeq = FileDataSetManager.MakeDataSetsWithTestSet(data.data, new List<int> { 10000,10000, 20000,200000,500000 });
timer.Stop();
timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));

//FileDataSetManager.WriteDataSet(sets.Item1, @"C:\data\belgica\grid\TS10kg.txt");
//FileDataSetManager.WriteGridDataSets(sets.Item2, new List<string> { @"C:\data\belgica\grid\DS20kd20.txt", @"C:\data\belgica\grid\DS200kd100.txt", @"C:\data\belgica\grid\DS500kd100.txt" });//, @"C:\data\belgica\out\DS200kg200x200.txt", @"C:\data\belgica\out\DS1gg200x200.txt" });
//FileDataSetManager.WriteDataSets(sets.Item3, new List<string> { @"C:\data\belgica\grid\TS10ks.txt", @"C:\data\belgica\grid\DS20ks.txt", @"C:\data\belgica\grid\DS200ks.txt", @"C:\data\belgica\grid\DS500ks.txt" });

