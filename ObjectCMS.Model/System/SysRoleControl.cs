using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;


namespace ObjectCMS.Model.System
{

    [ModelAttribute(TableName = "SysRoleControl", Fields = new string[] { "RoleId", "ControlId" })]
    public class SysRoleControl : BaseModel<SysRoleControl>
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ControlId { get; set; }
    }
}
