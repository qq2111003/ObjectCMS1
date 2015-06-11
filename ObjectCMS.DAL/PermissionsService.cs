using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ObjectCMS.Common;
using ObjectCMS.Model.System;
using ObjectCMS.Model.ModelConfig;

namespace ObjectCMS.DAL
{
    public class PermissionsService : DataProviderBase
    {
        public DataTable GetAllRole(int adminId)
        {
            string sql = "select a.*, (case when a.id = b.roleid then 1 else 0 end) as flag from SysRoles a left join SysAdminRoles b on a.id = b.roleid and b.adminid=" + adminId;
            return MainDB.ExecuteDataTable(CommandType.Text, sql);
        }

        public DataTable GetAllMenus(int roleId)
        {
            string sql = @"select a.Id,a.ParentId,a.Title,'' as Url,(case when a.id = b.menuid then 1 else 0 end) as flag from SysMenu a left join SysRoleMenu b on a.id = b.menuid and b.roleid=" + roleId;
            var dt = MainDB.ExecuteDataTable(CommandType.Text, sql);

            sql = "select a.Id+10000 as Id,(case when a.ParentId = 0 then 16 else a.ParentId+10000 end) as ParentId,a.Title,'' as Url, (case when a.id = b.nodeid then 1 else 0 end) as flag from Node a left join RoleNode b on a.id = b.nodeid and b.roleid=" + roleId;
            var dt1 = CurrentDB.ExecuteDataTable(CommandType.Text, sql);
            var obj = new object[dt.Columns.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            return dt;
        }
        public List<SysMenu> GetAllMenusForAdmin(int adminId, string where = "1=1")
        {
            string sql = "select * from SysMenu where Id in (select MenuId from SysRoleMenu where roleId in (select RoleId from SysAdminRoles where AdminId = " + adminId + ")) and " + where + " order by SORT DESC";
            return SysMenu.Translate(MainDB.ExecuteDataTable(CommandType.Text, sql));
        }

        private string GetAllAdminRoleIds(int adminId)
        {
            string sql = "select RoleId from SysAdminRoles where AdminId = " + adminId;
            var roleids = MainDB.ExecuteDataTable(CommandType.Text, sql);

            string roleidArrays = "0";
            for (int i = 0; i < roleids.Rows.Count; i++)
            {
                roleidArrays += "," + roleids.Rows[i][0].ToString();
            }
            return roleidArrays;
        }

        public List<Node> GetAllNodeTreeForAdmin(int adminId, string where = "1=1")
        {
            string sql = "select * from Node where Id in (select nodeId from RoleNode where roleId in (" + GetAllAdminRoleIds(adminId) + ")) and " + where + " order by Id ";
            return Node.Translate(CurrentDB.ExecuteDataTable(CommandType.Text, sql));
        }
        public DataTable GetAllControls(int roleId, int menuId)
        {
            string sql = "select a.*,(case when a.id = b.ControlId then 1 else 0 end) as flag from SysMenuControls a left join SysRoleControl b on a.id = b.ControlId and b.roleid=" + roleId + " where a.menuid=" + menuId;
            return MainDB.ExecuteDataTable(CommandType.Text, sql);
        }
        public DataTable GetAllNodeControls(int roleId, int nodeId)
        {
            string sql = "select * from RoleNodeControl where NodeControlMark like '" + nodeId + "_%' and RoleId=" + roleId;
            return CurrentDB.ExecuteDataTable(CommandType.Text, sql);

        }
        public SysAdmin AdminLoginCheck(string username, string password, out int status)
        {
            SqlParameter[] pars = new SqlParameter[]{
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50),
                new SqlParameter("@UserPwd", SqlDbType.VarChar, 32),
                new SqlParameter("@Status", SqlDbType.Int)
			};
            pars[0].Value = username;
            pars[1].Value = password;
            pars[2].Direction = ParameterDirection.Output;
            var result = MainDB.ExecuteDataTable(CommandType.StoredProcedure, "AdminLogin", pars);
            status = pars[2].Value.ToInt();
            if (status == -1)
            {
                return SysAdmin.Translate(result)[0];
            }
            return null;
        }
        public bool HasPermission(int adminId, int menuId)
        {
            if (menuId < 10000)
            {
                string sql = "select count(Id) as flag from SysRoleMenu where RoleId in (select RoleId from SysAdminRoles where AdminId = " + adminId + ") and SysRoleMenu.menuid = " + menuId;
                return MainDB.ExecuteScalar(CommandType.Text, sql).ToInt() == 1;
            }
            else
            {
                string sql = "select count(Id) as flag from RoleNode where RoleId in (" + GetAllAdminRoleIds(adminId) + ") and RoleNode.nodeid = " + (menuId - 10000);
                return CurrentDB.ExecuteScalar(CommandType.Text, sql).ToInt() == 1;
            }
        }
        public bool HasPermission(int adminId, int menuId, string ctrlId)
        {
            if (menuId < 10000)
            {
                string sql = "select count(id) as flag from SysRoleControl where SysRoleControl.RoleId in (select RoleId from SysAdminRoles where AdminId = " + adminId + ") and SysRoleControl.ControlId in (select Id from SysMenuControls where MenuId=" + menuId + " and CtrlId='" + ctrlId + "')";
                return MainDB.ExecuteScalar(CommandType.Text, sql).ToInt() == 1;
            }
            else
            {
                string sql = "select count(id) as flag from RoleNodeControl where RoleNodeControl.RoleId in (" + GetAllAdminRoleIds(adminId) + ") and RoleNodeControl.NodeControlMark = '" + menuId + "_" + ctrlId + "'";
                return CurrentDB.ExecuteScalar(CommandType.Text, sql).ToInt() == 1;
            }
        }
    }
}
