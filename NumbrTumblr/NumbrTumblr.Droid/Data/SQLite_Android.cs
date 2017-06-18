using System.IO;
using System.Runtime.CompilerServices;
using NumbrTumblr.Droid.Data;
using NumbrTumblr.Data;
using SQLite;

//note the dependency service assembly attribute is at the namespace of course
[assembly:Xamarin.Forms.Dependency(typeof(SQLite_Android))]
namespace NumbrTumblr.Droid.Data
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
            
        }

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "NumbrTumblr.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true);
            // Return the database connection
            return conn;
        }
    }
}