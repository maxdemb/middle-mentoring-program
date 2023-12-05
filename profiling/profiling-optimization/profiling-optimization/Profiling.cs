using System.Security.Cryptography;
using System.Text;

using Konscious.Security.Cryptography;

namespace profiling_optimization;

public class Profiling
{


    public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
    {

        var iterate = 10000;
        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate); // some HASH generator 
        byte[] hash = pbkdf2.GetBytes(20); // HASH generator generates hash in byte[]

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        var passwordHash = Convert.ToBase64String(hashBytes);

        return passwordHash;

    }

    // should not be significantly faster
    public string GeneratePasswordHashUsingSaltUpdated(string passwordText, byte[] salt, int hashLength = 20, int iterations = 10000)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterations))
        {
            byte[] hash = pbkdf2.GetBytes(hashLength);
            byte[] hashBytes = new byte[salt.Length + hash.Length];

            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
    }

    public string GeneratePasswordHashUsingArgon2(string passwordText, byte[] salt, int hashLength = 20, int iterations = 2500, int memorySize = 65536, int degreeOfParallelism = 4)
    {
        using (var hasher = new Argon2id(password: Encoding.UTF8.GetBytes(passwordText)))
        {
            hasher.Salt = salt;
            hasher.DegreeOfParallelism = degreeOfParallelism;
            hasher.MemorySize = memorySize;
            hasher.Iterations = iterations;
            byte[] hash = hasher.GetBytes(hashLength);

            var passwordHash = Convert.ToBase64String(hash);
            return passwordHash;
        }
    }
}

