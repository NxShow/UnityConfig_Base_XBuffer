using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class XBufferCreator
    {
        ClassInfo _classInfo;
        public XBufferCreator(ClassInfo classInfo)
        {
            _classInfo = classInfo;
        }

        public string ToProtoString()
        {
            if (_classInfo == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("// table_{0}\n", _classInfo.name);
            sb.Append("class ").Append(_classInfo.name).Append("\n{\n");

            for (int i = 0; i < _classInfo.propertys.Count; ++i)
            {
                var property = _classInfo.propertys[i];
                sb.Append("\t").Append(property.name).Append(":");

                if (property.type.EndsWith("[]"))
                {
                    sb.Append("[").Append(property.type.Replace("[]", "")).Append("];");
                }
                else
                {
                    sb.Append(property.type).Append(";");
                }

                if (!string.IsNullOrEmpty(property.desc))
                {
                    sb.Append("\t\t//").Append(property.desc.Replace(' ', '_'));
                }

                sb.Append("\n");
            }

            sb.Append("}\n\n");

            return sb.ToString();
        }
    }
}
