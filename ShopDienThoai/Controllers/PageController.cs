using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDienThoai.Controllers
{
    public class PageController : Controller
    {
        private readonly DienthoaiDBContext _context;
        public PageController(DienthoaiDBContext context)
        {
            _context = context;
        }
        
        [Route("/page/{Alias}", Name = "PageDetails")]
        public IActionResult Details(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                return RedirectToAction("Index", "Home");
            }
            var page = _context.Pages.AsNoTracking().SingleOrDefault(x => x.Alias == alias);
            if (page == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View(page);
        }
    }
}
