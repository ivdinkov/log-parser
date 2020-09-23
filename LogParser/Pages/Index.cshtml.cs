using LogParser.models;
using LogParser.Service;
using LogParser.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogParser.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogRepository _logRepository;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt", ".zip" };

        [BindProperty]
        public BufferedMultipleFileUploadDb FileUpload { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Param { get; set; }
        public IEnumerable<Log> Logs { get; set; }
        public string ErrorMsg { get; private set; }



        public IndexModel(IConfiguration config, ILogRepository logRepository)
        {
            _logRepository = logRepository;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
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
                var fileContent =
                    await FileHelpers.ProcessFormFile<BufferedMultipleFileUploadDb>(
                        formFile, ModelState, _permittedExtensions,
                        _fileSizeLimit);

                if (!ModelState.IsValid)
                {
                    ErrorMsg = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return Page();
                }

                await _logRepository.SaveAllLogsAsync(fileContent);
            }

            Logs = await _logRepository.GetAllLogsAsync();

            return Page();
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
