namespace EKemit.APIs.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //1-Get Folder Path

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", FolderName);

            //var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", FolderName);

            //2-Set FileName Unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            //3-GetFilePath
            var filePath = Path.Combine(FolderPath, fileName);

            //4-Save File As stream
            using var fs = new FileStream(filePath, FileMode.Create);

            //5-copy file into stream
            file.CopyTo(fs);
            //6-return fileName

            //return Path.Combine("images", "products", fileName);

            return Path.Combine("images", FolderName, fileName).Replace("\\", "/");

        }
        //pictureUrl => images/Products/jjiojioji
        public static void DeleteFile(string PictureUrl)
        {
                                                    //api/wwwroot/imagages/products/hiuhu.png 
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", PictureUrl).Replace("\\", "/"); ;
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
