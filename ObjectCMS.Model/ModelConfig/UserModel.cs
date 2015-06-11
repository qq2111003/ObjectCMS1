using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "UserModel", Fields = new string[] { "TableName","Title" })]
    public class UserModel : BaseModel<UserModel>
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Title { get; set; }
    }
}
