// See https://aka.ms/new-console-template for more information
using DataSetManager;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
bool scenario1 = true;
bool scenario2 = true;
bool scenario3 = true;
bool scenario4 = true;
int loopcount = 3;
var timer = new Stopwatch();
timer.Start();
var data = FileDataSetManager.ReadData(@"C:\data\belgica\belgica.txt");
timer.Stop();
TimeSpan timeTaken = timer.Elapsed;
Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
for (int i = 0; i < loopcount; i++)
{
    //scenario 1 
    Console.WriteLine("scenario 1");
    if (scenario1)
    {
        timer.Reset();
        timer.Start();
        var sets = FileDataSetManager.MakeDataSetsWithTestSet(data.data, new List<int> { 50000, 100000, 250000, 500000, 1000000, 1500000, 2000000, 2500000 });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
        timer.Start();
        FileDataSetManager.WriteDataSets(sets,
            new List<string> { @"C:\data\belgica\out\perf\TSs.txt",
                @"C:\data\belgica\out\perf\DS1s.txt",
                @"C:\data\belgica\out\perf\DS2s.txt",
                @"C:\data\belgica\out\perf\DS3s.txt",
                @"C:\data\belgica\out\perf\DS4s.txt",
                @"C:\data\belgica\out\perf\DS5s.txt",
                @"C:\data\belgica\out\perf\DS6s.txt",
                @"C:\data\belgica\out\perf\DS7s.txt" });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
    }
    //scenario 2 
    Console.WriteLine("scenario 2 - threads");
    if (scenario2)
    {
        timer.Reset();
        timer.Start();
        var sets = FileDataSetManager.MakeDataSetsWithTestSet(data.data, new List<int> { 50000, 100000, 250000, 500000, 1000000, 1500000, 2000000, 2500000 });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
        timer.Start();
        FileDataSetManager.WriteDataSetsThreads(sets,
            new List<string> { @"C:\data\belgica\out\perf\TSsThreads.txt",
                    @"C:\data\belgica\out\perf\DS1sThreads.txt",
                    @"C:\data\belgica\out\perf\DS2sThreads.txt",
                    @"C:\data\belgica\out\perf\DS3sThreads.txt",
                    @"C:\data\belgica\out\perf\DS4sThreads.txt",
                    @"C:\data\belgica\out\perf\DS5sThreads.txt",
                    @"C:\data\belgica\out\perf\DS6sThreads.txt",
                    @"C:\data\belgica\out\perf\DS7sThreads.txt" });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
    }
    //scenario 3 
    Console.WriteLine("scenario 3 - tasks");
    if (scenario3)
    {
        timer.Reset();
        timer.Start();
        var sets = FileDataSetManager.MakeDataSetsWithTestSet(data.data, new List<int> { 50000, 100000, 250000, 500000, 1000000, 1500000, 2000000, 2500000 });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
        timer.Start();
        FileDataSetManager.WriteDataSetsTasks(sets,
            new List<string> { @"C:\data\belgica\out\perf\TSsTasks.txt",
                    @"C:\data\belgica\out\perf\DS1sTasks.txt",
                    @"C:\data\belgica\out\perf\DS2sTasks.txt",
                    @"C:\data\belgica\out\perf\DS3sTasks.txt",
                    @"C:\data\belgica\out\perf\DS4sTasks.txt",
                    @"C:\data\belgica\out\perf\DS5sTasks.txt",
                    @"C:\data\belgica\out\perf\DS6sTasks.txt",
                    @"C:\data\belgica\out\perf\DS7sTasks.txt" });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
    }
    //scenario 4 
    Console.WriteLine("scenario 4 - combi");
    if (scenario4)
    {
        timer.Reset();
        timer.Start();
        FileDataSetManager.MakeDataSetsWithTestSetAndSave(data.data, new List<int> { 50000, 100000, 250000, 500000, 1000000, 1500000, 2000000, 2500000 }, new List<string> { @"C:\data\belgica\out\perf\TSsCombi.txt",
                    @"C:\data\belgica\out\perf\DS1sCombi.txt",
                    @"C:\data\belgica\out\perf\DS2sCombi.txt",
                    @"C:\data\belgica\out\perf\DS3sCombi.txt",
                    @"C:\data\belgica\out\perf\DS4sCombi.txt",
                    @"C:\data\belgica\out\perf\DS5sCombi.txt",
                    @"C:\data\belgica\out\perf\DS6sCombi.txt",
                    @"C:\data\belgica\out\perf\DS7sCombi.txt" });
        timer.Stop();
        timeTaken = timer.Elapsed;
        Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
    }
}
Console.WriteLine("end");