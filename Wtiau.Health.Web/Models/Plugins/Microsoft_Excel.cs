using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using _Excel = Microsoft.Office.Interop.Excel;

namespace Wtiau.Health.Web.Models.Plugins
{
    public class Microsoft_Excel
    {
        private _Application excel = new _Excel.Application();
        private string Path;
        private Workbook wb;
        private Worksheet ws;

        public Microsoft_Excel(string path)
        {
            this.Path = path;

        }

        public void Open(int sheet)
        {
            wb = excel.Workbooks.Open(Path);
            ws = wb.Worksheets[sheet];
        }

        public void Close()
        {
            wb.Close();
        }


        public void Save()
        {
            wb.Save();
        }

        public void SaveAs(string _path)
        {
            wb.SaveAs(_path);
        }

        public void WriteToCell(int i ,int j,string text)
        {
            i++;
            j++;
            ws.Cells[i, j].Value2 = text;
        }

        public int Get_RowCount()
        {
            var cel = ws.UsedRange;
            Object[,] cel1 = (Object[,])cel.Value;
            return cel1.GetLength(0);
        }
        public int Get_CulCount()
        {
            var cel = ws.UsedRange;
            Object[,] cel1 = (Object[,])cel.Value;
            return cel1.GetLength(1);
        }

        public List<string> Get_Cul(int ID)
        {
            var cel = ws.UsedRange;
            Object[,] cel1 = (Object[,])cel.Value;
            var q = cel1[0, 1];
            return null;
        }

        public List<string> Get_Row(int ID)
        {
            //var cel = ws.UsedRange;
            //Object[,] cel1 = (Object[,])cel.Value;
            return null;
        }

        public string Get_XY(int x, int y)
        {
            return null;
        }

        public System.Data.DataTable Get_DataTable(int x, int y)
        {
            return null;
        }

        public string[,] Get_Range(int SI, int SY, int EI, int EY)
        {
            Range range = (Range)ws.Range[ws.Cells[SI, SY], ws.Cells[EI, EY]];
            Object[,] Holder = range.Value2;
            string[,] SA = new string[EI - SI + 1, EY - SY + 1];
            for (int i = 1; i <= EI - SI + 1; i++)
            {
                for (int j = 1; j <= EY - SY + 1; j++)
                {
                    SA[i - 1, j - 1] = Holder[i, j].ToString();
                }
            }
            return SA;
        }


    }
}