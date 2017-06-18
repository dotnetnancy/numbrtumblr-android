using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NumbrTumblr.Data
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetConnection();
    }
}
