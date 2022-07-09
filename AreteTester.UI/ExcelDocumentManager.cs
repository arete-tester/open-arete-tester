using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using AreteTester.Actions;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Xml.Serialization;
using System.IO;
using AreteTester.Core;

namespace AreteTester.UI
{
    internal class ExcelDocumentManager
    {
        public static event EventHandler<ExcelDocumentEventArgs> ApplyingInDocument;
        public static event EventHandler<ExcelDocumentEventArgs> ClearingInDocument;

        public static void Apply(Project project, List<string> resultFiles)
        {
            List<string> documents = new List<string> ();

            FillDocuments(project.Modules, documents);

            foreach (string document in documents)
            {
                if (ApplyingInDocument != null) ApplyingInDocument(null, new ExcelDocumentEventArgs() { Document = document });
                System.Threading.Thread.Sleep(3000);
                foreach (string resultFile in resultFiles)
                {
                    ApplyInDocument(document, resultFile);
                }
            }

            if (ApplyingInDocument != null) ApplyingInDocument(null, new ExcelDocumentEventArgs());
        }

        public static void Clear(Project project)
        {
            List<string> documents = new List<string>();

            FillDocuments(project.Modules, documents);

            foreach (string document in documents)
            {
                if (ClearingInDocument != null) ClearingInDocument(null, new ExcelDocumentEventArgs() { Document = document });

                ClearInDocument(document); System.Threading.Thread.Sleep(3000);
            }

            if (ClearingInDocument != null) ClearingInDocument(null, new ExcelDocumentEventArgs());
        }

        private static void ClearInDocument(string document)
        {
            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(document, true))
            {
                Dictionary<string, NamedCell> namedCells = GetNamedCells(spreadsheet);

                foreach (string name in namedCells.Keys)
                {
                    UpdateCell(spreadsheet, namedCells[name], "");
                }
            }
        }

        private static void ApplyInDocument(string document, string resultFile)
        {
            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(document, true))
            {
                Dictionary<string, NamedCell> namedCells = GetNamedCells(spreadsheet);

                Dictionary<string, string> nameResultMapping = GetNameResultMapping(resultFile);

                foreach (string name in nameResultMapping.Keys)
                {
                    if (namedCells.ContainsKey(name))
                    {
                        UpdateCell(spreadsheet, namedCells[name], nameResultMapping[name]);
                    }
                }
            }
        }

        private static void UpdateCell(SpreadsheetDocument spreadsheet, NamedCell namedCell, string text)
        {
            WorksheetPart worksheetPart = GetWorksheetPartByName(spreadsheet, namedCell.SheetName);

            if (worksheetPart != null)
            {
                Cell cell = GetCell(worksheetPart.Worksheet, namedCell.ColumnName, namedCell.RowIndex);

                if (cell == null) return;

                cell.CellValue = new CellValue(text);
                cell.DataType = new EnumValue<CellValues>(CellValues.String);

                worksheetPart.Worksheet.Save();
            }
        }

        private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0) return null;

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);

            return worksheetPart;
        }

        private static Cell GetCell(Worksheet worksheet, string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null) return null;

            return row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, columnName + rowIndex, true) == 0).First();
        }

        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex.Value == rowIndex).FirstOrDefault();
        }

        private static Dictionary<string, NamedCell> GetNamedCells(SpreadsheetDocument spreadsheet)
        {
            Dictionary<string, NamedCell> namedCells = new Dictionary<string, NamedCell>();

            foreach (var definedName in spreadsheet.WorkbookPart.Workbook.DefinedNames)
            {
                string name = definedName.GetAttribute("name", null).Value;
                string cellReference = definedName.InnerText;

                NamedCell namedCell = GetNamedCell(cellReference);
                namedCells.Add(name, namedCell);
            }

            return namedCells;
        }

        private static NamedCell GetNamedCell(string cellReference)
        {
            NamedCell namedCell = new NamedCell();

            string[] sheetParts = cellReference.Split("!".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            namedCell.SheetName = sheetParts[0];

            string[] indexParts = sheetParts[1].Split("$".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            namedCell.ColumnName = indexParts[0];
            namedCell.RowIndex = UInt32.Parse(indexParts[1]);

            return namedCell;
        }

        private static Dictionary<string, string> GetNameResultMapping(string resultFile)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<AssertionReportItem>));
            using (StreamReader reader = new StreamReader(resultFile))
            {
                List<AssertionReportItem> results = (List<AssertionReportItem>)serializer.Deserialize(reader);
                foreach (AssertionReportItem result in results)
                {
                    if (mapping.ContainsKey(result.Name) == false)
                    {
                        mapping.Add(result.Name, (result.Success == "1" ? "SUCCESS" : "FAIL"));
                    }
                    else
                    {
                        mapping[result.Name] = (result.Success == "1" ? "SUCCESS" : "FAIL");
                    }
                }
            }

            return mapping;
        }

        private static void FillDocuments(List<Module> modules, List<string> documents)
        {
            foreach (Module module in modules)
            {
                if (File.Exists(module.Document))
                {
                    documents.Add(module.Document);
                }

                if (module.Modules.Count > 0)
                {
                    FillDocuments(module.Modules, documents);
                }
            }
        }
    }

    class NamedCell
    {
        public string SheetName { get; set; }

        public string ColumnName { get; set; }

        public uint RowIndex { get; set; }
    }

    class ExcelDocumentEventArgs : EventArgs
    {
        public string Document { get; set; }
    }
}
