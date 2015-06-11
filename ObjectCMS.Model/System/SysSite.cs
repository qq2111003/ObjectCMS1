using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.System
{
    [ModelAttribute(TableName = "SysSite", Fields = new string[] { "Name", "SiteMark", "IsDefault", "DbUid", "DbUpass" })]
    public class SysSite : BaseModel<SysSite>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SiteMark { get; set; }
        public bool IsDefault { get; set; }
        public string DbUid { get; set; }
        public string DbUpass { get; set; }
    }
}