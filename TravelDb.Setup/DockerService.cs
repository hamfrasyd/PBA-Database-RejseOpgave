using System.Diagnostics;

namespace TravelDb.Setup;

/// <summary>
/// Denne klasse har ansvaret for at starte en SQL Server Docker-container ved hjælp af Docker Compose.
/// </summary>
public class DockerService
{
    /// <summary>
    /// Finder compose.yaml og starter Docker-containeren.
    /// </summary>
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


    /// <summary>
    /// Finder compose.yaml ved at gå op gennem mappestrukturen.
    /// </summary>
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
}