/**************************************************************
 *  Filename:    CsvReader.cs
 *  Copyright:   Microsoft Co., Ltd.
 *
 *  Description: CsvReader ClassFile.
 *
 *  @author:     xiaobai
 *  @version     2018/5/30 10:45:52  @Reviser  Initial Version
 **************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class CsvReader
    {
        public static string[] stringSeparators = new string[] { "\n", "\r\n" };

        public string name;
        public string[,] table;

        public static CsvReader ReadFile(string csvFile)
        {
            if (!csvFile.EndsWith(".csv"))
            {
                Console.WriteLine("文件格式错误！只支持读取csv文件");
                return null;
            }

            StreamReader sr = new StreamReader(csvFile.Trim(), Encoding.UTF8);
            if (sr == null)
            {
                Console.WriteLine("文件路径有问题");
                return null;
            }
            string tableString = sr.ReadToEnd();

            CsvReader reader = new CsvReader
            {
                name = Path.GetFileNameWithoutExtension(csvFile),
                table = SplitCsvGrid(tableString)
            };
            return reader;
        }

        // splits a CSV file into a 2D string array
        public static string[,] SplitCsvGrid(string csvText)
        {
            //string[] lines = csvText.Split("\n"[0]);
            // TODO: should RemoveEmptyEntries
            string[] lines = csvText.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);

            // finds the max width of row
            int width = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] row = SplitCsvLine(lines[i]);
                width = System.Math.Max(width, row.Length);
            }

            // creates new 2D string grid to output to
            // 这版暂时改成这样, lines.Length 代表有多少个对象， width 代表对象上有多少属性
            //string[,] outputGrid = new string[width, lines.Length];
            string[,] outputGrid = new string[lines.Length, width];
            for (int y = 0; y < lines.Length; y++)
            {
                string[] row = SplitCsvLine(lines[y]);
                for (int x = 0; x < row.Length; x++)
                {
                    outputGrid[y, x] = row[x];

                    // This line was to replace "" with " in my output. 
                    // Include or edit it as you wish.
                    outputGrid[y, x] = outputGrid[y, x].Replace("\"\"", "\"");
                }
            }

            return outputGrid;
        }

        // splits a CSV row 
        static string[] SplitCsvLine(string line)
        {
            return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
            @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
            System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                    select m.Groups[1].Value).ToArray();
        }
    }
}