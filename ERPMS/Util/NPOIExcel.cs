using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ERPMS.Util
{
	public class NPOIExcel
	{
        public static MemoryStream ExportExcel(DataTable tb)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow header = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();

            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
           
            //表头
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                cell.SetCellValue(tb.Columns[i].ColumnName);
                //sheet.SetColumnWidth(i, 20 * 256);
                cell.CellStyle = style;
            }
            //数据  
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < tb.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(tb.Rows[i][j].ToString());
                    cell.CellStyle = style;
                }
            }
            //列宽自适应，只对英文和数字有效
            for (int i = 0; i <= tb.Rows.Count; i++)
                sheet.AutoSizeColumn(i);
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= tb.Columns.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    //当前行未被使用过
                var currentRow = sheet.GetRow(rowNum) ?? sheet.CreateRow(rowNum);
                    if (currentRow.GetCell(columnNum) != null)
                {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                            columnWidth = length;
                }
                }
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
            custmMemoryStream cm = new custmMemoryStream();
            cm.isClose = false;
            workbook.Write(cm);
            cm.Flush();
            cm.Position = 0;
            cm.isClose = false;
            return cm;
        }
    }
    public class custmMemoryStream : MemoryStream
    {
        public bool isClose { get; set; }
        public custmMemoryStream()
        {
            isClose = true;
        }
        public override void Close()
        {
            if (isClose)
            {
                base.Close();
            }
        }
    }

    public static class TestToTable
    {

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist, CreateRowDelegate<T> fn)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            // Could add a check to verify that there is an element 0
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return (dtReturn);
        }
        public delegate object[] CreateRowDelegate<T>(T t);
    }
}