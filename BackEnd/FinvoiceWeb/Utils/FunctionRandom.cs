using System.Text;

namespace FinvoiceWeb.Utils
{
    public class FunctionRandom
    {
        public static string RandomCode(int length)
        {
            byte[] array = new byte[256];
            Random random = new();
            random.NextBytes(array);
            string randomString = Encoding.Default.GetString(array);
            StringBuilder stringBuffer = new();

            for (int i = 0; i < randomString.Length; i++)
            {
                char character = randomString[i];

                if (stringBuffer.ToString().Equals(""))
                {
                    if (((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z')) && (length > 0))
                    {
                        stringBuffer.Append(character);
                        length--;
                    }
                }
                else
                {
                    if (((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z') || (character >= '0' && character <= '9')) && (length > 0))
                    {
                        stringBuffer.Append(character);
                        length--;
                    }
                }
            }

            return stringBuffer.ToString();
        }
    }
}
