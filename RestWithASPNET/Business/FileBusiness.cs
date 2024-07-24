using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business
{
	public class FileBusiness(IHttpContextAccessor context) : IFileBusiness
	{
		private readonly string _basePath = Directory.GetCurrentDirectory() + "/UploadDir";
		private readonly IHttpContextAccessor _context = context;
		private static readonly string[] _acceptedExtesions = [".pdf", ".jpg", ".png", ".jpeg"];

		public byte[] GetFile(string fileName)
		{
			var filePath = _basePath + "/" + fileName;

			return File.ReadAllBytes(filePath);
		}

		public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
		{
			FileDetailVO fileDetail = new();
			var fileType = Path.GetExtension(file.FileName);
			var baseUrl = _context.HttpContext?.Request.Host;

			if (_acceptedExtesions.Contains(fileType.ToLower()))
			{
				var fileName = Path.GetFileName(file.FileName);

				if (file?.Length > 0)
				{
					var destination = Path.Combine(_basePath, "", fileName);

					fileDetail.FileName = fileName;
					fileDetail.FileType = fileType;
					fileDetail.FileUrl = Path.Combine(baseUrl + "/api/file/v1/" + fileName);

					using var stream = new FileStream(destination, FileMode.Create);
					await file.CopyToAsync(stream);
				}
			}

			return fileDetail;
		}

		public async Task<ICollection<FileDetailVO>> SaveFilesToDisk(ICollection<IFormFile> files)
		{
			List<FileDetailVO> listFiles = [];

			foreach (var file in files)
				listFiles.Add(await SaveFileToDisk(file));

			return listFiles;
		}
	}
}
