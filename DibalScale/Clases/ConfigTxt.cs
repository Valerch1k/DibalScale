using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DibalScale.Clases
{
    /// <summary>
    ///  Инициализирует свойства из config.txt
    /// </summary>
    class ConfigTxt
    {

        public string Server { get; set; } // имя сервера 

        public string SqlServer { get; set; } // имя экземпляра sql сервера

        public string DateBase { get; set; } // имя базы данных

        public int NumberProgramImport { get; set; }   // номер программы , по номеру идет привязка ip адрессов в таблице ScalesDibal

        public ConfigTxt()
        {
            try
            {
                string[,] strArr = ReadParam();
                if (strArr.Length == 0)
                {
                    Log.Write("Ошибка в файле конфигураций config.txt");
                    Application.Exit();
                }
                for (int i = 0; i < strArr.Length / 2; i++)
                {
                    switch (strArr[i, 0].ToLower())
                    {
                        case "server":
                            Server = strArr[i, 1];
                            break;
                        case "sqlserver":
                            SqlServer = strArr[i, 1];
                            break;
                        case "datebase":
                            DateBase = strArr[i, 1];
                            break;
                        case "numberprogramimport":
                            NumberProgramImport = Convert.ToInt32(strArr[i, 1]);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Ошибка в файле конфигураций config.txt ");
                throw ex;
            }
        }

        /// <summary>
        /// Метод читает все параметры из файла config.txt и возращает коллекцию.
        /// </summary>
        /// <returns></returns>
        private string[,] ReadParam()
        {
            try
            {
                char[] delimeter = { '=' };
                string str;
                List<string> strList = new List<string>();
                string[,] strArr;
                int k = 0;
                StreamReader streamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\config.txt");
                while (!streamReader.EndOfStream)
                {
                    str = streamReader.ReadLine();
                    if (!str.StartsWith("//"))
                    {
                        strList.AddRange(str.Split(delimeter));
                    }
                }
                strArr = new string[strList.Count / 2, 2];
                for (int j = 0; j < 2; j++)
                {
                    if (j == 1)
                    { k = 1; }

                    for (int i = 0; i < strList.Count / 2; i++)
                    {
                        strArr[i, j] = strList[k];
                        k += 2;
                    }
                }
                return strArr;

            }
            catch (Exception ex)
            {
                Log.Write(ex, "Ошибка в файле конфигураций config.txt ");
                return new string[0, 0];
            }
        }
    }
}
