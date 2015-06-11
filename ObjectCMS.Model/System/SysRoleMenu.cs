using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.System
{
    [ModelAttribute(TableName = "SysRoleMenu", Fields = new string[] { "RoleId", "MenuId" })]
    public class SysRoleMenu : BaseModel<SysRoleMenu>
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }
}
