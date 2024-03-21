using BASEDDEPARTMENT.Entities;

namespace BASEDDEPARTMENT.Services.ImageService
{
	public interface IImageService
	{
		Task<bool> AddImage(Image image);
		Task<Image> GetImage(string id);
	}
}
