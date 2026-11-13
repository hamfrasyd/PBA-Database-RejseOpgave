using System.Diagnostics;

namespace TravelDb.Setup.Services;

//Denne klasse har ansvaret for at starte en SQL Server Docker-container ved hjælp af Docker Compose.
public class DockerService
{
    //Finder compose.yaml og starter Docker-containeren.
    public async Task StartAsync()
    {
        var composeFile = FindComposeFile();

        // Process.Start bruges til at starte en ny proces, i dette tilfælde Docker Compose, med de angivne argumenter.
        using var proces = Process.Start("docker", $"compose -f \"{composeFile}\" up -d");

        if (proces is null)
        {
            throw new Exception("Docker kunne ikke startes");
        }

        // WaitForExitAsync venter på, at processen afsluttes, før den fortsætter. Dette sikrer, at Docker-containeren er startet, før vi går videre.
        await proces.WaitForExitAsync();
        
        if (proces.ExitCode != 0)
        {
            throw new Exception("Docker-containeren kunne ikke startes. " + "Kontroller at Docker Desktop kører.");
        }
    }


    //Finder compose.yaml ved at gå op gennem mappestrukturen.
    private static string FindComposeFile()
    {
        //DirectoryInfo er en klasse der repræsenterer en mappe på filsystemet, her finder den den mappe hvor programmet kører fra
        DirectoryInfo directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            var path = Path.Combine(directory.FullName, "compose.yaml");

            if (File.Exists(path))
            {
                return path;
            }
           //går "op" i mappestrukturen indtil den finder compose.yaml
           directory = directory.Parent;
            
        }
        throw new FileNotFoundException("Kunne ikke finde compose.yaml.");
    }

    /// <summary>
    /// CreateFolderIfMissingAsync opretter en mappe inde i containeren, hvis den ikke allerede findes.
    /// mkdir -p fejler ikke, hvis mappen allerede findes, så metoden kan køres hver gang.
    /// </summary>
    public async Task CreateFolderIfMissingAsync(string containerName, string folderPath)
    {
        // -u mssql sikrer, at mappen bliver ejet af samme bruger, som SQL Server kører som.
        // Ellers kan SQL Server risikere ikke at have skriverettigheder til mappen.
        using var proces = Process.Start("docker", $"exec -u mssql {containerName} mkdir -p {folderPath}");

        if (proces is null)
        {
            throw new Exception("Docker kunne ikke startes");
        }

        await proces.WaitForExitAsync();

        if (proces.ExitCode != 0)
        {
            throw new Exception($"Kunne ikke oprette mappen '{folderPath}' i containeren '{containerName}'.");
        }
    }
}