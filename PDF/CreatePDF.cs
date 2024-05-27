using MVCFinalProject.Models;

using iTextSharp.text;

using iTextSharp.text.pdf;

namespace MVCFinalProject.PDF
{
    public class CreatePDF
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreatePDF(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public string UpdateRecipePdf(Recipe recipe)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string pdfDirectory = Path.Combine(wwwRootPath, "PDF");

            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            string fileName = $"AhmadNael.{recipe.User.FirstName}.{recipe.User.LastName}.pdf";
            string path = Path.Combine(pdfDirectory, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                var anyfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                document.Add(new Paragraph("Restoran recipies", anyfont));
                document.Add(new Paragraph("\n"));

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph("Recipe : " + recipe.Name, titleFont));
                document.Add(new Paragraph("\n"));

                var subTitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 15);
                document.Add(new Paragraph($"Chef: {recipe.User?.FirstName} {recipe.User?.LastName}", subTitleFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Category : {recipe.Category?.CategoryName}", subTitleFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Posted At: {recipe.CreationDate/*.ToString("dd/MMMM/yyyy")*/}", subTitleFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Price: ${recipe.Price}", subTitleFont));
                document.Add(new Paragraph("\n"));              
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);
                document.Add(new Paragraph("Description: ", bodyFont));
                document.Add(new Paragraph(recipe.Description + ".", bodyFont));
                document.Add(new Paragraph("\n"));


                document.Close();
            }
            return path;
        }

    }
}
    
