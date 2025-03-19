namespace MVC03.PL.Helpers
{
    public static class DocumentSetting
    {
        // 1.Upload
        public static string UploadFile(IFormFile file , string folderName)
        {
            // File Path
            //string folderPath = "C:\\Users\\HP\\Downloads\\MVC05-master\\MVC05-master\\MVC03.PL\\wwwroot\\files\\"+folderName;

            // var FolderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\"+folderName;

            var FolderPath= Path.Combine(Directory.GetCurrentDirectory() ,@"wwwroot\files" , folderName);

            // 2.File Name  : 

            var filename= $"{Guid.NewGuid()} {file.FileName}";

            var filepath = Path.Combine(FolderPath,filename);
            using var filestream = new FileStream(filepath,FileMode.Create);
            file.CopyTo(filestream);

            return filename;
        }
        // 2. Delete

        public static void DeleteFile(string fileName , string folderName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName , fileName);

            if (File.Exists(filepath))
                File.Delete(filepath);
        }
    }
}
