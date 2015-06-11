using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ObjectCMS.DAL;
using ObjectCMS.Model.System;
using ObjectCMS.Model.ModelConfig;

namespace ObjectCMS.BLL
{
    public class PermissionsManage
    {

        private readonly PermissionsService dal = new PermissionsService();
        public static readonly PermissionsManage Instance = new PermissionsManage();

        public DataTable GetAllRole(int adminId)
        {
            return dal.GetAllRole(adminId);
        }
        public DataTable GetAllMenus(int roleid)
        {
            return dal.GetAllMenus(roleid);
        }
        public List<SysMenu> GetAllMenusForAdmin(int adminId, string where = "1=1")
        {
            return dal.GetAllMenusForAdmin(adminId, where);
        }
        public List<Node> GetAllNodeTreeForAdmin(int adminId, string where = "1=1")
        {
            return dal.GetAllNodeTreeForAdmin(adminId, where);
        }
        public DataTable GetAllControls(int roleId, int menuId)
        {
            return dal.GetAllControls(roleId, menuId);
        }
        public DataTable GetAllNodeControls(int roleId, int nodeId)
        {
            return dal.GetAllNodeControls(roleId, nodeId);
        }
        public SysAdmin AdminLoginCheck(string username, string password, out int status)
        {
            return dal.AdminLoginCheck(username, password, out status);
        }

        public bool HasPermission(int adminId, int menuId)
        {
            return dal.HasPermission(adminId, menuId);
        }
        public bool HasPermission(int adminId, int menuId, string ctrlId)
        {
            return dal.HasPermission(adminId, menuId, ctrlId);
        }
    }
}
