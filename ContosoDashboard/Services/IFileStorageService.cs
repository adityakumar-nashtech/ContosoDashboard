using System;
using System.IO;
using System.Threading.Tasks;

namespace ContosoDashboard.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, IProgress<long>? progress = null);
    Task DeleteAsync(string filePath);
    Task<Stream> DownloadAsync(string filePath);
    Task<string> GetUrlAsync(string filePath, TimeSpan expiration);
}
