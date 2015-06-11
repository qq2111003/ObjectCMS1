using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model
{
    [ModelAttribute(TableName = "TeTemplateType", Fields = new string[] { "TypeName" })]
    public class TeTemplateType : BaseModel<TeTemplateType>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

    }
}