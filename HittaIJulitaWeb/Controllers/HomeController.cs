using HittaIJulitaWeb.Models;
using HittaIJulitaWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HittaIJulitaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search(SearchViewModel search)
        {
            if (search.Street == null && search.Place == null && search.PostalCode == null)
            {
                ViewBag.ErrorMessage = "Minst ett fält måste användas";
                return View("Index");
            }
            var result = FindMatches(search);
            return View("ResultPage", result);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public static List<SearchViewModel> FindMatches(SearchViewModel search)
        {
            List<SearchViewModel> matches = new List<SearchViewModel>();
            var rows = System.IO.File.ReadAllLines(@"..\HittaIJulitaWeb\Data\julita-postkod-db[1024].txt").ToList();
            rows = rows.Skip(1).ToList();
            foreach (var row in rows)
            {
                var columns = row.Split(';');
                bool found = SearchFile(columns, search.Street, search.PostalCode, search.Place);
                if (found)
                {
                    matches.Add(new SearchViewModel() { Street = columns[0], PostalCode = columns[1], Place = columns[2] });
                }
            }
            return matches;
        }
        public static bool SearchFile(string[] columns, string street, string postalCode, string place)
        {
            if (street != null)
            {
                if (columns[0].Contains(street, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            if (postalCode != null)
            {
                if (columns[1].Contains(postalCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            if (place != null)
            {
                if (columns[2].Contains(place, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
