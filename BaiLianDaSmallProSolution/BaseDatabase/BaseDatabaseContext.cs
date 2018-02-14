using BaseDatabase.Entities.BaseSettings;
using BaseDatabase.Entities.PayInfos;
using BaseDatabase.Entities.UserInfos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase
{
    public class BaseDatabaseContext : DbContext
    {
        public BaseDatabaseContext() : base("baseDbConn")
        {

        }

        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<BaseSetting> BaseSettings { get; set; }

        public DbSet<PayInfo> PayInfos { get; set; }
    }
}
