using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;

namespace VerticalDominance
{
    public class ExcelWriter
    {

        public bool WriteTestToSheet(string absolutePath, SpatialTest test)
        {
            string filepath = Path.Combine(absolutePath, "TestResults.xlsx");
            if (!File.Exists(filepath))
            {
                CreateSpreadsheetWorkbook(filepath);
            }

            SpreadsheetDocument spreadsheetDocument = null;

            // Open workbook
            try
            {
                spreadsheetDocument = SpreadsheetDocument.Open(filepath, true);

            } 
            catch (OpenXmlPackageException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Get sheets in workbook
            WorksheetPart newWorksheetPart = spreadsheetDocument.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = spreadsheetDocument.WorkbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new worksheet.
            uint sheetId = 1;

            if (sheets is not null && sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId =
                    sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            // Give the new worksheet a name.
            string sheetName = $"Participant {test.ParticipantID}";

            foreach (Sheet s in sheets)
            {
                if (s is not null && s.Name is not null && s.Name.Equals(sheetName))
                {
                    sheetName = $"Participant {test.ParticipantID}_{Guid.NewGuid().ToString("N")}";
                }

            }


            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);

            // Setup Headers
            Cell hdrTrialNum = InsertCellInWorksheet("A", 1, newWorksheetPart);
            Cell hdrOrientation = InsertCellInWorksheet("B", 1, newWorksheetPart);
            Cell hdrStimPair = InsertCellInWorksheet("C", 1, newWorksheetPart);
            Cell hdrResponseKey = InsertCellInWorksheet("D", 1, newWorksheetPart);
            Cell hdrResponseTime = InsertCellInWorksheet("E", 1, newWorksheetPart);
            Cell hdrAccuracy = InsertCellInWorksheet("F", 1, newWorksheetPart);

            Cell hdrPartId = InsertCellInWorksheet("H", 1, newWorksheetPart);
            Cell hdrDateTime = InsertCellInWorksheet("H", 2, newWorksheetPart);
            Cell hdrAccuracies = InsertCellInWorksheet("I", 4, newWorksheetPart);
            Cell hdrHorizontalSum = InsertCellInWorksheet("H", 5, newWorksheetPart);
            Cell hdrVerticalSum = InsertCellInWorksheet("H", 6, newWorksheetPart);
            Cell hdrAccuracyTotal = InsertCellInWorksheet("H", 7, newWorksheetPart);

            hdrTrialNum.CellValue = new CellValue("Trial");
            hdrOrientation.CellValue = new CellValue("Orientation");
            hdrStimPair.CellValue = new CellValue("Stim Pair");
            hdrResponseKey.CellValue = new CellValue("Response Key");
            hdrResponseTime.CellValue = new CellValue("Response Time");
            hdrAccuracy.CellValue = new CellValue("Accuracy");

            hdrPartId.CellValue = new CellValue("Participant Id");
            hdrDateTime.CellValue = new CellValue("DateTime");
            hdrAccuracies.CellValue = new CellValue("Accuracies");
            hdrHorizontalSum.CellValue = new CellValue("Horizontal:");
            hdrVerticalSum.CellValue = new CellValue("Vertical:");
            hdrAccuracyTotal.CellValue = new CellValue("Total:");

            spreadsheetDocument.Save();

            uint rowIndex = 2;

            // Write test data into sheet
            foreach (TrialBlock block in test.TrialBlocks)
            {
                foreach (SpatialTrial trial in block.Trials)
                {
                    System.Diagnostics.Debug.WriteLine($"Writing Trial {trial.TrialID}");
                    Cell cellTrialNum = InsertCellInWorksheet("A", rowIndex, newWorksheetPart);
                    Cell cellOrientation = InsertCellInWorksheet("B", rowIndex, newWorksheetPart);
                    Cell cellStimPair = InsertCellInWorksheet("C", rowIndex, newWorksheetPart);
                    Cell cellResponseKey = InsertCellInWorksheet("D", rowIndex, newWorksheetPart);
                    Cell cellResponseTime = InsertCellInWorksheet("E", rowIndex, newWorksheetPart);
                    Cell cellAccuracy = InsertCellInWorksheet("F", rowIndex, newWorksheetPart);

                    cellTrialNum.CellValue = new CellValue(trial.TrialID.ToString());
                    cellOrientation.CellValue = new CellValue(trial.Orientation.ToString());
                    cellStimPair.CellValue = new CellValue(trial.TrialTargets.ToString());
                    cellResponseKey.CellValue = new CellValue(trial.ResponseKey.ToString());
                    cellResponseTime.CellValue = new CellValue(trial.ResponseTime.ToString());
                    cellAccuracy.CellValue = new CellValue(trial.Accuracy.ToString());

                    rowIndex += 1;
                }
            }


            spreadsheetDocument.Save();
            spreadsheetDocument.Close();

            return true;
        }


        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }


        private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart, string participantID)
        {
            // Add a new worksheet part to the workbook.
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            string sheetName = $"Participant {participantID}";

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }


        private static void CreateSpreadsheetWorkbook(string filepath)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Main" };
            sheets.Append(sheet);

            workbookpart.Workbook.Save();

            // Close the document.
            spreadsheetDocument.Close();

        }
    }
}
