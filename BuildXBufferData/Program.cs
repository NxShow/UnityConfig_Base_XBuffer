using CommonLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildXBufferData
{
    class Program
    {
        private const string INVALID_PROPERTY = "Invalid";

        private static Assembly _configAssembly;
        private static Assembly configAssembly
        {
            get
            {
                return _configAssembly;
                //var ass = System.AppDomain.CurrentDomain.GetAssemblies();
                //foreach (var s in ass)
                //{
                //    if (s.FullName.Contains("xbuffer_runtime")) return s;
                //}
                //return null;
            }
            set
            {
                _configAssembly = value;
            }
        }

        static void Main(string[] args)
        {
            if (!Config.Load())
            {
                Console.WriteLine("请添加配置文件！");
                return;
            }
            // TODO Dll path 改成xbuff_config的
            // 验证反射对没对
            Assembly asm = Assembly.LoadFrom("xbuffer_config.dll");
            if (asm == null)
            {
                Console.WriteLine("dll不存在！");
                return;
            }
            configAssembly = asm;

            var csvPath = Config.Get("csv_path");
            var csvFileList = Directory.GetFiles(csvPath);

            Type t = asm.GetType("xbuffer." + Config.Get("mgr_class_name"));
            //ConfigManager obj = new ConfigManager();
            //Type t = obj.GetType();
            object obj = asm.CreateInstance(t.FullName);

            for (int i = 0; i < csvFileList.Length; ++i)
            {
                var csvFile = csvFileList[i];
                if (csvFile.EndsWith(".csv"))
                {
                    Console.WriteLine("--- Insert Bytes Data ---" + i + "---" + Path.GetFileName(csvFile));

                    CsvReader reader = CsvReader.ReadFile(csvFile);
                    var classInfo = CsvParser.Parse(reader.name, reader.table);

                    FillFieldData(t, obj, classInfo, reader.table);
                }
            }

            xbuffer.XSteam stream = new xbuffer.XSteam(1, 1024 * 1024 * 100);
            Type bufferT = asm.GetType("xbuffer."+ Config.Get("mgr_class_name") +"Buffer");

            object[] arg = new object[2] { obj, stream };
            var serializeMethod = bufferT.GetMethod("serialize", BindingFlags.Static | BindingFlags.Public);
            if (serializeMethod != null)
            {
                serializeMethod.Invoke(null, arg);
                var buffer = stream.getBytes();
                buffer = CompressTool.CompressBytes(buffer);        //压缩一下
                File.WriteAllBytes(Config.Get("data_out_path") + "/" + "ConfigData.bytes", buffer);
            }
            else
            {
                throw new Exception("找不到方法 ===> xbuffer." + Config.Get("mgr_class_name") + "Buffer.serialize");
            }

            Console.WriteLine("Finish!!!");
        }

        public static void FillFieldData(Type configMgrType, object configMgrObj, ClassInfo classInfo, string[,] table)
        {
            var asm = configAssembly;

            var tableField = configMgrType.GetField(classInfo.name);
            if (tableField != null)
            {
                // TODO 重新设置读取表的配置的方式 
                Type tableType = tableField.FieldType;
                object tableObject = asm.CreateInstance(tableType.FullName);
                FieldInfo dataArrayField = tableType.GetField("_configList");
                object dataArrayObj = new object();
                dataArrayObj = dataArrayField.FieldType.InvokeMember("Set", BindingFlags.CreateInstance, null, dataArrayObj, new object[] { table.GetLength(0) - CsvParser.CSV_IGNORED_ROW_COUNT });

                for (int j = CsvParser.CSV_IGNORED_ROW_COUNT; j < table.GetLength(0); j++)
                {
                    //表格的一行
                    var tableItemObject = asm.CreateInstance("xbuffer." + classInfo.name);
                    var tableItemType = asm.GetType("xbuffer." + classInfo.name);

                    for (int k = 0; k < table.GetLength(1); k++)
                    {
                        var propertyExtraType = table[CsvParser.CSV_PROPERTY_TYPE1_ROW, k];
                        if (propertyExtraType == INVALID_PROPERTY)  // Ignore
                        {
                            continue;
                        }

                        var fieldName = table[CsvParser.CSV_PROPERTY_NAME_ROW, k];
                        if (fieldName == null)
                        {
                            throw new MissingFieldException(string.Format("属性名称为空 在文件<<{0}>>的第{1}列", classInfo.name, k + 1));
                        }

                        var raw = table[j, k];

                        var field = tableItemType.GetField(fieldName);
                        if (field == null)
                        {
                            throw new MissingFieldException(string.Format("丢失了列，请重头执行打表程序，在文件<<{0}>>的第{1}行，第{2}列", classInfo.name, j + 1, k + 1));
                        }

                        object fieldObject; //表格的一列
                        int errorCode = ParseType(out fieldObject, field.FieldType, raw);
                        if (errorCode != 0)
                        {
                            throw new Exception(string.Format("解析数据出错，数据内容为{0}, 列名{1}, 类型{2}，在文件<<{3}>>的第{4}行，第{5}列", raw, field.Name, field.FieldType, classInfo.name, j + 1, k + 1));
                        }

                        field.SetValue(tableItemObject, fieldObject);
                    }

                    dataArrayField.FieldType.GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) }).Invoke(dataArrayObj, new object[] { tableItemObject, j - CsvParser.CSV_IGNORED_ROW_COUNT });
                }

                dataArrayField.SetValue(tableObject, dataArrayObj);
                tableField.SetValue(configMgrObj, tableObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="propertyExtraType"></param>
        /// <param name="raw"></param>
        /// <returns>返回0表示成功解析，返回1表示数据不匹配</returns>
        public static int ParseType(out object obj, Type type, string raw)
        {
            if (type == typeof(int))  // parse int
            {
                int value = 0;
                int.TryParse(raw, out value);
                obj = value;
            }
            else if (type == typeof(float))
            {
                float value = 0;
                float.TryParse(raw, out value);
                obj = value;
            }
            else if (type == typeof(string))
            {
                obj = raw == null ? string.Empty : raw;
            }
            else   //自定义类型
            {
                bool isArray = type.IsArray;
                if (isArray)  //数组类型
                {
                    var itemType = type.GetElementType();
                    string[] rawArr = new string[0];
                    if (!string.IsNullOrEmpty(raw))
                    {
                        if (raw.IndexOf('|') >= 0)
                        {
                            rawArr = raw.Split('|');
                        }
                        else
                        {
                            rawArr = raw.Split(';');
                        }
                    }

                    obj = new object();
                    obj = type.InvokeMember("Set", BindingFlags.CreateInstance, null, obj, new object[] { rawArr.Length });
                    for (int i = 0; i < rawArr.Length; ++i)
                    {
                        var rawItem = rawArr[i];
                        object objItem;
                        int errorCode = ParseType(out objItem, itemType, rawItem);
                        if (errorCode != 0)
                        {
                            obj = null;
                            return errorCode;
                        }
                        else
                        {
                            var setValueMethod = type.GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) });
                            if (setValueMethod != null)
                                setValueMethod.Invoke(obj, new object[] { objItem, i });
                        }
                    }
                }
                else   //非数组类型，复杂的组合类
                {
                    string[] rawItem = new string[0];
                    if (!string.IsNullOrEmpty(raw))
                    {
                        if (raw.IndexOf(':') >= 0)
                        {
                            rawItem = raw.Split(':');
                        }
                        else
                        {
                            rawItem = raw.Split(' ');
                        }
                    }

                    var fields = type.GetFields();
                    if (fields.Length == 1 && fields[0].FieldType.IsArray)  //只支持如下格式的 class A { public B[] b; }
                    {
                        rawItem = new string[] { raw };
                    }
                    else if (raw == "0" && fields.Length != 1)     //特殊处理 这种情况下表示这一列读默认值
                    {
                        
                    }
                    else if (rawItem.Length == 0)           //空的处理 这种情况下表示这一列读默认值
                    {
                        
                    }
                    else if (fields.Length != rawItem.Length)  //如果最终数量不一致 就报错
                    {
                        obj = null;
                        return 1;
                    }

                    obj = configAssembly.CreateInstance(type.FullName);
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        var field = fields[i];

                        object fieldObject;
                        int errorCode = ParseType(out fieldObject, field.FieldType, rawItem.Length > i ? rawItem[i] : null);
                        if (errorCode != 0)
                        {
                            obj = null;
                            return errorCode;
                        }

                        field.SetValue(obj, fieldObject);
                    }
                }
            }
            return 0;
        }
    }
}
