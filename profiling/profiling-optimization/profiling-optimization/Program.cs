// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using profiling_optimization;


byte[] GenerateSalt(int length)
{
    var salt = new byte[length];
    System.Security.Cryptography.RandomNumberGenerator.Fill(salt);
    return salt;
}

var salt = GenerateSalt(16);

var passwords = new[]
{
    "abc1234!John",
    "Asdfghjklqwertyuiozxcvbnnm;'/.,mnbvcxzlkjhgfdsapoiuytrewq;",
    "456",
    "@#$%^&*()_+{}|:<>?",
    "SecurePass21",
    "98765432109876543210-98765432109876543210987654321098765432109876543210",
    "987987987987987987987987987987987987987987987987987987987987987987987987987987987",
    "RandomWord890!",
    "ZXCVBNM<>?:LKJHGFDSAOIUYTREWQ",
    "789",
    "!@#$%^&*()_+?><:{}|",
    "AnotherPassword34",
    "567567567567567567567567567567567567567567567567567567567567567567567567567567567",
};

var profiling = new Profiling();

//for (var i = 0; i < passwords.Length; i += 1)
{
    var passwordHashOriginal = profiling.GeneratePasswordHashUsingSalt(passwords[0], salt);
    var passwordHashOriginalUpdated = profiling.GeneratePasswordHashUsingSaltUpdated(passwords[0], salt); 
    var passwordHashOriginalArgon2 = profiling.GeneratePasswordHashUsingArgon2(passwords[0], salt);
}

byte a = 1;
byte b = 2;
var c = a + b;

