using BASEDDEPARTMENT.EntityModels;
using BASEDDEPARTMENT.Repositories;
using BASEDDEPARTMENT.Services.AccountService;
using BASEDDEPARTMENT.Services.CommentService;
using BASEDDEPARTMENT.Services.PostService;

namespace BASEDDEPARTMENT
{
    public static class Startup
	{
		public static void ConfigureServices(this IServiceCollection services)
		{
			services.AddScoped<IRepository<Post>, PostRepository>();
			services.AddScoped<IPostService, PostService>();
			
			services.AddScoped<IRepository<Comment>, CommentRepository>();
			services.AddScoped<ICommentService, CommentService>();

			services.AddScoped<IAccountService, AccountService>();
		}
	}
}
