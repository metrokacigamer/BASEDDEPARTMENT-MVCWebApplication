using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Repositories;

namespace BASEDDEPARTMENT.Services.ImageService
{
	public class ImageService: IImageService
	{
		private readonly IRepository<Image> _imageRepository;
		private readonly MyDBContext _context;

		public ImageService(IRepository<Image> imageRepository, MyDBContext context)
		{
			_imageRepository = imageRepository;
			_context = context;
		}

		public async Task<bool> AddImage(Image image)
		{
			try
			{
				_imageRepository.Create(image);
				return await Task.FromResult(true);
			}
			catch (Exception)
			{
				return await Task.FromResult(false);
			}
		}

		public async Task<Image> GetImage (string id)
		{
			return await Task.FromResult(_imageRepository.Get(id));
		}

	}
}
