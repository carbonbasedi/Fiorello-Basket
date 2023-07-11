namespace Fiorello.Utilities.File
{
	public interface IFileService
	{
		string Upload(IFormFile file);
		void Delete(string filePath);
		bool IsImage(IFormFile file);
		bool IsBiggerThanSize(IFormFile file, int size = 100);
	}
}
