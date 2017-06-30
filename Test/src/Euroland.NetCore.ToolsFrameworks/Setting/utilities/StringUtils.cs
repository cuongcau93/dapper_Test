using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting.Utilities
{
    public class StringUtils
    {
        static System.Security.Cryptography.HashAlgorithm hash;
        private static System.Security.Cryptography.HashAlgorithm CreateHashAlgorithm()
        {
            if(hash == null)
                hash = System.Security.Cryptography.SHA1.Create();
            return hash;
        }
        /// <summary>
        /// Generates unique hash cod base on a string
        /// </summary>
        /// <param name="inputString">String to hash</param>
        /// <returns></returns>
        public static string GetHashString(string inputString)
        {
            byte[] byteData = CreateHashAlgorithm().ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputString));
            StringBuilder returnString = new StringBuilder();
            foreach (var b in byteData)
            {
                returnString.Append(b.ToString("x2"));
            }
            return returnString.ToString();
        }
    }
}
