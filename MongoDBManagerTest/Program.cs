// See https://aka.ms/new-console-template for more information
using DataSetManager;
using MongoDBManager;

Console.WriteLine("Hello, World!");
Console.WriteLine("Hello, World!");
string source = @"C:\data\belgica\belgica.txt";
string campaign = "Juli 2022";
string dataSeries = "ModelTest A1";
var data = FileDataSetManager.ReadData(source);
var sets = FileDataSetManager.MakeDataSetsWithTestSet(data.data, new List<int> { 500, 1000, 2500, 50000, 100000 });
string connString = "mongodb://localhost:27017";
MongoDBDataSetRepository repository = new MongoDBDataSetRepository(connString);
List<MongoDBDataSet> mds = new List<MongoDBDataSet>();
DataSetMetaInfo meta = new DataSetMetaInfo(sets[0].data.Count, source, campaign, dataSeries,DataSetType.TestSet);
MongoDBDataSet mdsSet = new MongoDBDataSet(sets[0], meta);
mds.Add(mdsSet);
for (int i = 1; i < sets.Count; i++)
{
    meta = new DataSetMetaInfo(sets[i].data.Count, source, campaign,dataSeries, DataSetType.DataSet);
    mdsSet = new MongoDBDataSet(sets[i], meta);
    mds.Add(mdsSet);
}
repository.WriteDataSets(mds);
Console.WriteLine("end");