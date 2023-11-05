using Microsoft.AspNetCore.Mvc;

namespace JWTAuthencation.HelpMethod
{
    public static class HandleImage
    {
        public static async Task<string> Upload(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", filename);

                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {

            }
            return filename;
        }

        public static async Task<IActionResult> GetImageUrl(string imagePath)
        {
            string filePath = "Uploads/" + imagePath;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string mimeType = "image/jpeg";

            var stream = new MemoryStream(fileBytes);

            return new FileStreamResult(stream, mimeType);
        }
	}
}
