using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ContosoDashboard.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IWebHostEnvironment env)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _basePath = Path.Combine(appData, "ContosoDashboard", "uploads");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, IProgress<long>? progress = null)
    {
        var id = Guid.NewGuid().ToString();
        var ext = Path.GetExtension(fileName);
        var fileNameOnDisk = id + ext;
        var fullPath = Path.Combine(_basePath, fileNameOnDisk);

        const int bufferSize = 81920;
        byte[] buffer = new byte[bufferSize];
        long totalRead = 0;

        using var fs = File.Create(fullPath);
        int read;
        while ((read = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await fs.WriteAsync(buffer, 0, read);
            totalRead += read;
            progress?.Report(totalRead);
        }

        return fileNameOnDisk;
    }

    public Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public Task<Stream> DownloadAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        Stream fs = File.OpenRead(fullPath);
        return Task.FromResult(fs);
    }

    public Task<string> GetUrlAsync(string filePath, TimeSpan expiration)
    {
        // Local implementation: return absolute file path
        var fullPath = Path.Combine(_basePath, filePath);
        return Task.FromResult(fullPath);
    }
}
