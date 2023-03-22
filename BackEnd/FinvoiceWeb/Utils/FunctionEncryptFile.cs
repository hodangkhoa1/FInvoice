namespace FinvoiceWeb.Utils
{
    public class FunctionEncryptFile
    {
        public static byte[] ConvertFileToByte(string filePath)
        {
            byte[] fileByteArray = File.ReadAllBytes(filePath);
            return fileByteArray;
        }
    }
}
