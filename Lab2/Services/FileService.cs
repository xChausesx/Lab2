namespace Lab2.Services
{
    public static class FileService
    {
        public static byte[]? ConvertToByteArr(IFormFile? formFile)
        {
            if(formFile == null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            var byteFile  = memoryStream.ToArray();

            return byteFile;
        }
    }
}
