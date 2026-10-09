//bruges til at finde SQL-scripts, der er injiceret i programmet
using DbUp;
using System.Diagnostics;
using System.Reflection;

//var connectionString =
//    "Server=tcp:pba-travel-server.database.windows.net;" +
//    "Database=TravelDb;" +
//    "User ID=sa-travel;" +
//    "Password=rejseopgave123!;" +
//    "Encrypt=True;" +
//    "TrustServerCertificate=False;";

var connectionString = "Server=localhost,1433;User Id=sa;Password=TravelDB!2026Local;TrustServerCertificate=True;";

//opretter en opgraderingsinstans. til databaseændringer.
var upgrader = DeployChanges.To
        .SqlDatabase(connectionString) //databaseændringerne skal ske på denne database 
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()) // her finder den vores scripts (sql filer - som vi har lagt ind som embedded resources. 
        .LogToConsole() //dbup skriver information ud i konsollen, så man kan se hvad der sker
        .Build(); 

var result = upgrader.PerformUpgrade(); //her undersøger dbup databaseændringerne og udfører dem, som mangler. 


//tester om opdateringen lykkedes. Hvis ikke, så vises fejlbeskeden i konsollen.
if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Databaseopdateringen fejlede:");
    Console.WriteLine(result.Error);
    Console.ResetColor();

    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Databaseopdateringen blev gennemført!");
Console.ResetColor();

return 0;