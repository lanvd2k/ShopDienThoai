using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShopDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDienThoai.Controllers
{
    public class ProductController : Controller
    {
        private readonly DienthoaiDBContext _context;
        public ProductController(DienthoaiDBContext context)
        {
            _context = context;
        }
        //[Route("shop.html", Name = "ShopProduct")]
        //public IActionResult Index(int? page)
        //{
        //    try
        //    {
        //        var pageNumber = page == null || page < 0 ? 1 : page.Value;
        //        var pageSize = 10;
        //        var lsProducts = _context.Products.AsNoTracking().OrderByDescending(x => x.DateCreated);
        //        PagedList<Product> models = new PagedList<Product>(lsProducts, pageNumber, pageSize);
        //        ViewBag.CurrentPage = pageNumber;
        //        return View(models);
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }


        //}

        [Route("shop.html", Name = "ShopProduct")]
        public IActionResult Index(int? page, int CatID = 0)
        {
            try
            {
                var pageNumber = page == null || page < 0 ? 1 : page.Value;
                var pageSize = 10;
                List<Product> lsProducts = new List<Product>();
                if (CatID != 0)
                {
                    lsProducts = _context.Products.AsNoTracking().Where(x => x.CatId == CatID).Include(x => x.Cat).OrderByDescending(x => x.ProductId).ToList();
                }
                else
                {
                    lsProducts = _context.Products.AsNoTracking().Include(x => x.Cat).OrderByDescending(x => x.ProductId).ToList();
                }


                PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentCateID = CatID;
                ViewBag.CurrentPage = pageNumber;

                ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);

                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }

        public IActionResult Filter(int CatID = 0)
        {
            var url = $"/shop.html?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/shop.html";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        [Route("/{Alias}", Name = "ListProduct")]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x=>x.Alias== Alias);
                var pageSize = 10;
                var lsProducts = _context.Products.AsNoTracking().Where(x => x.CatId == danhmuc.CatId).OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var listProduct = _context.Products.AsNoTracking()
                    .Where(x => x.CatId == product.CatId && x.ProductId != id && x.Active == true)
                    .OrderByDescending(x=>x.DateCreated).Take(3).ToList();
                ViewBag.SanPham = listProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}
