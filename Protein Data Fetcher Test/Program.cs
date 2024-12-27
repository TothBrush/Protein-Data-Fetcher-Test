using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter the PDB ID to fetch the CIF file:");
        string pdbId = Console.ReadLine()?.Trim();

        // Validate user input
        if (string.IsNullOrEmpty(pdbId) || pdbId.Length != 4)
        {
            Console.WriteLine("Invalid PDB ID. It should be a 4-character string (e.g., '1AON').");
            return;
        }

        string pdbUrl = $"https://files.rcsb.org/download/{pdbId}.cif";
        string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..");
        string dataDirectory = Path.Combine(projectRoot, "data");
        Directory.CreateDirectory(dataDirectory); // Ensure the data directory exists
        string cifFilePath = Path.Combine(dataDirectory, $"{pdbId}.cif");

        try
        {
            using HttpClient client = new HttpClient();
            Console.WriteLine($"Fetching CIF file for {pdbId}...");

            // Send GET request to fetch the CIF file
            HttpResponseMessage response = await client.GetAsync(pdbUrl);
            response.EnsureSuccessStatusCode();

            // Read CIF content
            string cifContent = await response.Content.ReadAsStringAsync();

            // Save the CIF content to a file
            await File.WriteAllTextAsync(cifFilePath, cifContent);

            Console.WriteLine($"CIF file saved to: {cifFilePath}");
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Failed to fetch the CIF file. Error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
