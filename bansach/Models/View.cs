using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bansach.Models
{
    public class View
    {
        public KhachHang kh { get; set; }
        public CT_DonDatHang cthd { get; set; }
        public DonDatHang hd { get; set; }
        public TheLoai tl { get; set; }
        public Sach sach { get; set; }
        [DisplayFormat(DataFormatString ="{0:0.##0}")]
        public double ThanhTien { get; set; }   
    }
}