using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.TemplateEngine
{
    [ModelAttribute(TableName = "TeTemplateType", Fields = new string[] { "Title", "MarkName", "Ico" })]
    public class TeTemplateType : BaseModel<TeTemplateType>
    {

    }
}
