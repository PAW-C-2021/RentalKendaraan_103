using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan.Models
{
    public partial class Kendaraan
    {
        public Kendaraan()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdKendaraan { get; set; }

        [Required(ErrorMessage = "Nama Kendaraan Tidak Boleh Kosong")]
        public string NamaKendaraan { get; set; }

        [MinLength(6, ErrorMessage = "No Pol Minimal 6 angka")]
        [MaxLength(11, ErrorMessage = "No Pol Maksimal 11 angka")]
        public string NoPolisi { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public string NoStnk { get; set; }

        public int? IdJenisKendaraan { get; set; }

        public string Ketersediaan { get; set; }

        public JenisKendaraan IdJenisKendaraanNavigation { get; set; }
        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
