using System;
using DevExpress.Xpo;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DataProcess.Controls
{
    public class WriteCSV
    {
        public static StringBuilder ReadTxT(string filePath)
        {
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
            StringBuilder stringBuilder = new StringBuilder();
            string strLine;
            while ((strLine = sr.ReadLine()) != null)
            {
                stringBuilder.Append(strLine);
                stringBuilder.Append(",");
            }

            //Console.WriteLine(stringBuilder.ToString());
            sr?.Close();
            return stringBuilder;

        }
    }


}