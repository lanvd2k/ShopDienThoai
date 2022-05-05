using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShopDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDienThoai.Controllers
{
    public class BlogController : Controller
    {
        private readonly DienthoaiDBContext _context;
        public BlogController(DienthoaiDBContext context)
        {
            _context = context;
        }
        [Route("blogs.html", Name = "Blog")]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page < 0 ? 1 : page.Value;
            var pageSize = 4;
            var lsTinDangs = _context.TinDangs.AsNoTracking().OrderByDescending(x => x.PostId);
            PagedList<TinDang> models = new PagedList<TinDang>(lsTinDangs, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
        [Route("/tin-tuc/{Alias}-{id}.html", Name = "TinDetails")]
        public IActionResult Details(int id)
        {
            var tindang = _context.TinDangs.AsNoTracking().SingleOrDefault(x => x.PostId == id);
            if (tindang == null)
            {
                return RedirectToAction("Index");
            }
            var listBaiVietLienQuan = _context.TinDangs.AsNoTracking().Where(x => x.Published == true && x.PostId != id)
                .Take(3).OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.BaiVietLienQuan = listBaiVietLienQuan;
            return View(tindang);
        }
    }
}
