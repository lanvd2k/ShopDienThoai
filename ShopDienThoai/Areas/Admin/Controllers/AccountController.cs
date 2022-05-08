using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Extensions;
using ShopDienThoai.Helper;
using ShopDienThoai.Models;
using ShopDienThoai.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly DienthoaiDBContext _context;

        public INotyfService _notifyService { get; }
        public AccountController(DienthoaiDBContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginViewModel customer)
        {
            var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);
            if (khachhang == null)
            {
                return RedirectToAction("DangKyTaiKhoan");
            }
            else
            {
                string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                if (khachhang.Password != pass)
                {
                    return View(customer);
                }
                else
                {
                    
                    _notifyService.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "AdminProducts");
                }
            }
            
        }

        
    }
}
