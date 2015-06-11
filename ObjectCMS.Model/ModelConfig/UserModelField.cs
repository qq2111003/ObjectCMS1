using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "UserModelField", Fields = new string[] { "UserModelId", "FieldName", "Title", "FieldTypeId", "Sort" })]
    public class UserModelField : BaseModel<UserModelField>
    {
        public int Id { get; set; }
        public int UserModelId { get; set; }
        public string FieldName { get; set; }
        public string Title { get; set; }
        public int FieldTypeId { get; set; }
        public int Sort { get; set; }
    }
}
