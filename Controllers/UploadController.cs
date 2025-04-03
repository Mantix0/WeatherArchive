using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherArchive.Models;

namespace WeatherArchive.Controllers;

public class UploadController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _context;

    public UploadController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    private int? GetIntValue(ICell cell)
    {
        try
        {
            return (int)cell.NumericCellValue;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private List<WeatherRecord> ParseExcelFile(MemoryStream stream)
    {
        var records = new List<WeatherRecord>();

        XSSFWorkbook workbook = new XSSFWorkbook(stream);

        foreach (ISheet sheet in workbook)
        {
            for (int rowNumber = 4; rowNumber <= sheet.LastRowNum; rowNumber++)
            {
                IRow row = sheet.GetRow(rowNumber);

                var record = new WeatherRecord
                {
                    Date = DateOnly.Parse(row.GetCell(0).StringCellValue),
                    Time = TimeOnly.Parse(row.GetCell(1).StringCellValue),
                    Temperature = row.GetCell(2)?.NumericCellValue,
                    Humidity = row.GetCell(3)?.NumericCellValue,
                    DewPoint = row.GetCell(4)?.NumericCellValue,
                    Pressure = GetIntValue(row.GetCell(5)),
                    WindDirection = row.GetCell(6)?.StringCellValue,
                    WindSpeed = GetIntValue(row.GetCell(7)),
                    CloudCover = GetIntValue(row.GetCell(8)),
                    CloudBase = GetIntValue(row.GetCell(9)),
                    HorizontalVisibility = GetIntValue(row.GetCell(10)),
                    WeatherPhenomena = row.GetCell(11)?.StringCellValue,
                };
                records.Add(record);
            }
        }

        return records;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
        List<string> failedFilesNames = [];
        string resultString = "";

        if (files.Count == 0)
        {
            ViewBag.Message = "Выберите хотя бы один файл";
            return View("Index");
        }

        foreach (var file in files)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            try
            {
                var records = ParseExcelFile(memoryStream);
                await _context.WeatherRecords.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при обработке файла {file.FileName}: {e.Message}");
                failedFilesNames.Add(file.FileName);
                _context.ChangeTracker.Clear();
            }
        }

        if (failedFilesNames.Count > 0)
        {
            resultString += "Ошибка при обработке файлов: ";
            foreach (var name in failedFilesNames)
            {
                resultString += name + ", ";
            }
            resultString = resultString.Trim().TrimEnd(',');
        }
        else
        {
            resultString += "Все файлы успешно обработаны";
        }

        ViewBag.Message = resultString;
        return View("Index");
    }

    public IActionResult Index()
    {
        return View();
    }
}
