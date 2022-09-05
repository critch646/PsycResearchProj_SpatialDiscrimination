using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using log4net;

namespace VerticalDominance
{
    public class ExcelWriter
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ExcelWriter()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Takes passed spatial test and writes its data to an Excel workbook (creating a new one if it doesn't
        /// already exist) by inserting a new worksheet and inserting data to that worksheet.
        /// </summary>
        /// <param name="absolutePath">Path and filename of target Excel workbook.</param>
        /// <param name="test">Spatial test to write-out data.</param>
        /// <returns></returns>
        public bool WriteTestToSheet(string absolutePath, SpatialTest test)
        {
            // Create file path
            string filepath = Path.Combine(absolutePath, "TestResults.xlsx");

            // Check if file exists
            if (!File.Exists(filepath))
            {
                CreateSpreadsheetWorkbook(filepath);
                log.Info($"File exists {filepath}");
            }

            // Create a spreadsheet document
            SpreadsheetDocument spreadsheetDocument = null;

            // Open workbook
            try
            {
                spreadsheetDocument = SpreadsheetDocument.Open(filepath, true);

            } 
            catch (OpenXmlPackageException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                log.Error($"Opening workbook: {ex.Message}");
                return false;
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                log.Error($"Opening workbook: {ex.Message}");
                return false;
            }

            // Insert new worksheet part
            WorksheetPart newWorksheetPart = InsertWorksheet(spreadsheetDocument.WorkbookPart, test.ParticipantID.ToString());

            // Insert labels
            newWorksheetPart = InsertWorksheetLabels(newWorksheetPart);
            spreadsheetDocument.Save();

            // Insert test data
            newWorksheetPart = InsertTestData(newWorksheetPart, test);
            spreadsheetDocument.Save();
            spreadsheetDocument.Close();

            return true;
        }


        /// <summary>
        /// Inserts a new cell based on the passed coumna name and row index into the passed worksheet part.
        /// </summary>
        /// <param name="columnName">Target column name to insert to.</param>
        /// <param name="rowIndex">Target row index to insert to.</param>
        /// <param name="worksheetPart">Target worksheet part to insert to.</param>
        /// <returns></returns>
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

                // Create and insert new cell
                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();

                return newCell;
            }
        }


        /// <summary>
        /// Create and insert a new worksheet into the pased workbook part and naming it using the passed participant ID.
        /// </summary>
        /// <param name="workbookPart">Target workbook part to insert to.</param>
        /// <param name="participantID">Participant's ID number.</param>
        /// <returns></returns>
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

            // Give the new worksheet a name.
            string sheetName = $"Participant {participantID}";

            foreach (Sheet s in sheets)
            {
                if (s is not null && s.Name is not null && s.Name.Equals(sheetName))
                {
                    sheetName = $"Participant {participantID}_{Guid.NewGuid().ToString("N")}";
                }

            }

            // Add datetime style sheet
            try
            {
                if (workbookPart.WorkbookStylesPart == null)
                {
                    WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

                    stylesPart.Stylesheet = new Stylesheet
                    {
                        Fonts = new Fonts(new Font()),
                        Fills = new Fills(new Fill()),
                        Borders = new Borders(new Border()),
                        CellStyleFormats = new CellStyleFormats(new CellFormat()),
                        CellFormats =
                            new CellFormats(
                                new CellFormat(),
                                new CellFormat
                                {
                                    NumberFormatId = 22,
                                    ApplyNumberFormat = true
                                })
                    };
                }

            } 
            catch (OpenXmlPackageException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                log.Error($"Adding datetime style: {ex.Message}");
            }



            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }


        /// <summary>
        /// Inserts labels for test data and information.
        /// </summary>
        /// <param name="newWorksheetPart">Worksheet part to insert labels into.</param>
        /// <returns></returns>
        private static WorksheetPart InsertWorksheetLabels(WorksheetPart newWorksheetPart)
        {
            // Insert label cells for test data
            Cell labelTrialNum = InsertCellInWorksheet("A", 1, newWorksheetPart);
            Cell labelOrientation = InsertCellInWorksheet("B", 1, newWorksheetPart);
            Cell labelStimPair = InsertCellInWorksheet("C", 1, newWorksheetPart);
            Cell LabelResponseKey = InsertCellInWorksheet("D", 1, newWorksheetPart);
            Cell labelResponseTime = InsertCellInWorksheet("E", 1, newWorksheetPart);
            Cell labelAccuracy = InsertCellInWorksheet("F", 1, newWorksheetPart);

            // Insert label cells for test information
            Cell labelPartId = InsertCellInWorksheet("H", 1, newWorksheetPart);
            Cell labelDateTime = InsertCellInWorksheet("H", 2, newWorksheetPart);
            Cell labelAccuracies = InsertCellInWorksheet("I", 3, newWorksheetPart);
            Cell labelHorizontalSum = InsertCellInWorksheet("H", 4, newWorksheetPart);
            Cell labelVerticalSum = InsertCellInWorksheet("H", 5, newWorksheetPart);
            Cell labelAccuracyTotal = InsertCellInWorksheet("H", 6, newWorksheetPart);

            // Assign label cell values for test data
            labelTrialNum.CellValue = new CellValue("Trial");
            labelOrientation.CellValue = new CellValue("Orientation");
            labelStimPair.CellValue = new CellValue("Stim Pair");
            LabelResponseKey.CellValue = new CellValue("Response Key");
            labelResponseTime.CellValue = new CellValue("Response Time");
            labelAccuracy.CellValue = new CellValue("Accuracy");

            // Assign label cell values for test information
            labelPartId.CellValue = new CellValue("Participant Id");
            labelDateTime.CellValue = new CellValue("DateTime");
            labelAccuracies.CellValue = new CellValue("Accuracy");
            labelHorizontalSum.CellValue = new CellValue("Horizontal");
            labelVerticalSum.CellValue = new CellValue("Vertical");
            labelAccuracyTotal.CellValue = new CellValue("Total");
            
            // Set data types for test data cells
            labelTrialNum.DataType = new EnumValue<CellValues>(CellValues.String);
            labelOrientation.DataType = new EnumValue<CellValues>(CellValues.String);
            labelStimPair.DataType = new EnumValue<CellValues>(CellValues.String);
            LabelResponseKey.DataType = new EnumValue<CellValues>(CellValues.String);
            labelResponseTime.DataType = new EnumValue<CellValues>(CellValues.String);
            labelAccuracy.DataType = new EnumValue<CellValues>(CellValues.String);

            // Set data types for test information cells
            labelPartId.DataType = new EnumValue<CellValues>(CellValues.String);
            labelDateTime.DataType = new EnumValue<CellValues>(CellValues.String);
            labelAccuracies.DataType = new EnumValue<CellValues>(CellValues.String);
            labelHorizontalSum.DataType = new EnumValue<CellValues>(CellValues.String);
            labelVerticalSum.DataType = new EnumValue<CellValues>(CellValues.String);
            labelAccuracyTotal.DataType = new EnumValue<CellValues>(CellValues.String);

            return newWorksheetPart;
        }


        /// <summary>
        /// Creates a new spreadsheet workbook at the target file path.
        /// </summary>
        /// <param name="filepath">Where to create workbook.</param>
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


        /// <summary>
        /// Inserts test data from spatial test into passed worksheet part.
        /// </summary>
        /// <param name="newWorksheetPart"></param>
        /// <param name="test"></param>
        /// <returns></returns>
        private static WorksheetPart InsertTestData(WorksheetPart newWorksheetPart, SpatialTest test)
        {
            uint rowIndex = 2;

            // Write test data into sheet
            foreach (TrialBlock block in test.TrialBlocks)
            {
                foreach (SpatialTrial trial in block.Trials)
                {
                    // Insert new cells into worksheet
                    Cell cellTrialNum = InsertCellInWorksheet("A", rowIndex, newWorksheetPart);
                    Cell cellOrientation = InsertCellInWorksheet("B", rowIndex, newWorksheetPart);
                    Cell cellStimPair = InsertCellInWorksheet("C", rowIndex, newWorksheetPart);
                    Cell cellResponseKey = InsertCellInWorksheet("D", rowIndex, newWorksheetPart);
                    Cell cellResponseTime = InsertCellInWorksheet("E", rowIndex, newWorksheetPart);
                    Cell cellAccuracy = InsertCellInWorksheet("F", rowIndex, newWorksheetPart);

                    // Assign data values to cells
                    cellTrialNum.CellValue = new CellValue((int)trial.TrialID);
                    cellOrientation.CellValue = new CellValue(trial.Orientation.ToString());
                    cellStimPair.CellValue = new CellValue(trial.TrialTargets.ToString());
                    cellResponseKey.CellValue = new CellValue(trial.ResponseKey.ToString());
                    cellResponseTime.CellValue = new CellValue((int)trial.ResponseTime);
                    cellAccuracy.CellValue = new CellValue((int)trial.Accuracy);

                    // Set data type of cells
                    cellTrialNum.DataType = new EnumValue<CellValues>(CellValues.Number);
                    cellOrientation.DataType = new EnumValue<CellValues>(CellValues.String);
                    cellStimPair.DataType = new EnumValue<CellValues>(CellValues.String);
                    cellResponseKey.DataType = new EnumValue<CellValues>(CellValues.String);
                    cellResponseTime.DataType = new EnumValue<CellValues>(CellValues.Number);
                    cellAccuracy.DataType = new EnumValue<CellValues>(CellValues.Number);

                    rowIndex++;
                }
            }

            // Write test information
            // Insert new information cells into worksheet
            Cell cellParticipantId = InsertCellInWorksheet("I", 1, newWorksheetPart);
            Cell cellHorizontalAccuracy = InsertCellInWorksheet("I", 4, newWorksheetPart);
            Cell cellVerticalAccuracy = InsertCellInWorksheet("I", 5, newWorksheetPart);
            Cell cellTotalAccuracy = InsertCellInWorksheet("I", 6, newWorksheetPart);

            // Assign data values to cells
            cellParticipantId.CellValue = new CellValue((int)test.ParticipantID);

            // Assign formulas to cells
            cellHorizontalAccuracy.CellFormula = new CellFormula("=SUMIF(B:B, \"horizontal\", F:F)/100");
            cellVerticalAccuracy.CellFormula = new CellFormula("=SUMIF(B:B, \"vertical\", F:F)/100");
            cellTotalAccuracy.CellFormula = new CellFormula("=SUM(I4,I5)");

            // Set data type of cells
            cellParticipantId.DataType = new EnumValue<CellValues>(CellValues.Number);
            cellHorizontalAccuracy.DataType = new EnumValue<CellValues>(CellValues.Number);
            cellVerticalAccuracy.DataType = new EnumValue<CellValues>(CellValues.Number);
            cellTotalAccuracy.DataType = new EnumValue<CellValues>(CellValues.Number);

            // Set up datetime cell
            Cell cellDateTime = InsertCellInWorksheet("I", 2, newWorksheetPart);
            string oaValue = test.DateTime.ToOADate().ToString();
            CellValue cellValueDateTime = new CellValue(oaValue);

            cellDateTime.DataType = new EnumValue<CellValues>(CellValues.Number);
            cellDateTime.StyleIndex = 1;
            cellDateTime.Append(cellValueDateTime);


            return newWorksheetPart;
        }
    }
}
