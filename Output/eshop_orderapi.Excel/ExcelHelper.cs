using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using eshop_orderapi.Excel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace eshop_orderapi.Excel
{
    public static class ExcelHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static List<CustomerModel> GetImportFromFile(string filePath)
        {
            var response = new List<CustomerModel>();
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                StringBuilder excelResult = new StringBuilder();

                foreach (Sheet thesheet in thesheetcollection)
                {
                    string sheetName = thesheet.Name;
                    if (!sheetName.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                    SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                    int rowCounter = 0;
                    foreach (Row thecurrentrow in thesheetdata)
                    {
                        rowCounter++;
                        if (rowCounter == 1)
                        {
                            continue;
                        }
                        var model = new CustomerModel();
                        foreach (Cell thecurrentcell in thecurrentrow)
                        {
                            string currentcellvalue = string.Empty;

                            int columnIndex = ColumnIndex(thecurrentcell.CellReference) - 1;
                            if (thecurrentcell.DataType != null)
                            {
                                int id;
                                if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                {
                                    SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                    SetModelValue(model, columnIndex, item.InnerText);
                                }
                            }
                            else
                            {
                                SetModelValue(model, columnIndex, thecurrentcell.InnerText);
                            }
                            columnIndex++;
                        }
                        response.Add(model);
                    }
                }
            }
            return response;
        }

        private static void SetModelValue(CustomerModel model, int columnIndex, string innerText)
        {
            switch (columnIndex)
            {
                case 0:
                    model.Name = innerText;
                    break;
            }
        }

        private static int ColumnIndex(string reference)
        {
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int)reference[ix] - 64);
            return ci;
        }

        public static DataSet GetDataTabletFromExcelFile(string filePath)
        {
            var file = new FileInfo(filePath);
            DataSet dtExcel;
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                string sheetName = reader.Name;

                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                dtExcel = dataSet;
            }
            return dtExcel;
        }
    }
}