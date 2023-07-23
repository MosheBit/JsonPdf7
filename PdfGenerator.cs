using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using Font = iTextSharp.text.Font;
using LiveCharts;
using LiveCharts.Wpf;
using System.IO;
using ChartType = System.Windows.Forms.DataVisualization.Charting.Chart;
using iText.Kernel.Colors;



namespace JsonPdf6
{
    public static class PdfGenerator
    {
        private static PdfFooter pdfFooter; // הוספת המשתנה הגלובלי כאן



        public static void GeneratePdf(List<Employee> employees, string outputPath, string logoPath, string userFooterText)
        {


            // יצירת מסמך PDF חדש
            Document document = new Document();
            // כאן יש להשתמש במשתנה outputPath כדי לקבוע את הנתיב ליצירת הקובץ ה-PDF
            string pdfFilePath = "path/to/your/pdf/file.pdf";
          //  PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));

            // יצירת מחלקת PdfWriter חדשה והגדרת PdfFooterPageEvent כאירוע הדף
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
            PdfFooterPageEvent pdfFooterEvent = new PdfFooterPageEvent(userFooterText);
            writer.PageEvent = pdfFooterEvent;


            document.Open();
            document.NewPage();// לגרף


            // הוספת כותרת באנגלית מעל הקו השחור
            string headerText = "Nevet Israel Pools";
            PdfContentByte content = writer.DirectContent;
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            content.SetFontAndSize(font, 12);
            content.BeginText();
            content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, headerText, (document.PageSize.Width - document.LeftMargin - document.RightMargin) / 2 + document.LeftMargin, document.PageSize.Height - document.TopMargin + 10, 0);
            content.EndText();

            // קו שחור מתחת לכותרת
            content.SetLineWidth(1f);
            content.SetColorStroke(BaseColor.BLACK);
            content.MoveTo(document.LeftMargin, document.PageSize.Height - document.TopMargin + 5);
            content.LineTo(document.PageSize.Width - document.RightMargin, document.PageSize.Height - document.TopMargin + 5);
            content.Stroke();

            /////////////////////////////////////////////////


            // הוספת הלוגו למסמך ה-PDF
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
            logo.ScaleToFit(100f, 100f); // קביעת גודל הלוגו
            logo.SetAbsolutePosition(20f, document.PageSize.Height - 120f); // קביעת מיקום הלוגו
            document.Add(logo);


            // הוספת מרווח עליון לפני הטבלה
            for (int i = 0; i < 7; i++)
            {
                document.Add(new Paragraph(" ")); // שורת רווח ריקה
            }


            /////////////////////////////////////////////////////////

            // יצירת טבלה עם 5 עמודות
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100; // רוחב הטבלה באחוזים מהרוחב הכולל של המסמך
            table.SetWidths(new float[] { 20f, 20f, 20f, 20f, 20f }); // קביעת רוחב עמודות הטבלה
            table.HorizontalAlignment = Element.ALIGN_LEFT; // יישור הטבלה לשמאל

            ///////////////////////////////////////////////////////

            // הוספת כותרות העמודות
            table.AddCell("ID");
            table.AddCell("First Name");
            table.AddCell("Last Name");
            table.AddCell("Phone Number");
            table.AddCell("Email");

            // הוספת הנתונים לטבלה
            foreach (var employee in employees)
            {
                table.AddCell(employee.ID.ToString());
                table.AddCell(employee.FirstName);
                table.AddCell(employee.LastName);
                table.AddCell(employee.PhoneNumber);
                table.AddCell(employee.Email);
            }

            // הוספת הטבלה למסמך ה-PDF
            document.Add(table);

            //////////////////////////////////GGGGGGGGGGGGGGGGG///
            // גרף עמודות
            PdfContentByte contentByte = writer.DirectContent;
            float columnHeight = 200f; // גובה העמודות

            float graphXPosition = 100f; // מיקום הגרף בציר האיקס בעמוד
            float graphYPosition = 100f; // מיקום הגרף בציר הוואי בעמוד

            float columnWidth = 50f; // רוחב העמודות

            contentByte.SetRGBColorFill(100, 149, 237); // צבע מילוי העמודות - במקרה זה, צבע כחול

            int maxValue = employees.Max(e => e.ID); // תופש את ID לגרף

            float documentWidth = document.PageSize.Width; // קביעת רוחב הקובץ ה-PDF
            int maxColumns = (int)(documentWidth / columnWidth); // מספר העמודות המרבי בשורה

            int totalColumns = employees.Count; // מספר עמודות כולל

            float fixedColumnWidth = documentWidth / totalColumns; // רוחב עמודה קבוע

            foreach (var employee in employees)
            {
                float columnHeightScaled = (employee.ID / (float)maxValue) * columnHeight; // קנה המידה של גובה העמודה מתוך ערכי הטבלה

                // חישוב הרוחב הפנימי של העמודה בהתאם לרוחב הקבוע והרוחב הנותר
                float remainingWidth = documentWidth - graphXPosition;
                float innerColumnWidth = Math.Min(fixedColumnWidth, remainingWidth / totalColumns);

                // ציור העמודה בגודל המתאים (עם רוחב קבוע)
                contentByte.Rectangle(graphXPosition, graphYPosition, innerColumnWidth, columnHeightScaled);
                graphXPosition += innerColumnWidth; // עבור למיקום העמודה הבאה
            }

            contentByte.Fill(); // מילוי העמודות בצבע המילוי שנקבע

            //int maxValue = employees.Select(e => int.Parse(e.PhoneNumber)).Max(); אם אני רוצה לבצעה ערך לא מספר זה הקוד

            //// יצירת גרף עוגה
            //PieChart pieChart = new PieChart();
            //pieChart.AddValue("Category 1", 20);
            //pieChart.AddValue("Category 2", 30);
            //pieChart.AddValue("Category 3", 50);

            //// ציור הגרף על הדף
            //PdfContentByte content = writer.DirectContent;
            //pieChart.Draw(content, new Rectangle(100, 100, 300, 300));




            ///////////////////////////////////////////////////////////////////


            //פנקציה לקריאת החתימה 
            // החלף "חתימה" ו"___________" בתוכן החתימה המתאימה
            //SignatureHelper.AddSignature(document, writer, "חתימה", "___________"); 

            /////
             string footerText = "טקסט תחתית הדף"; // טקסט תחתית הדף שאתה רוצה להציג

            //// יצירת אובייקט מסוג PdfFooter עם הטקסט של תחתית הדף
            PdfFooter pdfFooter = new PdfFooter(footerText);

            //// הצמדת האובייקט הנ"ל לכתובת הפניה הנוכחית ב-PageEvent של ה-writer
           // writer.PageEvent = pdfFooter;




            document.NewPage();// לגרף
            document.Close();


           

        }


    }


    // קלאס זה הועתק ישירות מהקוד שלך ושונה את השם שלו
    public class PdfFooterPageEvent : PdfPageEventHelper
    {
        private string footerText;
        // הוספת שדה פרטיות עבור הטקסט בתחתית הדף

        public PdfFooterPageEvent(string footerText)
        {
            this.footerText = footerText; // שמירת הטקסט שיבוא מהאפליקציה
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // קו שחור למטה 
            PdfContentByte content = writer.DirectContent;
            content.SetLineWidth(1f);
            content.SetColorStroke(BaseColor.BLACK);
            content.MoveTo(document.LeftMargin, document.BottomMargin);
            content.LineTo(document.PageSize.Width - document.RightMargin, document.BottomMargin);
            content.Stroke();

            // הצגת הטקסט שקיבלנו מהאפליקציה
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            content.SetFontAndSize(font, 12);
            content.BeginText();
            content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, footerText, (document.PageSize.Width - document.LeftMargin - document.RightMargin) / 2 + document.LeftMargin, document.BottomMargin - 20, 0);
            content.EndText();
        }
    }


    public class PdfFooter
    {
        // ערך התחתית
        public string FooterText { get; set; }

        // קונסטרוקטור עם ערך התחתית כפרמטר
        public PdfFooter(string footerText)
        {
            FooterText = footerText;
        }
    }

    public class SignatureHelper
    {
        public static void AddSignature(Document document, PdfWriter writer, string signatureText, string lineText)
        {
            // יצירת חתימה
            PdfPTable signatureTable = new PdfPTable(1);
            signatureTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            signatureTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell signatureCell = new PdfPCell(new Phrase(signatureText, new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD)));
            signatureCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            signatureTable.AddCell(signatureCell);

            // הוספת קו קטן מתחת לחתימה
            PdfPCell lineCell = new PdfPCell(new Phrase(lineText, new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL)));
            lineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            lineCell.HorizontalAlignment = Element.ALIGN_RIGHT; // יישור הערה לצד ימין
            signatureTable.AddCell(lineCell);

            // מיקום החתימה והקו בתחתית העמוד בצד שמאל
            signatureTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            float tableHeight = signatureTable.TotalHeight;
            signatureTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.BottomMargin - tableHeight, writer.DirectContent);
        }


        

    }



}
