using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;

namespace ConvertingFontApp.util
{
    public static class ExcelCsvUtil
    {
        public static DataTable ReadToDataTable(string filePath)
        {
            var dt = new DataTable();
            var ext = Path.GetExtension(filePath)?.ToLowerInvariant();

            if (ext == ".csv")
            {
                // Auto-detect delimiter (, or ;)
                var firstLine = "";
                using (var reader = new StreamReader(filePath))
                {
                    firstLine = reader.ReadLine() ?? "";
                }
                var delimiter = firstLine.Contains(";") && !firstLine.Contains(",") ? ";" : ",";

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    BadDataFound = null
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    using (var dr = new CsvDataReader(csv))
                    {
                        dt.Load(dr);
                    }
                }
            }
            else if (ext == ".xlsx" || ext == ".xls")
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    var range = worksheet.RangeUsed();
                    if (range != null)
                    {
                        var firstRow = range.FirstRow();
                        foreach (var cell in firstRow.Cells())
                        {
                            var colName = cell.GetString().Trim();
                            if (string.IsNullOrEmpty(colName))
                                colName = $"Column{cell.Address.ColumnNumber}";
                            
                            // Ensure unique column name
                            string uniqueName = colName;
                            int counter = 1;
                            while (dt.Columns.Contains(uniqueName))
                            {
                                uniqueName = $"{colName}_{counter++}";
                            }
                            dt.Columns.Add(uniqueName);
                        }

                        foreach (var row in range.RowsUsed().Skip(1))
                        {
                            var dr = dt.NewRow();
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                dr[i] = row.Cell(i + 1).GetString();
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            else
            {
                throw new NotSupportedException($"Format file {ext} tidak didukung untuk batch mapping merger.");
            }

            return dt;
        }
    }
}
