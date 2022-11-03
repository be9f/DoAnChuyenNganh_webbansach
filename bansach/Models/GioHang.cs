using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bansach.Models
{
    public class GioHang
    {
        DbBanSachDataContext data = new DbBanSachDataContext();
        public int iMaSach { get; set; }
        public string tTenSach { get; set; }
        public string tBiaSach { get; set; }
        public Double dDongGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien { get { return iSoLuong * dDongGia; } }

        public GioHang(int MaSach)
        {
            iMaSach = MaSach;
            Sach sach = data.Saches.Single(n => n.MaSach == iMaSach);
            tTenSach = sach.TenSach;
            tBiaSach = sach.BiaSach;
            dDongGia = double.Parse(sach.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}