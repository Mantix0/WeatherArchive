using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using WeatherArchive.Models;
using X.PagedList.Extensions;

namespace WeatherArchive.Controllers;

public class WeatherController : Controller
{
    private readonly ApplicationContext _context;
    private int pageSize = 100;

    public WeatherController(ApplicationContext context)
    {
        _context = context;
    }

    public IActionResult Index(
        int? page,
        string viewType = "year",
        int? year = null,
        int? month = null
    )
    {
        int minYear = 1970;
        int maxYear = DateTime.Now.Year;
        int pageNumber = page ?? 1;

        try
        {
            minYear = _context.WeatherRecords.Min(r => r.Date.Year);
            maxYear = _context.WeatherRecords.Max(r => r.Date.Year);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        ViewBag.ViewType = viewType;
        ViewBag.SelectedYear = year;
        ViewBag.SelectedMonth = month;
        ViewBag.MinYear = minYear;
        ViewBag.MaxYear = maxYear;

        IQueryable<WeatherRecord> records = _context
            .WeatherRecords.OrderBy(r => r.Date)
            .ThenBy(r => r.Time);

        if (viewType == "year")
        {
            records = records.Where(r => r.Date.Year == year);
        }
        else if (viewType == "month")
        {
            records = records.Where(r => r.Date.Month == month && r.Date.Year == year);
        }

        return View(records.ToPagedList(pageNumber, pageSize));
    }
}
