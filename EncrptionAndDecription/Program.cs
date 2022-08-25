using EncrptionAndDecription;
using System.Security.Cryptography; // CryptographicException

using static System.Console;
WriteLine("Registering Alice with Pa$$w0rd.");
var alice = Protector.Register("Alice", "Pa$$w0rd");
WriteLine($"Name: {alice.Name}");
WriteLine($"Salt: {alice.Salt}");
WriteLine("Password (salted and hashed): {0}",
arg0: alice.SaltedHashedPassword);
WriteLine();
Write("Enter a new user to register: ");
string? username = ReadLine();
Write($"Enter a password for {username}: ");
string? password = ReadLine();
var user = Protector.Register(username!, password!);
WriteLine($"Name: {user.Name}");
WriteLine($"Salt: {user.Salt}");
WriteLine("Password (salted and hashed): {0}",
arg0: user.SaltedHashedPassword);
WriteLine();
bool correctPassword = false;
while (!correctPassword)
{
    Write("Enter a username to log in: ");
    string? loginUsername = ReadLine();
    Write("Enter a password to log in: ");
    string? loginPassword = ReadLine();
    correctPassword = Protector.CheckPassword(
    loginUsername!, loginPassword!);
    if (correctPassword)
    {
        WriteLine($"Correct! {loginUsername} has been logged in.");
    }
    else
    {
        WriteLine("Invalid username or password. Try again.");
    }
}














//Write("Enter a message that you want to encrypt: ");
//string? message = ReadLine();
//Write("Enter a password: ");
//string? password = ReadLine();
//string cryptoText = Protector.Encrypt(message!, password!);
//WriteLine($"Encrypted text: {cryptoText}");
//Write("Enter the password: ");
//string? password2 = ReadLine();
//try
//{
//    string clearText = Protector.Decrypt(cryptoText, password2!);
//    WriteLine($"Decrypted text: {clearText}");
//}
//catch (CryptographicException ex)
//{
//    WriteLine("{0}\nMore details: {1}",
//    arg0: "You entered the wrong password!",
//    arg1: ex.Message);
//}
//catch (Exception ex)
//{
//    WriteLine("Non-cryptographic exception: {0}, {1}",
//    arg0: ex.GetType().Name,
//    arg1: ex.Message);
//}