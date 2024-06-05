using CI.TMS.Claim.API.DTOs.Request;
using myTree.MicroService.Helper;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Serilog;
using System.IO;

namespace CI.TMS.Claim.API.Helper.Reports
{
    public class ExportToExcel
    {
        public static byte[] TECReport(ReportTECRequestDTO request)
        {
            try
            {
                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    decimal? total = 0;
                    // Add a new worksheet to the Excel package
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    // Add data to the worksheet
                    int row = 1;
                    #region From

                    worksheet.Cells["A" + row + ":N" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = "Traveler: " + request.Traveler;
                    row++;

                    worksheet.Cells["A" + row + ":N" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = "Destination: " + request.Destination;
                    row++;

                    worksheet.Cells["A" + row + ":N" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = "Date: " + request.Date;
                    row++;

                    worksheet.Cells["A" + row + ":N" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = request.SystemCode;
                    row++;

                    #endregion

                    #region Table
                    //set Header
                    row++;
                    worksheet.Cells["A" + row].Value = "CIFOR-ICRAF";
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Style.Font.Size = 14;
                    worksheet.Cells["A" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A" + row + ":N" + row].Merge = true;
                    row++;

                    worksheet.Cells["A" + row].Value = "Journal printing template";
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Style.Font.Size = 14;
                    worksheet.Cells["A" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A" + row + ":N" + row].Merge = true;

                    row++;

                    int startRow = row;
                    worksheet.Cells["A" + row].Value = "Account";
                    worksheet.Column(1).Width = 15;

                    worksheet.Cells["B" + row].Value = "Text";
                    worksheet.Column(2).Width = 25;

                    worksheet.Cells["C" + row].Value = "Cost center";
                    worksheet.Column(3).Width = 15;

                    worksheet.Cells["D" + row].Value = "Resno";
                    worksheet.Column(4).Width = 10;

                    worksheet.Cells["E" + row].Value = "Workorder";
                    worksheet.Column(5).Width = 15;

                    worksheet.Cells["F" + row].Value = "Legal entity";
                    worksheet.Column(6).Width = 10;

                    worksheet.Cells["G" + row].Value = "Expense type";
                    worksheet.Column(7).Width = 10;

                    worksheet.Cells["H" + row].Value = "Entity";
                    worksheet.Column(8).Width = 15;

                    worksheet.Cells["I" + row].Value = "Tax system";
                    worksheet.Column(9).Width = 10;

                    worksheet.Cells["J" + row].Value = "Currency";
                    worksheet.Column(10).Width = 10;

                    worksheet.Cells["K" + row].Value = "Amount";
                    worksheet.Column(11).Width = 15;

                    worksheet.Cells["L" + row].Value = "Amount (USD)";
                    worksheet.Column(12).Width = 15;

                    worksheet.Cells["M" + row].Value = "APAR ID";
                    worksheet.Column(13).Width = 10;

                    worksheet.Cells["N" + row].Value = "APAR name";
                    worksheet.Column(14).Width = 10;

                    // Set background color for a specific cell
                    worksheet.Cells["A" + row + ":N" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row + ":N" + row].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A" + row + ":N" + row].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    worksheet.Cells["A" + row + ":N" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    row++;

                    //set  data table
                    if (request.ClaimJournal.Count > 0)
                    {
                        foreach (ClaimJournalRequestDTO obj in request.ClaimJournal)
                        {
                            worksheet.Cells["A" + row].Value = obj.Account;
                            worksheet.Cells["B" + row].Value = obj.Text;
                            worksheet.Cells["C" + row].Value = obj.CostCenterId;
                            worksheet.Cells["D" + row].Value = obj.Cat3;
                            worksheet.Cells["E" + row].Value = obj.WorkOrderId;
                            worksheet.Cells["F" + row].Value = obj.Cat5;
                            worksheet.Cells["G" + row].Value = obj.Cat6;
                            worksheet.Cells["H" + row].Value = obj.EntityId;
                            worksheet.Cells["I" + row].Value = obj.TaxSystem;
                            worksheet.Cells["J" + row].Value = obj.Currency;
                            
                            worksheet.Cells["K" + row].Style.Numberformat.Format = "#,##0.00";
                            worksheet.Cells["K" + row].Value = obj.Amount;

                            worksheet.Cells["L" + row].Style.Numberformat.Format = "#,##0.00";
                            worksheet.Cells["L" + row].Value = obj.AmountUsd;
                            worksheet.Cells["M" + row].Value = obj.AparId;
                            worksheet.Cells["N" + row].Value = obj.AparName;

                            total += obj.AmountUsd;
                            row++;
                        }
                    }
                    else
                    {
                        row++;
                    }


                    //set total
                    worksheet.Cells["A" + row + ":K" + row].Merge = true;
                    worksheet.Cells["A" + row].Value = "Total";
                    worksheet.Cells["A" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells["L" + row].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells["L" + row].Value = total;
                    worksheet.Cells["M" + row + ":N" + row].Merge = true;

                    worksheet.Cells["A" + row + ":N" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row + ":N" + row].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A" + row + ":N" + row].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);


                    int endRow = row;
                    // Get the range of cells containing the table
                    ExcelRange tableRange = worksheet.Cells["A" + startRow + ":N" + endRow];

                    // Apply border around the table range
                    tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    worksheet.Cells["A" + startRow + ":N" + endRow].Style.WrapText = true; // Enable text wrapping for the cell
                    worksheet.Cells["A" + startRow + ":N" + endRow].Style.VerticalAlignment = ExcelVerticalAlignment.Top; // Align text at the top


                    row++;
                    #endregion

                    #region Transaction
                    row++;

                    //worksheet.Cells["A" + row + ":H" + row].Merge = true;
                    //worksheet.Cells["A" + row].Style.Font.Bold = true;
                    //worksheet.Cells["A" + row].Value = "Transaction No: " + request.TransactionNo;
                    //row++;

                    worksheet.Cells["A" + row + ":H" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = "Period: " + request.Period;
                    row++;

                    worksheet.Cells["A" + row + ":H" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = "Trans.Date: " + request.TransactionDate;
                    row++;

                    worksheet.Cells["A" + row + ":H" + row].Merge = true;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Value = "Transaction type: " + request.TransactionType;
                    row++;

                    if (!string.IsNullOrEmpty(request.JournalNo))
                    {
                        worksheet.Cells["A" + row + ":H" + row].Merge = true;
                        worksheet.Cells["A" + row].Style.Font.Bold = true;
                        worksheet.Cells["A" + row].Value = "Journal no: " + request.JournalNo;
                        row++;
                    }                   

                    #endregion

                    return package.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Detail());
                throw;
            }

        }
    }
}
