/**************************************************************
 *  Filename:    CsvParser.cs
 *  Copyright:   Microsoft Co., Ltd.
 *
 *  Description: CsvParser ClassFile.
 *
 *  @author:     xiaobai
 *  @version     2018/5/30 10:49:05  @Reviser  Initial Version
 **************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLib
{
    public class PropertyInfo
    {
        public string name;
        public string type;
        public string desc;
        public bool key;

        public bool IsArray { get { return type.EndsWith("[]"); } }
        public string Type { get { return type.Replace("[]", ""); } }
    }

    public class ClassInfo
    {
        public string name;
        public List<PropertyInfo> propertys;

        public ClassInfo(string name)
        {
            this.name = name;
            propertys = new List<PropertyInfo>();
        }
    }

    public class CsvParser
    {
        public const int CSV_KEY_SIGN_ROW = 0;
        public const int CSV_PROPERTY_NAME_ROW = 1;
        public const int CSV_PROPERTY_TYPE_ROW = 2;
        public const int CSV_PROPERTY_TYPE1_ROW = 3;
        public const int CSV_IGNORED_ROW_COUNT = 5;

        public static ClassInfo Parse(string name, string[,] table)
        {
            ClassInfo classInfo = new ClassInfo(name);
            for (int i = 0; i < table.GetLength(1); i++)
            {
                var propertyName = table[CSV_PROPERTY_NAME_ROW, i];
                if (string.IsNullOrEmpty(propertyName))  // 字段名 第二行
                    continue;
                // custom type
                var customType = table[CSV_PROPERTY_TYPE1_ROW, i];  //第四行
                if (customType == "Invalid")
                    continue;
                if (string.IsNullOrEmpty(customType) || customType == "ObscuredInt")       //默认int, 暂时不支持加密Int
                    customType = "int";

                var isKey = table[CSV_KEY_SIGN_ROW, i];
                bool key = isKey == "1";

                var desc = table[CSV_IGNORED_ROW_COUNT - 1, i]; // 注释   第5行

                PropertyInfo property = new PropertyInfo
                {
                    name = propertyName,
                    type = customType,
                    desc = desc,
                    key = key
                };
                classInfo.propertys.Add(property);
            }
            return classInfo;
        }
    }
}