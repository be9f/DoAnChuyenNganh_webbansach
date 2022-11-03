using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bansach.Models;

namespace bansach.Controllers
{
    public class BookstoreController : Controller
    {
        DbBanSachDataContext data=new DbBanSachDataContext();
        private List<Sach> LaySachMoi(int count)
        {
            return data.Saches.OrderByDescending(a => a.NgayNhap).Take(count).ToList();
        }
        //GET: Bookstore
        public ActionResult Index()
        {
            var sachmoi= LaySachMoi(5);
            return View(sachmoi);
        }
       
        public ActionResult TheLoai()
        {
            var tl= from TheLoai in data.TheLoais select TheLoai;
            return PartialView(tl);
        }
        public ActionResult SPTheoTheLoai(int id)
        {
            var sach = from s in data.Saches where s.MaTheLoai== id select s;
            return View(sach);
        }
        public ActionResult ChiTiet(int id)
        {
            var sach = from s in data.Saches where s.MaSach == id select s;
            return View(sach.Single());
        }
        public ActionResult SearchProduct(string searchStr, int page = 0)
        {
            var Sach = from s in data.Saches select s;
            if (!String.IsNullOrEmpty(searchStr))
            {
                Sach = Sach.Where(p => p.TenSach.Contains(searchStr));
            }
            return View("SearchProduct", Sach.ToList());
        }

    }
}