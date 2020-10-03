using LogParser.models;
using LogParser.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace LogParser.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogRepository _logRepository;
        private byte[] fileContent;

        [BindProperty]
        public BufferedMultipleFileUploadDb FileUpload { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Param { get; set; }
        public IEnumerable<Log> Logs { get; set; }
        public string ErrorMsg { get; private set; }



        public IndexModel(IConfiguration config, ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }


        public void OnGet()
        {
            Logs = new List<Log>();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMsg = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return Page();
            }

            await _logRepository.ClearLogsAsync();

            foreach (var formFile in FileUpload.FormFiles)
            {
                var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

                try
                {
                    if (ext.Equals(".zip"))
                    {
                        using var memoryStream = formFile.OpenReadStream();
                        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read);
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            var unzippedFile = entry.Open();
                            using var stream = new MemoryStream();
                            await unzippedFile.CopyToAsync(stream);
                            await SaveLogs(stream);
                        }
                    }
                    else
                    {
                        using var memoryStream = new MemoryStream();
                        await formFile.CopyToAsync(memoryStream);
                        await SaveLogs(memoryStream);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                if (!ModelState.IsValid)
                {
                    ErrorMsg = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return Page();
                }
            }

            Logs = await _logRepository.GetAllLogsAsync();

            return Page();
        }

        private async Task SaveLogs(MemoryStream stream)
        {
            fileContent = stream.ToArray();
            await _logRepository.SaveAllLogsAsync(fileContent);
        }

        public async Task<IActionResult> OnGetReData()
        {
            if (Param == null)
            {
                return BadRequest();
            }

            var result = await _logRepository.GetDataAsync(Param);

            return new JsonResult(result);
        }
    }
}