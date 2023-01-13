using StudentAdministrationSystem.Models;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StudentAdministrationSystem.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace StudentAdministrationSystem.DTO
{
    public class GenerateReport : GradeStudent
    {
        public GenerateReport(StudentAdministrationSystemContext context) : base(context)
        {
        }
        //Generate PDF for finalized student result
        public byte[] CreateResultInPDF(int StudentId)
        {
         
            var finalResult = FinalResult(StudentId);
            var output = new MemoryStream();
            if (finalResult != null)
            {
                var document = new Document(PageSize.A4, 50, 50, 25, 25);

              
                var writer = PdfWriter.GetInstance(document, output);

                document.Open();

                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
                var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

                document.Add(new Paragraph("STUDENT MANAGER RESULT SHEET", titleFont));
                document.Add(new Paragraph("Student Results are below", bodyFont));

                document.Add(Chunk.NEWLINE);
                document.Add(new Paragraph("Student Information: ", subTitleFont));
                var header = new PdfPTable(2);
                header.HorizontalAlignment = 0;
                header.SpacingBefore = 10;
                header.SpacingAfter = 10;
                header.DefaultCell.Border = 0;
                header.SetWidths(new int[] { 1, 4 });

                header.AddCell(new Phrase("Student No.:", boldTableFont));
                header.AddCell(finalResult.Student.StudentNumber);
                header.AddCell(new Phrase("Full Name :", boldTableFont));
                header.AddCell(finalResult.Student.FirstName+" "+finalResult.Student.LastName);
                header.AddCell(new Phrase("Email :", boldTableFont));
                header.AddCell(finalResult.Student.Email);
                header.AddCell(new Phrase("Programme :", boldTableFont));
                header.AddCell(finalResult.DegreeProgramme.Title);
                header.AddCell(new Phrase("Code :", boldTableFont));
                header.AddCell(finalResult.DegreeProgramme.Code);
                header.AddCell(new Phrase("Average :", boldTableFont));
                header.AddCell(finalResult.Overall.ToString());
                header.AddCell(new Phrase("Grade :", boldTableFont));
                header.AddCell(finalResult.Remarks);
                document.Add(header);

                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("Result Informatiion: ", subTitleFont));

                var body = new PdfPTable(4);
                body.HorizontalAlignment = 0;
                body.SpacingBefore = 10;
                body.SpacingAfter = 10;
                body.TotalWidth = 9f;

                body.AddCell(new Phrase("Module Code", boldTableFont));
                body.AddCell(new Phrase("Module Name", boldTableFont));
                body.AddCell(new Phrase("Average", boldTableFont));
                body.AddCell(new Phrase("Grade", boldTableFont));

             
                
                foreach (var item in finalResult.Course)
                {
                    body.AddCell(item.Code);
                    body.AddCell(item.Name);
                    body.AddCell(item.Marks.ToString());
                    body.AddCell(item.Remarks);                   
                }
                document.Add(body);
                DateTime dateTime = DateTime.Now;

                PdfContentByte cb = writer.DirectContent;
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(bf, 10);
                cb.BeginText();
                cb.SetTextMatrix(50, 5);
                cb.ShowText(dateTime.ToString());
                cb.EndText();
                document.Close();


                return output.ToArray();
            }
            return output.ToArray();
        }
          
        }
}
