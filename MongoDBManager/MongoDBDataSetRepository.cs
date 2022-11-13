using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBManager
{
    public class MongoDBDataSetRepository
    {
        private IMongoClient dbClient;
        private IMongoDatabase database;
        private string connectionString;

        public MongoDBDataSetRepository(string connectionString)
        {
            this.connectionString = connectionString;
            dbClient = new MongoClient(connectionString);
            database= dbClient.GetDatabase("Belgica");
        }
        public void WriteDataSets(List<MongoDBDataSet> datasets)
        {
            var collection = database.GetCollection<MongoDBDataSet>("datasets");
            collection.InsertMany(datasets);
        }
        public List<MongoDBDataSet> FindDataSets(string campaignCode,DataSetType dataSetType,string dataSeries)
        {
            var collection = database.GetCollection<MongoDBDataSet>("datasets");
            return collection.Find(x => x.metaInfo.CampaignCode == campaignCode 
            && x.metaInfo.DataSetType==dataSetType 
            && x.metaInfo.DataSeries== dataSeries).ToList();          
        }
        public List<MongoDBDataSet> FilterDataSets(string campaignCode, DataSetType dataSetType, string dataSeries)
        {
            var collection = database.GetCollection<MongoDBDataSet>("datasets");
            var filter1 = Builders<MongoDBDataSet>.Filter.Eq(x => x.metaInfo.DataSetType, dataSetType);
            var filter2 = Builders<MongoDBDataSet>.Filter.Eq(x => x.metaInfo.CampaignCode,campaignCode);
            var filter3 = Builders<MongoDBDataSet>.Filter.Eq(x => x.metaInfo.DataSeries, dataSeries);
            return collection.Find(filter1&filter2&filter3).ToList();
        }
        public MongoDBDataSet FindById(string id)
        {
            return database.GetCollection<MongoDBDataSet>("datasets").Find(x => x.id == ObjectId.Parse(id)).FirstOrDefault();
        }
        //public List<MongoDBDataSet> crudRead()
        //{
        //    var collection = database.GetCollection<MongoDBDataSet>("datasets");
        //    var filter1 = Builders<MongoDBDataSet>.Filter.Eq(x => x.metaInfo.DataSetType, DataSetType.DataSet);
        //    var filter2 = Builders<MongoDBDataSet>.Filter.Eq(x => x.metaInfo.CampaignCode, "Juli 2022");
        //    var filter3 = Builders<MongoDBDataSet>.Filter.Eq(x => x.metaInfo.DataSeries, "ModelTest A");
        //    var res = collection.Find(filter1 & filter2 & filter3).ToList();
        //    //Console.WriteLine(res2);
        //    ////methode 3
        //    //var res3 = collection.AsQueryable<MongoDBDataSet>().Where(x => x.metaInfo.NrOfDataPoints > 2000).ToList();
        //    //Console.WriteLine(res3);
        //    return res;
        //}
    }
}
