using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan.Models
{
    public partial class Pengembalian
    {
        public int IdPengembalian { get; set; }

        [Required(ErrorMessage = "Tanggal Pengembalian Tidak Boleh Kosong")]
        public DateTime? TglPengembalian { get; set; }

        public int? IdPeminjaman { get; set; }

        public int? IdKondisi { get; set; }

        public int? Denda { get; set; }

        public KondisiKendaraan IdKondisiNavigation { get; set; }
        public Peminjaman IdPeminjamanNavigation { get; set; }
    }
}
