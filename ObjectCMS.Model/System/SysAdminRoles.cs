using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.System
{
    [ModelAttribute(TableName = "SysAdminRoles", Fields = new string[] {  "AdminId", "RoleId" })]
    public class SysAdminRoles : BaseModel<SysAdminRoles>
    {

        public int Id { get; set; }
        public int AdminId { get; set; }
        public int RoleId { get; set; }
    }
}
