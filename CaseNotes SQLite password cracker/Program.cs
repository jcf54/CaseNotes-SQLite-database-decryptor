using System.Data.SQLite;
SQLiteConnection conn;

int passwordAttempts = 0;

Console.WriteLine("Enter CaseNotes database (*.db3) file path: ");
string filePath = Console.ReadLine();

// https://github.com/brannondorsey/naive-hashcat/releases/download/data/rockyou.txt
Console.WriteLine("Enter dictionary file path (*.txt)");
string dictionaryPath = Console.ReadLine();

IEnumerable<string> allPasswords = File.ReadLines(@dictionaryPath);


// iterate over every password in the list
foreach (string password in allPasswords)
{
    passwordAttempts++;
    try
    {
        // Create connection with password from dictionary
        conn = new SQLiteConnection("Data Source=" + filePath + "; Password=" + password);
        // Open connection (not possible if wrong password)
        conn.Open();

        // By now, an exception will have been thrown if the password
        // is incorrect
        Console.WriteLine("Password is '" + password + "'");
        Console.WriteLine("Password found in " + passwordAttempts + " attempts");

        conn.Close();

        // Halt program execution
        break;
    }
    // if an SQLiteException is raised
    catch (SQLiteException ex)
    {
        // If the exception message DOES NOT contain "encrypted" ("file is encrypted or not a database")
        if (ex.ToString().IndexOf("file is encrypted or is not a database") == 0)
        {
            // throw original error
            throw ex;
        }
    }
}