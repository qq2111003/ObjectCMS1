using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.System
{
    [ModelAttribute(TableName = "SysRoles", Fields = new string[] { "Name", "Enable" })]
    public class SysRoles : BaseModel<SysRoles>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
}
