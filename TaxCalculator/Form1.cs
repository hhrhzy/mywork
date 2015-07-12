using System;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;



namespace TaxCalculator
{
    public partial class Form1 : Form
    {
        //东莞
        double dg_base=3572.67;
        //深圳
        double sz_base=5218.34;
        //
        double dg_range_min = 0;
        double dg_range_max = 0;
        double sz_range_min = 0;
        double sz_range_max = 0;
        double JiShuFangWei_min = 0;
        double JiShuFangWei_max = 0;
        public static Form1 f1 = null;
        public static System.Data.DataTable dtname = new System.Data.DataTable();
        public static int No_button = 0;
        public Form1()
        {
            InitializeComponent();
            f1 = this;
            textBox_GeRenBiLi.Enabled = true;
            textBox_GeRenBiLi.Text = "5";
            dg_range_min = (int)(dg_base * 3);
            dg_range_max = (int)(dg_base * 5);
            sz_range_min = (int)(sz_base * 3);
            sz_range_max = (int)(sz_base * 5);
            label2.Text = "[现软件设定‘东莞’免税公积金基数：3倍为" + dg_range_min + "，5倍为" + dg_range_max + "]";
            label_GJJresult.Text = "东莞免税公积金：";
            JiShuFangWei_min = dg_range_min;
            JiShuFangWei_max = dg_range_max;
            
            
        }


        public bool DoExport(System.Data.DataTable dt)
        {
            Microsoft.Office.Interop.Excel.Application app = new ApplicationClass();
            if (app == null)
            {
                throw new Exception("Excel无法启动");
            }
            app.Visible = true;
            Workbooks wbs = app.Workbooks;
            Workbook wb = wbs.Add(Missing.Value);
            Worksheet ws = (Worksheet)wb.Worksheets[1];
            //ws.Name = dtname.Rows[0][0].ToString();
            int cnt = dt.Rows.Count;
            int columncnt = dt.Columns.Count;

            object[,] objData;
            int RowIndex = 0;
            Range r;
            while (RowIndex < cnt)
            {
                if (cnt + 1 - RowIndex > 10000)
                {
                    objData=new object[10000+1,columncnt];
                    for (int i = 0; i < 10000; i++)
                    {
                        System.Data.DataRow dr = dt.Rows[RowIndex + i];
                        for (int j = 0; j < columncnt; j++)
                        {
                            objData[i, j] = dr[j];
                        }
                    }
                    r = ws.get_Range(app.Cells[RowIndex + 1, 1], app.Cells[RowIndex + 10001, columncnt]);
                    r.NumberFormat = "@";
                    r.Value2 = objData;
                    RowIndex += 10000;
                }
                else
                {
                    objData = new object[cnt + 1 - RowIndex, columncnt];
                    for (int i = 0; i < cnt - RowIndex; i++)
                    {
                        System.Data.DataRow dr = dt.Rows[i + RowIndex];
                        for (int j = 0; j < columncnt; j++)
                        {
                            objData[i, j] = dr[j];
                        }
                    }
                    r = ws.get_Range(app.Cells[RowIndex + 1, 1], app.Cells[cnt + 1, columncnt]);
                    r.NumberFormat = "@";
                    r.Value2 = objData;
                    RowIndex += cnt + 1 - RowIndex;
                }
                //r.NumberFormat = "@";
                //r = r.get_Resize(cnt+1, columncnt);
                //r.Value2 = objData;
               // r.EntireColumn.AutoFit();
            }

            /*
            // *****************获取数据********************
            object[,] objData = new Object[cnt +1, columncnt];  // 创建缓存数据
            ////获取列标题
            //for (int i = 0; i < columncnt; i++)
            //{
            //    objData[0, i] = dt.Columns[i].ColumnName;
            //}
            //获取具体数据
            for (int i = 0; i < cnt; i++)
            {
                System.Data.DataRow dr = dt.Rows[i];
                for (int j = 0; j < columncnt; j++)
                {
                    objData[i, j] = dr[j];
                }
            }
            //********************* 写入Excel******************
            Range r = ws.get_Range(app.Cells[1, 1], app.Cells[cnt + 1, columncnt]);
            r.NumberFormat = "@";
            //r = r.get_Resize(cnt+1, columncnt);
            r.Value2 = objData;
            r.EntireColumn.AutoFit();
            */
            app = null;
            return true;
        }

        public class ExcelHelper
        {
            public class x2003
            {
                #region Excel2003
                /// <summary>
                /// 将Excel文件中的数据读出到DataTable中(xls)
                /// </summary>
                /// <param name="file"></param>
                /// <returns></returns>
                public static System.Data.DataTable ExcelToTableForXLS(string file)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                        int x = hssfworkbook.Workbook.NumSheets;
                        dtname.Columns.Add("", typeof(string));
                        for (int i = 0; i < x; i++)
                        {
                            dtname.Rows.Add(hssfworkbook.Workbook.GetSheetName(i));
                            //dt.Rows[i][0]=(hssfworkbook.Workbook.GetSheetName(i));

                        }
                        
                        import frm = new import();
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            dtname.Rows.Clear();
                            dtname.Rows.Add(frm.selectedT);
                        }
                        else
                        {
                            return null;
                        }
                        ISheet sheet = hssfworkbook.GetSheet(dtname.Rows[0][0].ToString());
                        //表头
                        IRow header = sheet.GetRow(sheet.FirstRowNum);
                        List<int> columns = new List<int>();
                        for (int i = 0; i < header.LastCellNum; i++)
                        {
                            object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                            if (obj == null || obj.ToString() == string.Empty)
                            {
                                dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                                //continue;
                            }
                            else
                                dt.Columns.Add(new DataColumn(obj.ToString()));
                            columns.Add(i);
                        }
                        //数据
                        for (int i = sheet.FirstRowNum ; i <= sheet.LastRowNum; i++)
                        {
                            DataRow dr = dt.NewRow();
                            bool hasValue = false;
                            foreach (int j in columns)
                            {
                                dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                                if (dr[j] != null && dr[j].ToString() != string.Empty)
                                {
                                    hasValue = true;
                                }
                            }
                            if (hasValue)
                            {
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    return dt;
                }

                /// <summary>
                /// 将DataTable数据导出到Excel文件中(xls)
                /// </summary>
                /// <param name="dt"></param>
                /// <param name="file"></param>
                public static void TableToExcelForXLS(System.Data.DataTable dt, string file)
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                    ISheet sheet = hssfworkbook.CreateSheet("Test");

                    //表头
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }

                    //数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row1.CreateCell(j);
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    hssfworkbook.Write(stream);
                    var buf = stream.ToArray();

                    //保存为Excel文件
                    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                    }
                }

                /// <summary>
                /// 获取单元格类型(xls)
                /// </summary>
                /// <param name="cell"></param>
                /// <returns></returns>
                private static object GetValueTypeForXLS(HSSFCell cell)
                {
                    if (cell == null)
                        return null;
                    switch (cell.CellType)
                    {
                        case CellType.Blank: //BLANK:
                            return null;
                        case CellType.Boolean: //BOOLEAN:
                            return cell.BooleanCellValue;
                        case CellType.Numeric: //NUMERIC:
                            return cell.NumericCellValue;
                        case CellType.String: //STRING:
                            return cell.StringCellValue;
                        case CellType.Error: //ERROR:
                            return cell.ErrorCellValue;
                        case CellType.Formula: //FORMULA:
                        default:
                            return "=" + cell.CellFormula;
                    }
                }
                #endregion
            }

            public class x2007
            {
                #region Excel2007
                /// <summary>
                /// 将Excel文件中的数据读出到DataTable中(xlsx)
                /// </summary>
                /// <param name="file"></param>
                /// <returns></returns>
                public static System.Data.DataTable ExcelToTableForXLSX(string file)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                        int x = xssfworkbook.NumberOfSheets;
                        dtname.Columns.Add("", typeof(string));
                        for (int i = 0; i < x; i++)
                        {
                            dtname.Rows.Add(xssfworkbook.GetSheetName(i));
                            //dt.Rows[i][0]=(hssfworkbook.Workbook.GetSheetName(i));

                        }
                        
                        import frm = new import();
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            dtname.Rows.Clear();
                            dtname.Rows.Add(frm.selectedT);
                        }
                        else
                        {
                            return null;
                        }
                        ISheet sheet = xssfworkbook.GetSheet(dtname.Rows[0][0].ToString());
                        //表头
                        IRow header = sheet.GetRow(sheet.FirstRowNum);
                        List<int> columns = new List<int>();
                        for (int i = 0; i < header.LastCellNum; i++)
                        {
                            object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                            if (obj == null || obj.ToString() == string.Empty)
                            {
                                dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                                //continue;
                            }
                            else
                                dt.Columns.Add(new DataColumn(obj.ToString()));
                            columns.Add(i);
                        }
                        //数据
                        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                        {
                            DataRow dr = dt.NewRow();
                            bool hasValue = false;
                            foreach (int j in columns)
                            {
                                dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                                if (dr[j] != null && dr[j].ToString() != string.Empty)
                                {
                                    hasValue = true;
                                }
                            }
                            if (hasValue)
                            {
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    return dt;
                }

                /// <summary>
                /// 将DataTable数据导出到Excel文件中(xlsx)
                /// </summary>
                /// <param name="dt"></param>
                /// <param name="file"></param>
                public static void TableToExcelForXLSX(System.Data.DataTable dt, string file)
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                    ISheet sheet = xssfworkbook.CreateSheet("Test");

                    //表头
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }

                    //数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row1.CreateCell(j);
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    xssfworkbook.Write(stream);
                    var buf = stream.ToArray();

                    //保存为Excel文件
                    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                    }
                }

                /// <summary>
                /// 获取单元格类型(xlsx)
                /// </summary>
                /// <param name="cell"></param>
                /// <returns></returns>
                private static object GetValueTypeForXLSX(XSSFCell cell)
                {
                    if (cell == null)
                        return null;
                    switch (cell.CellType)
                    {
                        case CellType.Blank: //BLANK:
                            return null;
                        case CellType.Boolean: //BOOLEAN:
                            return cell.BooleanCellValue;
                        case CellType.Numeric: //NUMERIC:
                            return cell.NumericCellValue;
                        case CellType.String: //STRING:
                            return cell.StringCellValue;
                        case CellType.Error: //ERROR:
                            return cell.ErrorCellValue;
                        case CellType.Formula: //FORMULA:
                        default:
                            return "=" + cell.CellFormula;
                    }
                }
                #endregion
            }

            public static System.Data.DataTable GetDataTable()
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Excel03-07文件(*.xls);Excel07-13文件(*.xlsx)|*.xls;*.xlsx";
                openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFile.Multiselect = false;
                if (openFile.ShowDialog() == DialogResult.Cancel) return null;
                var filepath = openFile.FileName;
                string fileType = System.IO.Path.GetExtension(filepath);
                if (string.IsNullOrEmpty(fileType)) return null;
                var dt = new System.Data.DataTable("xls");
                if (filepath.EndsWith("s"))
                {
                    dt = x2003.ExcelToTableForXLS(filepath);
                }
                else
                {
                    dt = x2007.ExcelToTableForXLSX(filepath);
                }
                return dt;
            }
        }

        private void button_MianShuiGJJ_Click(object sender, EventArgs e)
        {
            if (textBox_jishu.Text == ""||textBox_GeRenBiLi.Text=="")
            {
                MessageBox.Show("请确认所有输入框都有填写！`(*∩_∩*)′", "提示");
            }
            else
            {
                double jishu = double.Parse(textBox_jishu.Text);
                //int bili = int.Parse(textBox_GeRenBiLi.Text);
                //bili = bili / 100;
                if (jishu <= JiShuFangWei_min)
                {
                    resultBox_MianShui.Text = (jishu * double.Parse(textBox_GeRenBiLi.Text) / 100).ToString();
                }
                else if (jishu > JiShuFangWei_min && jishu < JiShuFangWei_max)
                {
                    double tem1 = jishu * double.Parse(textBox_GeRenBiLi.Text) / 100;
                    double tem2 = double.Parse(textBox_GeRenBiLi.Text) / 100 + 0.05;
                    tem2 = (jishu - JiShuFangWei_min) * tem2;
                    resultBox_MianShui.Text = (tem1 - tem2).ToString();
                }
                else
                {
                    double tem1 = JiShuFangWei_max * double.Parse(textBox_GeRenBiLi.Text) / 100;
                    double tem2 = double.Parse(textBox_GeRenBiLi.Text) / 100 + 0.05;
                    tem2 = (JiShuFangWei_max - JiShuFangWei_min) * tem2;
                    resultBox_MianShui.Text = (tem1 - tem2).ToString();
                }
            }
           
        }

        private void radioButton_dg_CheckedChanged(object sender, EventArgs e)
        {
            textBox_GeRenBiLi.Enabled = true;
            JiShuFangWei_min = dg_range_min;
            JiShuFangWei_max = dg_range_max;
            label2.Text = "[现软件设定‘东莞’免税公积金基数：3倍为" + dg_range_min + "，5倍为" + dg_range_max + "]";
            label_GJJresult.Text = "东莞免税公积金：";
        }

        private void radioButton_sz_CheckedChanged(object sender, EventArgs e)
        {
            textBox_GeRenBiLi.Enabled = false;
            textBox_GeRenBiLi.Text = "5";
            JiShuFangWei_min = sz_range_min;
            JiShuFangWei_max = sz_range_max;
            label2.Text = "[现软件设定‘深圳’免税公积金基数：3倍为" + sz_range_min + "，5倍为" + sz_range_max + "]";
            label_GJJresult.Text = "深圳免税公积金：";
        }

        

        private void resultBox_MianShui_MouseClick(object sender, MouseEventArgs e)
        {
            resultBox_MianShui.SelectAll();
        }

       
        private void resultBox_GeRenSuoDe_MouseClick(object sender, MouseEventArgs e)
        {
            resultBox_GeRenSuoDe.SelectAll();
        }

        private void textBox_jishu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)46 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void textBox_GeRenBiLi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)46 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void textBox_YingNaShui_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)46 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)46 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)46 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)46 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        



        private void button_msImp_Click(object sender, EventArgs e)
        {
            No_button = 1;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            label_statu.Text = "正在进行导入→处理→导出数据操作，请稍后...";
            System.Data.DataTable dt = new System.Data.DataTable();
            dtname.Clear();
            dt = ExcelHelper.GetDataTable();
            if (dt != null) 
            {
                dt.Columns.Add();
                if(dt.Rows[0][2].ToString()!="地区")
                {
                    MessageBox.Show("导入表的C列不是“地区”列，请按照选表界面图片所示格式将C列名字和内容改成“地区”列。","提示");
                    erroEX();
                    return;
                }
                else if (dt.Rows[0][1].ToString() != "缴费基数")
                {
                    MessageBox.Show("导入表的B列不是“缴费基数”列，请按照选表界面图片所示格式将B列名字和内容改成“缴费基数”列。", "提示");
                    erroEX();
                    return;
                }
                else if (dt.Rows[0][3].ToString() != "个人比例")
                {
                    MessageBox.Show("导入表的D列不是“个人比例”列，请按照选表界面图片所示格式将D列名字和内容改成“个人比例”列。", "提示");
                    erroEX();
                    return;
                }
                else if (dt.Rows[0][5].ToString() != "免税公积金")
                {
                    MessageBox.Show("导入表的F列不是“免税公积金”列，请按照选表界面图片所示格式将F列名字和内容改成“免税公积金”列。\n此列数据用于存放计算后数据，原有数据将会被覆盖。", "提示");
                    erroEX();
                    return;
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        if (dt.Rows[i][2].ToString() == "东莞")
                        {
                            JiShuFangWei_min = dg_range_min;
                            JiShuFangWei_max = dg_range_max;
                            double jishu = double.Parse(dt.Rows[i][1].ToString());
                            double bili = double.Parse(dt.Rows[i][3].ToString());
                            //bili = bili / 100;
                            if (jishu <= JiShuFangWei_min)
                            {
                                dt.Rows[i][5] = Math.Round((jishu * bili), 2).ToString();
                            }
                            else if (jishu > JiShuFangWei_min && jishu < JiShuFangWei_max)
                            {
                                double tem1 = jishu * bili;
                                double tem2 = bili + 0.05;
                                tem2 = (jishu - JiShuFangWei_min) * tem2;
                                dt.Rows[i][5] = Math.Round((tem1 - tem2), 2).ToString();
                            }
                            else
                            {
                                double tem1 = JiShuFangWei_max * bili;
                                double tem2 = bili + 0.05;
                                tem2 = (JiShuFangWei_max - JiShuFangWei_min) * tem2;
                                dt.Rows[i][5] = Math.Round((tem1 - tem2), 2).ToString();
                            }
                        }
                        else if (dt.Rows[i][2].ToString() == "深圳")
                        {
                            JiShuFangWei_min = sz_range_min;
                            JiShuFangWei_max = sz_range_max;
                            double jishu = double.Parse(dt.Rows[i][1].ToString());
                            double bili = double.Parse(dt.Rows[i][3].ToString());
                            //bili = bili / 100;
                            if (jishu <= JiShuFangWei_min)
                            {
                                dt.Rows[i][5] = Math.Round((jishu * bili), 2).ToString();
                            }
                            else if (jishu > JiShuFangWei_min && jishu < JiShuFangWei_max)
                            {
                                double tem1 = jishu * bili;
                                double tem2 = bili + 0.05;
                                tem2 = (jishu - JiShuFangWei_min) * tem2;
                                dt.Rows[i][5] = Math.Round((tem1 - tem2), 2).ToString();
                            }
                            else
                            {
                                double tem1 = JiShuFangWei_max * bili;
                                double tem2 = bili + 0.05;
                                tem2 = (JiShuFangWei_max - JiShuFangWei_min) * tem2;
                                dt.Rows[i][5] = Math.Round((tem1 - tem2), 2).ToString();
                            }
                        }
                        else
                        {
                            dt.Rows[i][6] = "此行数据错误，不做处理。";
                        }
                    }
                    catch
                    {
                        MessageBox.Show("在读取第"+ (i+1) + "行时出现问题，请检查数据格式。","提示");
                    }
                    
                }
                DoExport(dt);
            }
            erroEX();
        }

        private void erroEX()
        {
            textBox_GeRenBiLi.Enabled = true;
            label2.Text = "[现软件设定‘东莞’免税公积金基数：3倍为" + dg_range_min + "，5倍为" + dg_range_max + "]";
            label_GJJresult.Text = "东莞免税公积金：";
            JiShuFangWei_min = dg_range_min;
            JiShuFangWei_max = dg_range_max;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            label_statu.Text = "";
        }

        private void button_YingNaShui_Click(object sender, EventArgs e)
        {
            
            if (textBox_Yingfa.Text == ""||textBox_shebao.Text==""||textBox_mgjj.Text=="")
            {
                MessageBox.Show("请确认所有输入框都有填写！`(*∩_∩*)′", "提示");
            }
            else
            {
                double yingfa = double.Parse(textBox_Yingfa.Text);
                double shebao = double.Parse(textBox_shebao.Text);
                double mgjj = double.Parse(textBox_mgjj.Text);
                string temp = "中国";
                if(radioButton_forg.Checked==true)
                {
                    temp = "外籍";
                }
                resultBox_GeRenSuoDe.Text = doCalcu_gjjtax(yingfa,shebao, mgjj,temp).ToString();
            }
        }

        public double doCalcu_gjjtax(double yingfa,double shebao,double mgjj,string region)
        {
            double s = yingfa - shebao - mgjj;
            double startTax = 3500;
            double result = 0;
            if(region!="中国")
            {
                startTax = 4800;
            }
            if (s <= startTax)
            {
                result = 0;
            }
            else if (s > startTax && s <= startTax + 1500)
            {
                result = (s - startTax) * 0.03 - 0;
            }
            else if (s > startTax + 1500 && s <= startTax + 4500)
            {
                result = (s - startTax) * 0.1 - 105;
            }
            else if (s > startTax + 4500 && s <= startTax + 9000)
            {
                result = (s - startTax) * 0.2 - 555;
            }
            else if (s > startTax + 9000 && s <= startTax + 35000)
            {
                result = (s - startTax) * 0.25 - 1005;
            }
            else if (s > startTax + 35000 && s <= startTax + 55000)
            {
                result = (s - startTax) * 0.3 - 2755;
            }
            else if (s > startTax + 55000 && s <= startTax + 80000)
            {
                result = (s - startTax) * 0.35 - 5505;
            }
            else if (s > startTax + 80000)
            {
                result = (s - startTax) * 0.45 - 13505;
            }
            return result;
        }

        private void button_imGs_Click(object sender, EventArgs e)
        {
            No_button = 2;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            label_statu.Text = "正在进行导入→处理→导出数据操作，请稍后...";
            System.Data.DataTable dt = new System.Data.DataTable();
            dtname.Clear();
            dt = ExcelHelper.GetDataTable();
            if (dt != null)
            {
                if(dt.Rows[0][32].ToString()!="应发合计")
                {
                    MessageBox.Show("导入表的AG列不是“应发合计”列，请按照选表界面图片所示格式将AG列名字和内容改成“应发合计”列。", "提示");
                }
                else if(dt.Rows[0][37].ToString()!="扣社保")
                {
                    MessageBox.Show("导入表的AL列不是“扣社保”列，请按照选表界面图片所示格式将AL列名字和内容改成“扣社保”列。", "提示");
                }
                else if (dt.Rows[0][43].ToString() != "免税公积金")
                {
                    MessageBox.Show("导入表的AR列不是“免税公积金”列，请按照选表界面图片所示格式将AR列名字和内容改成“免税公积金”列。", "提示");
                }
                else if (dt.Rows[0][66].ToString() != "国籍")
                {
                    MessageBox.Show("导入表的BO列不是“国籍”列，请按照选表界面图片所示格式将BO列名字和内容改成“国籍”列。", "提示");
                }
                else if (dt.Rows[0][44].ToString() != "扣所得税")
                {
                    MessageBox.Show("导入表的AS列不是“扣所得税”列，请按照选表界面图片所示格式将AS列名字和内容改成“扣所得税”列。\n此列数据用于存放计算后数据，原有数据将会被覆盖。", "提示");
                }
                else
                {
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            double yingfa = double.Parse(dt.Rows[i][32].ToString());
                            double shebao = double.Parse(dt.Rows[i][37].ToString());
                            double mgjj = double.Parse(dt.Rows[i][43].ToString());
                            string region = dt.Rows[i][66].ToString();
                            dt.Rows[i][44] = Math.Round(doCalcu_gjjtax(yingfa, shebao, mgjj, region), 2);
                        }
                        catch
                        {
                            MessageBox.Show("在读取第" + (i + 1) + "行时出现问题，请检查数据格式。", "提示");
                        }
                        
                    }

                    DoExport(dt);
                }

              }
            groupBox2.Enabled = true;
            groupBox1.Enabled = true;
            label_statu.Text = "";
            
        }

        private void textBox_jishu_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9.-]*$");
            var str = textBox_jishu.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox_jishu.Text = sb.ToString();
                textBox_jishu.SelectionStart = textBox_jishu.Text.Length; //定义输入焦点在最后一个字符
            }
        }

        private void textBox_GeRenBiLi_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9.-]*$");
            var str = textBox_GeRenBiLi.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox_GeRenBiLi.Text = sb.ToString();
                textBox_GeRenBiLi.SelectionStart = textBox_GeRenBiLi.Text.Length; //定义输入焦点在最后一个字符
            }
        }

        private void textBox_Yingfa_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9.-]*$");
            var str = textBox_Yingfa.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox_Yingfa.Text = sb.ToString();
                textBox_Yingfa.SelectionStart = textBox_Yingfa.Text.Length; //定义输入焦点在最后一个字符
            }
        }

        private void textBox_shebao_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9.-]*$");
            var str = textBox_shebao.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox_shebao.Text = sb.ToString();
                textBox_shebao.SelectionStart = textBox_shebao.Text.Length; //定义输入焦点在最后一个字符
            }
        }

        private void textBox_mgjj_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9.-]*$");
            var str = textBox_mgjj.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox_mgjj.Text = sb.ToString();
                textBox_mgjj.SelectionStart = textBox_mgjj.Text.Length; //定义输入焦点在最后一个字符
            }
        }

        private void textBox_shuihou_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9.-]*$");
            var str = textBox_shuihou.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox_shuihou.Text = sb.ToString();
                textBox_shuihou.SelectionStart = textBox_shuihou.Text.Length; //定义输入焦点在最后一个字符
            }
        }


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    for (int i = 0; i < 68; i++)
        //    {
        //        dt.Columns.Add("com"+i);
        //    }
        //    for (int i = 0; i < 62000; i++)
        //    {
        //        dt.Rows.Add();
        //        for (int j = 0; j < 68;j++ )
        //        {
        //            dt.Rows[i][j] = i + "." + j;
        //        }
                
        //    }
        //    DoExport(dt);
        //}

        private void button_shuiqian_Click(object sender, EventArgs e)
        {
           resultBox_shuiqian.Text = DoCaTaxOld(double.Parse(textBox_shuihou.Text),"").ToString();
        }

        
        private double DoCaTaxOld(double AfterTax,string region)
        {
            double result = 0;
            //if (region != "中国")
            //{
            //    startTax = 4800;
            //}
            if (AfterTax <= 3500)
            {
                result = AfterTax;
            }
            else if (AfterTax > 3500 && AfterTax <= 4955)
            {
                result = (AfterTax-3500*0.03)/0.97;
            }
            else if (AfterTax > 4955 && AfterTax <= 7655)
            {
                result = (45+AfterTax-5000*0.1)/0.9;
            }
            else if (AfterTax > 7655 && AfterTax <= 11255)
            {
                result = (45+300+AfterTax-8000*0.2)/0.8;
            }
            else if (AfterTax > 11255 && AfterTax <= 30755)
            {
                result = (45+300+900+AfterTax-12500*0.25)/0.75;
            }
            else if (AfterTax > 30755 && AfterTax <= 44755)
            {
                result = (45+300+900+6500+AfterTax-38500*0.3)/0.7;
            }
            else if (AfterTax > 44755 && AfterTax <= 61005)
            {
                result = (45+300+900+6500+6000+AfterTax-58500*0.35)/0.65;
            }
            else if (AfterTax > 61005)
            {
                result = (45 + 300 + 900 + 6500 + 6000 + 8750 + AfterTax - 83500 * 0.45)/0.55;
            }
            return result;
        }

    }
}
