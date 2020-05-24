using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using Androtomist.Models.Database;

public class StringFuncs : DBClass
{

    public bool IsBase64(string base64String)
    {
        // Credit: oybek http://stackoverflow.com/users/794764/oybek
        if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
           || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            return false;

        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch
        {
            // Handle the exception
        }
        return false;
    }

    // Create a hash of the given password and salt.
    public string CreateHash(string password, string salt)
    {
        // Get a byte array containing the combined password + salt.
        string authDetails = password + salt;
        byte[] authBytes = Encoding.ASCII.GetBytes(authDetails);

        // Use MD5 to compute the hash of the byte array, and return the hash as
        // a Base64-encoded string.
        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashedBytes = md5.ComputeHash(authBytes);
        string hash = Convert.ToBase64String(hashedBytes);

        return hash;
    }

}