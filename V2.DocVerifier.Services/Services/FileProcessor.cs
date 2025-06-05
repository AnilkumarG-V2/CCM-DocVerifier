using Microsoft.Extensions.Configuration;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.Interfaces;
using V2.DocVerifier.Services.Resources;

namespace V2.DocVerifier.Services.Services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IConfiguration _configuration;

        public FileProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// If the uploaded file is PDF then using SpirePDF SDK, the document is iterated through pages and each page is extracted and stored as image for further processing.
        /// </summary>
        /// <param name="model">GeminiViewModel</param>
        /// <returns></returns>
        public async Task SplitFileAsync(GeminiViewModel model)
        {
            string _datetime = DateTime.Now.ToString(Resource.FileNameDateTimeFormat);
            if (model.FormFile.Length > 0)
            {
                var _path = @$"{_configuration[Resource.ConfiguredImagePath]}{_datetime}";
                Directory.CreateDirectory(_path);

                model.FilePath = _path;

                string filePathWithFileName = $@"{_path}\{model.FormFile.FileName}";

                using (Stream fileStream = new FileStream(filePathWithFileName, FileMode.Create))
                {
                    await model.FormFile.CopyToAsync(fileStream);
                }

                if (model.FormFile.ContentType.ToLower().Contains(Resource.PDFFileExtension))
                {
                    using (PdfDocument pdfDocument = new PdfDocument())
                    {
                        pdfDocument.LoadFromFile(filePathWithFileName);

                        for (int _counter = 0; _counter < pdfDocument.Pages.Count; ++_counter)
                        {
                            var image = pdfDocument.SaveAsImage(_counter, PdfImageType.Bitmap);
                            image.Save(@$"{_path}\{Resource.ImageFilePrefix}{_counter + 1}.{Resource.DestinationImageFileExtension}");
                            model.ImageFiles.Add(@$"{_path}\{Resource.ImageFilePrefix}{_counter + 1}.{Resource.DestinationImageFileExtension}");
                        }
                    }
                }
                else
                {
                    model.ImageFiles.Add(filePathWithFileName);
                }
            }
        }
    }
}
