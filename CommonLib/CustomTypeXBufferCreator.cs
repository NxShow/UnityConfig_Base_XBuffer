using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class CustomTypeXBufferCreator
    {
        public List<string> _customType;
        public Assembly _asm;

        public CustomTypeXBufferCreator(Assembly asm)
        {
            _asm = asm;
            _customType = new List<string>();
        }

        public int AddClass(ClassInfo classInfo)
        {
            int count = 0;
            var propertys = classInfo.propertys;
            for (int i = 0; i < propertys.Count; ++i)
            {
                var property = propertys[i];

                var customType = property.Type;
                if (!_customType.Contains(customType))
                {
                    _customType.Add(customType);
                    ++count;
                }
            }
            return count;
        }

        public List<ClassInfo> GetCustomClass()
        {
            List<ClassInfo> classList = new List<ClassInfo>();
            for (int i = 0; i < _customType.Count; ++i)
            {
                var customType = _customType[i];
                Type t = _asm.GetType(customType);
                if (t != null)
                {
                    ClassInfo classItemInfo = new ClassInfo(customType);
                    var fields = t.GetFields();
                    for (int j = 0; j < fields.Length; ++j)
                    {
                        var field = fields[j];
                        if (field.IsPublic && !field.IsStatic)
                        {
                            var type = field.FieldType.Name;
                            type = type.Replace("Int32", "int");
                            type = type.Replace("String", "string");
                            type = type.Replace("Single", "float");
                            classItemInfo.propertys.Add(new PropertyInfo() { name = field.Name, type = type, desc = "param_" + j });
                            if (type.EndsWith("[]"))
                                type = type.Replace("[]", "");
                            if (!_customType.Contains(type))
                            {
                                _customType.Add(type);
                            }
                        }
                    }

                    classList.Add(classItemInfo);
                }
            }

            //强制生成 因为dll中木有
            ClassInfo classInfo = new ClassInfo("Vector3");
            classInfo.propertys.Add(new PropertyInfo() { name = "x", type = "float", desc = "param_1" });
            classInfo.propertys.Add(new PropertyInfo() { name = "y", type = "float", desc = "param_2" });
            classInfo.propertys.Add(new PropertyInfo() { name = "z", type = "float", desc = "param_3" });
            classList.Add(classInfo);
            return classList;
        }

        public string ToProtoString()
        {
            List<ClassInfo> customClassList = GetCustomClass();
            StringBuilder sb = new StringBuilder();
            foreach (var customClass in customClassList)
            {
                XBufferCreator creator = new XBufferCreator(customClass);
                sb.Append(creator.ToProtoString());
            }
            return sb.ToString();
        }
    }
}
