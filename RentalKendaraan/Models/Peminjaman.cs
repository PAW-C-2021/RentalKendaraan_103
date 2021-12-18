using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan.Models
{
    public partial class Peminjaman
    {
        public Peminjaman()
        {
            Pengembalian = new HashSet<Pengembalian>();
        }

        public int IdPinjaman { get; set; }

        [Required(ErrorMessage = "Tanggal Peminjaman Tidak Boleh Kosong")]
        public DateTime? TglPeminjaman { get; set; }

        public int? IdKendaraan { get; set; }

        public int? IdCustomer { get; set; }

        public int? IdJaminan { get; set; }

        public int? Biaya { get; set; }

        public Customer IdCustomerNavigation { get; set; }
        public Jaminan IdJaminanNavigation { get; set; }
        public Kendaraan IdKendaraanNavigation { get; set; }
        public ICollection<Pengembalian> Pengembalian { get; set; }
    }
}
