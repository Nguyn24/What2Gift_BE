namespace What2Gift.Application.Abstraction.Authentication;

public interface IImageUploader
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, string folderName);
}