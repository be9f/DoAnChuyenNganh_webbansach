using bansach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;

namespace bansach.Controllers
{
    public class AdminController : Controller
    {
        DbBanSachDataContext data = new DbBanSachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewBag["loi1"] = "Nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewBag["loi2"] = "Nhập mật khẩu";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PasswordAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu sai";
                }
            }
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(data.Saches.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(Sach sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");

            if (fileupload == null)
            {
                ViewBag.ThongBao = "Xin vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/HinhSanPham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.BiaSach = fileName;
                    data.Saches.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult ChiTiet(int id)
        {
            Sach s = data.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.MaSach = s.MaSach;
            if (s == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(s);
        }
        [HttpGet]
        public ActionResult XoaSach(int id)
        {
            Sach s = data.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.MaSach = s.MaSach;
            if (s == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(s);
        }

        [HttpPost, ActionName("XoaSach")]
        public ActionResult XacNhanXoa(int id)
        {
            Sach s = data.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.MaSach = s.MaSach;
            if (s == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Saches.DeleteOnSubmit(s);
            data.SubmitChanges();
            return RedirectToAction("Sach");
        }

        //[HttpGet]
        //public ActionResult SuaSach(int id)
        //{
        //    Sach s = data.Saches.SingleOrDefault(n => n.MaSach == id);
        //    ViewBag.MaSach = s.MaSach;
        //    if (s == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");
        //    return View(s);
        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult SuaSach(Sach sach, HttpPostedFileBase fileupload)
        //{
        //    ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");
        //    if (fileupload == null)
        //    {
        //        ViewBag.ThongBao = "Xin vui lòng chọn ảnh bìa";
        //        return View();
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var fileName = Path.GetFileName(fileupload.FileName);
        //            var path = Path.Combine(Server.MapPath("~/HinhSanPham"), fileName);
        //            if (System.IO.File.Exists(path))
        //            {
        //                ViewBag.ThongBao = "Hình ảnh đã tồn tại";
        //            }
        //            else
        //            {
        //                fileupload.SaveAs(path);
        //            }
        //            sach.BiaSach = fileName;
        //            UpdateModel(sach);
        //            data.SubmitChanges();
        //        }
        //        return RedirectToAction("Sach");
        //    }
        //}

        public ActionResult TheLoai()
        {
            return View(data.TheLoais.ToList());
        }
        [HttpGet]
        public ActionResult Themtheloai()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themtheloai(TheLoai theloai)
        {
            data.TheLoais.InsertOnSubmit(theloai);
            data.SubmitChanges();
            return RedirectToAction("TheLoai");
        }

        [HttpGet]
        public ActionResult XoaTheLoai(int id)
        {
            TheLoai tl = data.TheLoais.SingleOrDefault(n => n.MaTheLoai == id);
            ViewBag.MaTheLoai = tl.MaTheLoai;
            if (tl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tl);
        }

        [HttpPost, ActionName("XoaTheLoai")]
        public ActionResult XacNhanXoaTL(int id)
        {
            TheLoai tl = data.TheLoais.SingleOrDefault(n => n.MaTheLoai == id);
            ViewBag.MaTheLoai = tl.MaTheLoai;
            if (tl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TheLoais.DeleteOnSubmit(tl);
            data.SubmitChanges();
            return RedirectToAction("TheLoai");
        }

        public ActionResult Khach()
        {
            return View(data.KhachHangs.ToList());
        }

        [HttpGet]
        public ActionResult XoaKhachHang(int id)
        {
            KhachHang khach = data.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = khach.MaKH;
            if (khach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khach);
        }

        [HttpPost, ActionName("XoaKhachHang")]
        public ActionResult XacNhanXoaKH(int id)
        {
            KhachHang khach = data.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = khach.MaKH;
            if (khach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KhachHangs.DeleteOnSubmit(khach);
            data.SubmitChanges();
            return RedirectToAction("Khach");
        }
        public ActionResult HoaDon(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(data.DonDatHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CTHoaDon(int MaGioHang)
        {
            using (DbBanSachDataContext data = new DbBanSachDataContext())
            {
                List<KhachHang> kh = data.KhachHangs.ToList();
                List<DonDatHang> hd = data.DonDatHangs.ToList();
                List<Sach> sach = data.Saches.ToList();
                List<CT_DonDatHang> cthd = data.CT_DonDatHangs.ToList();
                var main = from h in hd
                           join k in kh on h.MaKH equals k.MaKH
                           where (h.MaGioHang == MaGioHang)
                           select new View
                           {
                               hd = h,
                               kh = k
                           };
                var sub = from c in cthd
                          join s in sach on c.MaSach equals s.MaSach
                          where (c.MaGioHang == MaGioHang)
                          select new View
                          {
                              cthd = c,
                              sach = s,
                              ThanhTien = Convert.ToDouble(c.GiaTien * c.SoLuong)
                          };
                ViewBag.Main = main;
                ViewBag.Sub = sub;
                return View();
            }
        }
        public ActionResult GopY()
        {
            return View(data.Gopies.ToList());
        }
        public ActionResult Chitietgopy(int id)
        {
            GopY gy = data.Gopies.SingleOrDefault(n => n.MaGY == id);
            ViewBag.MaGY = gy.MaGY;
            if (gy == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(gy);
        }
        [HttpGet]
        public ActionResult XoaGopY(int id)
        {
            GopY gy = data.Gopies.SingleOrDefault(n => n.MaGY == id);
            ViewBag.MaGY = gy.MaGY;
            if (gy == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(gy);
        }

        [HttpPost, ActionName("XoaGopY")]
        public ActionResult XacNhanXoaGY(int id)
        {
            GopY gy = data.Gopies.SingleOrDefault(n => n.MaGY == id);
            ViewBag.MaGY = gy.MaGY;
            if (gy == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Gopies.DeleteOnSubmit(gy);
            data.SubmitChanges();
            return RedirectToAction("GopY");
        }
    }
}