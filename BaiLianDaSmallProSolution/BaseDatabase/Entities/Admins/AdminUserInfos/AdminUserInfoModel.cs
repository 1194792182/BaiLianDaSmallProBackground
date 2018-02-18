using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase.Entities.Admins.AdminUserInfos
{
    public class AdminUserInfoModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PwdSalt { get; set; }

        public DateTime CreateOn { get; set; }
    }
}
