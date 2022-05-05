using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Extensions;
using ShopDienThoai.Models;
using ShopDienThoai.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDienThoai.Controllers
{
    public class DonHangController : Controller
    {
        private readonly DienthoaiDBContext _context;
        public INotyfService _notyfService { get; }
        public DonHangController(DienthoaiDBContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts");
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(id));
                if(khachhang == null)
                {
                    return NotFound();
                }
                var donhang = await _context.Orders.Include(x => x.TransactStatus)
                    .FirstOrDefaultAsync(m => m.OrderId == id && Convert.ToInt32(taikhoanID) == m.CustomerId);
                if(donhang == null)
                {
                    return NotFound();
                }
                var chitietdonhang = _context.OrderDetails.Include(x => x.Product).AsNoTracking()
                    .Where(x => x.OrderId == id).OrderBy(x=>x.OrderDetailId).ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                //ViewBag.DH = donhang;
                return PartialView("Details", donHang);
                //return View(donHang);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
