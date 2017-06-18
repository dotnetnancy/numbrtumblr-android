using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.ViewModel;
using SQLite;
using Xamarin.Forms;

namespace NumbrTumblr.Data
{
    public class SQLiteLotteryDatabase
    {
        private static SQLite.SQLiteConnection _connection;
        private static SQLiteLotteryDatabase _database;

        public static SQLiteLotteryDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new SQLiteLotteryDatabase();
                }
                return _database;
            }
        }

        public SQLiteLotteryDatabase()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            //here we could create our tables etc lets say 
            // create table -> will not create table if it exists already 
            //_connection.CreateTable<SomeDefinedDTO> ();
            bool result = CreateDatabaseIfNotExists();

        }

        public static SQLite.SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = DependencyService.Get<ISQLite>().GetConnection();
                }
                return _connection;
            }
        }

        public bool CreateDatabaseIfNotExists()
        {
            bool success = false;
            try
            {
                // create table -> will not create table if it exists already 
                //_connection.CreateTable<SomeDefinedDTO> ();

                Connection.CreateTable<StateProvince>();
                Connection.CreateTable<Lottery>();
                Connection.CreateTable<NumberSet>();
                Connection.CreateTable<NumberSetNumber>();
                Connection.CreateTable<Shuffle>();
                Connection.CreateTable<ShuffleNumber>();
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        public IEnumerable<Lottery> GetLottery()
        {
            return (from t in Connection.Table<Lottery>()
                    select t).ToList();
        }

        public Lottery GetLottery(int id)
        {
            return Connection.Table<Lottery>().FirstOrDefault(t => t.LotteryID == id);
        }

        public int DeleteLottery(int id)
        {
            lock (Connection)
            {
                //return the number of items deleted
                return _connection.Delete<Lottery>(id); //this will be more complicated need to delete fk first i imagine
            }
        }

        public int AddLottery(Lottery lottery)
        {
            lock (Connection)
            {
                //returns the primary key of the item inserted if it has a auto increment key
                return Connection.Insert(lottery);
            }
        }

        public IEnumerable<NumberSet> GetNumberSet()
        {
            return (from t in Connection.Table<NumberSet>()
                    select t).ToList();
        }

        public NumberSet GetNumberSet(int id)
        {
            return Connection.Table<NumberSet>().FirstOrDefault(t => t.NumberSetID == id);
        }

        public int DeleteNumberSet(int id)
        {
            lock (Connection)
            {
                return _connection.Delete<NumberSet>(id); //this will be more complicated need to delete fk first i imagine
            }
        }

        public int AddNumberSet(NumberSet NumberSet)
        {
            lock (Connection)
            {
                return Connection.Insert(NumberSet);
            }
        }


        public IEnumerable<NumberSetNumber> GetNumberSetNumber()
        {
            return (from t in Connection.Table<NumberSetNumber>()
                    select t).ToList();
        }

        public NumberSetNumber GetNumberSetNumber(int id)
        {
            return Connection.Table<NumberSetNumber>().FirstOrDefault(t => t.NumberSetNumberID == id);
        }

        public void DeleteNumberSetNumber(NumberSetNumber numberSetNumber)
        {
            lock (Connection)
            {
                var numberSetNumberFromDb =
                    GetNumberSetNumberByNumberSetIdAndNumberSetNumber(numberSetNumber.NumberSetID,
                        numberSetNumber.Number);
                if (numberSetNumberFromDb != null)
                    _connection.Delete<NumberSetNumber>(numberSetNumberFromDb.NumberSetNumberID); //this will be more complicated need to delete fk first i imagine
            }
        }

        public int DeleteNumberSetNumber(int id)
        {
            lock (Connection)
            {
                return _connection.Delete<NumberSetNumber>(id); //this will be more complicated need to delete fk first i imagine
            }
        }

        private int UpdateShuffleAndNumbers(Shuffle shuffle)
        {
            int shuffleUpdateResult = default(int);
            lock(Connection)
            {
                shuffleUpdateResult = Connection.Update(shuffle);
                if(shuffleUpdateResult > 0)
                {
                    foreach(ShuffleNumber shuffleNumber in shuffle.ResultOfShuffle)
                    {
                        int shuffleNumberUpdateResult = Connection.Update(shuffleNumber);
                        if(shuffleNumberUpdateResult <= 0)
                        {
                            //throw an error
                        }
                    }
                }
            }
            return shuffleUpdateResult;
        }

        private Shuffle SaveNewShuffleAndNumbers(Shuffle shuffle)
        {
            int shuffleInsertResult = default(int);
            lock (Connection)
            {
                shuffle.NumberSetID = shuffle.NumberSet.NumberSetID;
                shuffle.ShuffleDateTime = DateTime.UtcNow;
                shuffleInsertResult = Connection.Insert(shuffle);
                if (shuffleInsertResult > 0 && shuffle.ShuffleID > 0)
                {
                    var shuffleID = shuffle.ShuffleID;
                    foreach (ShuffleNumber shuffleNumber in shuffle.ResultOfShuffle)
                    {
                        shuffleNumber.ShuffleID = shuffleID;
                        int shuffleNumberInsertResult = Connection.Insert(shuffleNumber);
                        if (!(shuffleNumberInsertResult > 0 && shuffleNumber.ShuffleNumberID > 0))
                        {
                            //throw an error 
                        }
                    }
                }
                else
                {
                    //throw an error
                }
            }
            return shuffle;
        }

        public int AddNumberSetNumber(NumberSetNumber numberSetNumber)
        {
            lock (Connection)
            {
                if (
                    GetNumberSetNumberByNumberSetIdAndNumberSetNumber(numberSetNumber.NumberSetID,
                        numberSetNumber.Number) == null)
                    return Connection.Insert(numberSetNumber);
                else
                    return 0;
            }
        }

        public IEnumerable<Shuffle> GetShuffle()
        {
            return (from t in Connection.Table<Shuffle>()
                    select t).ToList();
        }

        public Shuffle GetShuffle(int id)
        {
            var shuffle = Connection.Table<Shuffle>().FirstOrDefault(t => t.ShuffleID == id);
            shuffle.ResultOfShuffle = Connection.Table<ShuffleNumber>().Where(x => x.ShuffleID == id).ToList();
            if (shuffle != null)
            {
                shuffle.NumberSet = Connection.Table<NumberSet>().FirstOrDefault(y => y.NumberSetID == shuffle.NumberSetID);
            }
            return shuffle;
        }

        internal int DeleteNumberSet(NumberSet numberSet)
        {
            if (numberSet.NumberSetNumbers == null || numberSet.NumberSetNumbers.Count == 0)
            {
                //then try to load the numbers in case they exist and delete them too
                numberSet.NumberSetNumbers = GetNumberSetNumbersByNumberSetId(numberSet.NumberSetID);
            }
            if (numberSet.NumberSetNumbers != null)
            {
                foreach (NumberSetNumber numberSetNumber in numberSet.NumberSetNumbers)
                {
                    DeleteNumberSetNumber(numberSetNumber.NumberSetNumberID);
                }
            }
            return DeleteNumberSet(numberSet.NumberSetID);
        }

        public int DeleteShuffle(int id)
        {
            lock (Connection)
            {
                return _connection.Delete<Shuffle>(id); //this will be more complicated need to delete fk first i imagine
            }
        }

        public int AddShuffle(Shuffle Shuffle)
        {
            lock (Connection)
            {
                return Connection.Insert(Shuffle);
            }
        }

        public IEnumerable<ShuffleNumber> GetShuffleNumber()
        {
            return (from t in Connection.Table<ShuffleNumber>()
                    select t).ToList();
        }

        public ShuffleNumber GetShuffleNumber(int id)
        {
            return Connection.Table<ShuffleNumber>().FirstOrDefault(t => t.ShuffleNumberID == id);
        }

        public int DeleteShuffleNumber(int id)
        {
            lock (Connection)
            {
                return _connection.Delete<ShuffleNumber>(id); //this will be more complicated need to delete fk first i imagine
            }
        }

        internal int DeleteShuffle(Shuffle shuffle)
        {
            lock (Connection)
            {
                foreach (ShuffleNumber shuffleNumber in shuffle.ResultOfShuffle)
                {
                    _connection.Delete<ShuffleNumber>(shuffleNumber.ShuffleNumberID);
                }
                return _connection.Delete<Shuffle>(shuffle.ShuffleID);
            }
        }

        public int AddShuffleNumber(ShuffleNumber ShuffleNumber)
        {
            lock (Connection)
            {
                return Connection.Insert(ShuffleNumber);
            }
        }

        public NumberSet GetNumberPool()
        {
            return Connection.Table<NumberSet>().FirstOrDefault(t => t.IsApplicationNumberPool);
        }

        public List<ShuffleNumber> GetShuffleNumbersByShuffleId(int shuffleId)
        {
            return Connection.Table<ShuffleNumber>().Where(x => x.ShuffleID == shuffleId).ToList();
        }

        public List<NumberSetNumber> GetNumberSetNumbersByNumberSetId(int numberSetId)
        {
            return Connection.Table<NumberSetNumber>().Where(x => x.NumberSetID == numberSetId).ToList();
        }

        public Lottery AddOrUpdateLottery(Lottery lottery)
        {
            int primaryKey = default(int);
            int countRecordsAffected = default(int);
            if (lottery != null)
            {
                if (lottery.LotteryID > 0)
                {
                    //it exists so update
                    countRecordsAffected = SaveLottery(lottery);
                    primaryKey = lottery.LotteryID;
                }
                else
                {
                    //probably new
                   AddLottery(lottery);
                    primaryKey = lottery.LotteryID;

                }
                if (primaryKey <= 0 && countRecordsAffected <= 0)
                {
                    //throw an error
                }
                else
                {
                    return GetLottery(primaryKey);
                }
            }
            return null;
        }

        private int SaveLottery(Lottery lottery)
        {
            lock (Connection)
            {
                return Connection.Update(lottery);
            }
        }

        public Lottery GetLotteryByName(string newTextValue)
        {
            return Connection.Table<Lottery>().FirstOrDefault(y => y.LotteryName == newTextValue);
        }

        public NumberSet GetNumberSetByName(string newTextValue)
        {
            return Connection.Table<NumberSet>().FirstOrDefault(y => y.NumberSetName == newTextValue);
        }

        internal Shuffle SaveShuffle(Shuffle shuffle)
        {
            Shuffle resultShuffle;
            int primaryKey = default(int);
            int countRecordsAffected = default(int);
            NumberSet numberSet = shuffle.NumberSet;

            if (numberSet != null  || shuffle.NumberSetID > 0)
            {
                if (shuffle.ShuffleID > 0)
                {
                    //it exists so update
                    countRecordsAffected = UpdateShuffleAndNumbers(shuffle);
                    primaryKey = shuffle.ShuffleID;
                }
                else
                {
                    //probably new
                   resultShuffle = SaveNewShuffleAndNumbers(shuffle);
                    if(resultShuffle != null && resultShuffle.ShuffleID > 0)
                    {
                        countRecordsAffected = 1;
                        primaryKey = resultShuffle.ShuffleID;
                    }

                }
                if (primaryKey <= 0 && countRecordsAffected <= 0)
                {
                    //throw an error
                }
                else
                {
                    return GetShuffle(primaryKey);
                }
            }
            return null;
        }

        public async Task<NumberSet> AddOrUpdateNumberSet(NumberSetViewModel numberSetViewModel)
        {
            int primaryKey = default(int);
            int countRecordsAffected = default(int);
            NumberSet numberSet = numberSetViewModel.NumberSet;

            if (numberSet != null)
            {
                if (numberSet.NumberSetID > 0)
                {
                    //it exists so update
                    countRecordsAffected = SaveNumberSet(numberSet);
                    primaryKey = numberSet.NumberSetID;
                }
                else
                {
                    //probably new
                    AddNumberSet(numberSet);
                    primaryKey = numberSet.NumberSetID;

                }
                if (primaryKey <= 0 && countRecordsAffected <= 0)
                {
                    //throw an error
                }
                else
                {
                    return GetNumberSet(primaryKey);
                }
            }

            if (numberSetViewModel.Lottery != null)
            {
                //TODO: save the numberSet Lottery association in a table, read it when you do a get
            }
            return null;
        }

        private int SaveNumberSet(NumberSet numberSet)
        {
            lock (Connection)
            {
                return Connection.Update(numberSet);
            }
        }

        public NumberSetNumber GetNumberSetNumberByNumberSetIdAndNumberSetNumber(int numberSetId, int number)
        {
            return Connection.Table<NumberSetNumber>().FirstOrDefault(y => y.NumberSetID == numberSetId &&
                                                                        y.Number == number);
        }

        public List<Shuffle> GetShufflesByNumberSet(int numberSetID)
        {
            List<Shuffle> foundShuffles = new List<Shuffle>();
            var shuffles =  Connection.Table<Shuffle>().Where(t => t.NumberSetID == numberSetID);
            foreach(Shuffle shuffle in shuffles)
            {
                var foundShuffle = GetShuffle(shuffle.ShuffleID);
                if(foundShuffle != null)
                {
                    foundShuffles.Add(foundShuffle);
                }
            }
            return foundShuffles;
        }

        public List<StateProvince> GetStateProvinces()
        {
            return Connection.Table<StateProvince>().ToList();
        }
    }
}
