using DbUp;
using System.Reflection;

namespace RejseAppDatabase.Services;

/// <summary>
/// MigrationService er ansvarlig for at køre database-migrationer ved hjælp af DbUp. 
/// Den finder og udfører alle SQL-scripts, der er indlejret i assemblyen, for at opdatere databasen til den nyeste version.
/// </summary>
public class MigrationService
{
    public void Run(string connectionString)
    {
        //opretter en opgraderingsinstans. til databaseændringer.
        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString) //databaseændringerne skal ske på denne database 
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()) // her finder den vores scripts (sql filer - som vi har lagt ind som embedded resources. 
            .LogToConsole() //dbup skriver information ud i konsollen, så man kan se hvad der sker
            .Build();

        var result = upgrader.PerformUpgrade(); //her undersøger dbup databaseændringerne og udfører dem, som mangler. 

        if (!result.Successful)
        {
            throw result.Error;
        }
    }
}