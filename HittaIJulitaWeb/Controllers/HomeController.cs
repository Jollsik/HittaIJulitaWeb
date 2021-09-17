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
            if (search.GatuPlats == null && search.Plats == null && search.Postnummer == null)
            {
                ViewBag.ErrorMessage = "Minst ett fält måste användas";
                return View("Index");
            }
            var result = SearchData(search.GatuPlats, search.Postnummer, search.Plats);
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
        public static List<SearchViewModel> SearchData(string street, string postalCode, string place)
        {
            List<SearchViewModel> matches = new List<SearchViewModel>();
            var rows = System.IO.File.ReadAllLines(@"..\HittaIJulitaWeb\Data\julita-postkod-db[1024].txt").ToList();
            rows = rows.Skip(1).ToList();
            foreach(var row in rows)
            {
                var columns = row.Split(';');
                if(columns[0].Contains(street, StringComparison.InvariantCultureIgnoreCase) || columns[1] == postalCode || columns[2] == place)
                {
                    matches.Add(new SearchViewModel() {GatuPlats = columns[0], Postnummer = columns[1], Plats = columns[2] });
                }
            }
            return matches;
        }
    }
}
