using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CommonLib;

namespace BuildXBuffer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Config.Load())
            {
                Console.WriteLine("请添加配置文件！");
                return;
            }

            if (!File.Exists(Config.Get("template")))
            {
                Console.WriteLine("请输入正确的模板文件路径");
                return;
            }

            if (!File.Exists(Config.Get("buffer_template")))
            {
                Console.WriteLine("请输入正确的buffer模板文件路径");
                return;
            }

            var csvPath = Config.Get("csv_path");
            var csvFileList = Directory.GetFiles(csvPath);
            GenXBuffer(csvFileList);
        }

        static void GenXBuffer(string[] csvFileList)
        {
            //var dllPath = Config.Get("dll_path");
            //Assembly asm = Assembly.LoadFrom(dllPath);
            //CustomTypeXBufferCreator customClassCreator = null;
            //if (asm != null)
            //{
            //    customClassCreator = new CustomTypeXBufferCreator(asm);
            //}

            List<ClassInfo> classList = new List<ClassInfo>();
            ClassInfo configManagerClass = new ClassInfo(Config.Get("mgr_class_name"));
            for (int i = 0; i < csvFileList.Length; ++i)
            {
                var csvFile = csvFileList[i];
                if (csvFile.EndsWith(".csv"))
                {
                    Console.WriteLine("--- Collect Class Info ---" + i + "---" + Path.GetFileName(csvFile));

                    CsvReader reader = CsvReader.ReadFile(csvFile);
                    var classInfo = CsvParser.Parse(reader.name, reader.table);
                    classList.Add(classInfo);

                    configManagerClass.propertys.Add(new CommonLib.PropertyInfo() { name = classInfo.name, type = classInfo.name + "s", desc = classInfo.name });

                    //if (customClassCreator != null)
                    //{
                    //    customClassCreator.AddClass(classInfo);
                    //}
                }
            }

            //自定义类生成proto文件 
            //{
            //    var customClassProtoString = customClassCreator.ToProtoString();
            //    File.WriteAllText(Config.Get("out_path") + "/../custom_class.xb", customClassProtoString);
            //}

            if (Directory.Exists(Config.Get("cs_out_path")))
                Directory.Delete(Config.Get("cs_out_path"), true);
            Directory.CreateDirectory(Config.Get("cs_out_path"));
            

            var template_str = File.ReadAllText(Config.Get("template"));
            var reader_template_str = File.ReadAllText(Config.Get("buffer_template"));
            var collections_template_str = File.ReadAllText(Config.Get("collections_template"));
            var collections_buffer_template_str = File.ReadAllText(Config.Get("collections_buffer_template"));

            var output = "";
            var showHead = true;
            foreach (var classItem in classList)
            {
                System.Console.WriteLine("Gen Class >>>>>>> " + classItem.name);

                output = Parser.Parse(classItem, template_str, showHead);
                showHead = false;
                File.WriteAllText(Config.Get("cs_out_path") + "/" + classItem.name + Config.Get("suffix"), output);

                output = Parser.Parse(classItem, reader_template_str, showHead);
                File.WriteAllText(Config.Get("cs_out_path") + "/" + classItem.name + "Buffer" + Config.Get("suffix"), output);

                output = Parser.Parse(classItem, collections_template_str, showHead);
                File.WriteAllText(Config.Get("cs_out_path") + "/" + classItem.name + "s" + Config.Get("suffix"), output);

                output = Parser.Parse(classItem, collections_buffer_template_str, showHead);
                File.WriteAllText(Config.Get("cs_out_path") + "/" + classItem.name + "sBuffer" + Config.Get("suffix"), output);
            }

            // 导出ConfigManager
            {
                System.Console.WriteLine("Gen Class >>>>>>> " + configManagerClass.name);

                output = Parser.Parse(configManagerClass, template_str, showHead);
                File.WriteAllText(Config.Get("cs_out_path") + "/" + configManagerClass.name + Config.Get("suffix"), output);

                var config_mgr_buffer_template = File.ReadAllText(Config.Get("config_mgr_buffer_template"));
                output = Parser.Parse(configManagerClass, config_mgr_buffer_template, showHead);
                File.WriteAllText(Config.Get("cs_out_path") + "/" + configManagerClass.name + "Buffer" + Config.Get("suffix"), output);
            }
        }
    }
}
