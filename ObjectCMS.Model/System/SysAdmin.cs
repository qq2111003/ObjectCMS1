using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.System
{

    [ModelAttribute(TableName = "SysAdmin", Fields = new string[] { "Name", "Password", "LastLoginTime", "LoginTimes", "IsMultiLogin", "Enable" })]
    public class SysAdmin : BaseModel<SysAdmin>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int LoginTimes { get; set; }
        public bool IsMultiLogin { get; set; }
        public bool Enable { get; set; }

        //ext
        public string RPassword { get; set; }
    }
}
