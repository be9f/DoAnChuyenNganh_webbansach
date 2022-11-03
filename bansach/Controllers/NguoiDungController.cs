using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bansach.Models;

namespace bansach.Controllers
{
    public class NguoiDungController : Controller
    {
        DbBanSachDataContext db = new DbBanSachDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var xnmatkhau = collection["Matkhaunhattrlai"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var email = collection["Email"];
            var ngaysinh = string.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Vui lòng nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(xnmatkhau))
            {
                ViewData["Loi4"] = "Vui lòng nhập lại mật khẩu";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi5"] = "Vui lòng nhập địa chỉ";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Vui lòng nhập SĐT";
            }
            else
            {
                kh.HoVaTenKH = hoten;
                kh.TenDangNhap = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.SDT = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TenDangNhap == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    //ViewBag.Thongbao = "Dang nhap thang cong";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu sai";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult GopY()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GopY(FormCollection collection, GopY gy)
        {
            var hoten = collection["TenKH"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var noidunggopy = collection["NoiDungGopY"];
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không để trống";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi2"] = "Vui lòng nhập địa chỉ";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi3"] = "Vui lòng nhập email";
            }
            else if (string.IsNullOrEmpty(noidunggopy))
            {
                ViewData["Loi2"] = "Vui lòng nhập nội dung góp ý";
            }
            else
            {
                gy.TenKH = hoten;
                gy.Email = email;
                gy.DiaChi = diachi;
                gy.NoiDungGopY = noidunggopy;
                db.Gopies.InsertOnSubmit(gy);
                db.SubmitChanges();
                return RedirectToAction("GopY");
            }
            ViewBag.Thongbao = "Đã gửi thành công";
            return this.GopY();
        }
    }
}