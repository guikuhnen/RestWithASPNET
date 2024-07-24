using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Authorize("Bearer")]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class FileController(ILogger<FileController> logger, IFileBusiness fileBusiness) : ControllerBase
	{
		private readonly ILogger<FileController> _logger = logger;
		private readonly IFileBusiness _fileBusiness = fileBusiness;

		[HttpGet("downloadFile/{fileName}")]
		[ProducesResponseType(200, Type = typeof(byte[]))]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(500)]
		[Produces("application/octet-stream")]
		public async Task<IActionResult> DownloadFile(string fileName)
		{
			byte[] buffer = _fileBusiness.GetFile(fileName);

			if (buffer != null)
			{
				HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
				HttpContext.Response.Headers.Append("content-length", buffer.Length.ToString());
				await HttpContext.Response.Body.WriteAsync(buffer);
			}

			return new ContentResult();
		}

		[HttpPost("uploadFile")]
		[ProducesResponseType(200, Type = typeof(FileDetailVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(500)]
		[Produces("application/json")]
		public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
		{
			var fileDetailVO = await _fileBusiness.SaveFileToDisk(file);

			return Ok(fileDetailVO);
		}

		[HttpPost("uploadFiles")]
		[ProducesResponseType(200, Type = typeof(List<FileDetailVO>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(500)]
		[Produces("application/json")]
		public async Task<IActionResult> UploadFiles([FromForm] ICollection<IFormFile> files)
		{
			var fileDetails = await _fileBusiness.SaveFilesToDisk(files);

			return Ok(fileDetails);
		}
	}
}
