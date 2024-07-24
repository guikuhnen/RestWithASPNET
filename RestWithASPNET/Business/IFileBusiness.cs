using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business
{
	public interface IFileBusiness
	{
		byte[] GetFile(string fileName);
		Task<FileDetailVO> SaveFileToDisk(IFormFile file);
		Task<ICollection<FileDetailVO>> SaveFilesToDisk(ICollection<IFormFile> files);
	}
}
