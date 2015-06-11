using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "UserModelFieldType", Fields = new string[] { "TypeName", "Ico", "DBType" })]
    public class UserModelFieldType : BaseModel<UserModelFieldType>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Ico { get; set; }
        public string DBType { get; set; }
    }
}
