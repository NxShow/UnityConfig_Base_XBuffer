/*
 * File Name:               Parser.cs
 *
 * Description:             将类对象转化成代码文本
 * Author:                  lisiyu <576603306@qq.com>
 * Create Date:             2017/10/25
 */

namespace CommonLib
{
    public class Parser
    {
        /// <summary>
        /// 将类对象转化成代码文本
        /// </summary>
        /// <param name="proto_class">类结构</param>
        /// <param name="template_str">模板文本</param>
        /// <returns></returns>
        public static string Parse(ClassInfo proto_class, string template_str, bool showHead)
        {
            var template = new XTemplate(template_str);

            template.setCondition("HEAD", showHead);
            template.setValue("#CLASS_TYPE#", "class");
            template.setValue("#CLASS_NAME#", proto_class.name);
            template.setValue("#CLASS_COMMENT#", "Auto Created " + proto_class.name);


            // 设置Key
            {
                PropertyInfo keyProperty = null;
                foreach (var item in proto_class.propertys)
                {
                    if (item.key)
                    {
                        keyProperty = item;
                    }
                }

                if (keyProperty == null)
                {
                    keyProperty = proto_class.propertys[0];
                }
                template.setValue("#KEY_TYPE#", keyProperty.type);
                template.setValue("#KEY_NAME#", keyProperty.name);
            }

            template.setCondition("DESERIALIZE_CLASS", true);
            template.setCondition("SERIALIZE_CLASS", true);

            if (template.beginLoop("#VARIABLES#"))
            {
                foreach (var item in proto_class.propertys)
                {
                    template.setCondition("SINGLE", !item.IsArray);
                    template.setCondition("ARRAY", item.IsArray);
                    template.setValue("#VAR_TYPE#", item.Type);
                    template.setValue("#VAR_NAME#", item.name);
                    template.setValue("#VAR_COMMENT#", item.desc);
                    template.nextLoop();
                }
                template.endLoop();
            }

            if (template.beginLoop("#DESERIALIZE_PROCESS#"))
            {
                foreach (var item in proto_class.propertys)
                {
                    template.setCondition("SINGLE", !item.IsArray);
                    template.setCondition("ARRAY", item.IsArray);
                    template.setValue("#VAR_TYPE#", item.Type);
                    template.setValue("#VAR_NAME#", item.name);
					template.setValue("#VAR_COMMENT#", item.desc);
                    template.nextLoop();
                }
                template.endLoop();
            }

            if (template.beginLoop("#DESERIALIZE_RETURN#"))
            {
                foreach (var item in proto_class.propertys)
                {
					template.setValue("#VAR_TYPE#", item.Type);
                    template.setValue("#VAR_NAME#", item.name);
					template.setValue("#VAR_COMMENT#", item.desc);
                    template.nextLoop();
                }
                template.endLoop();
            }

            if (template.beginLoop("#SERIALIZE_PROCESS#"))
            {
                foreach (var item in proto_class.propertys)
                {
                    template.setCondition("SINGLE", !item.IsArray);
                    template.setCondition("ARRAY", item.IsArray);
                    template.setValue("#VAR_TYPE#", item.Type);
                    template.setValue("#VAR_NAME#", item.name);
					template.setValue("#VAR_COMMENT#", item.desc);
                    template.nextLoop();
                }
                template.endLoop();
            }

            return template.getContent();
        }
    }
}