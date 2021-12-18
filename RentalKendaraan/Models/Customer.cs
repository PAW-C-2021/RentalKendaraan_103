using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan.Models
{

    public partial class Customer
    {
        public Customer()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdCustomer { get; set; }

        [Required(ErrorMessage = "Nama Customer Tidak Boleh Kosong")]
        public string NamaCustomer { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage ="Hanya boleh diisi oleh angka")]
        public string Nik { get; set; }

        public string Alamat { get; set; }

        [MinLength(10, ErrorMessage ="No Hp Minimal 10 angka")]
        [MaxLength(13, ErrorMessage = "No Hp Maksimal 13 angka")]
        public string NoHp { get; set; }

        public int? IdGender { get; set; }

        public Gender IdGenderNavigation { get; set; }
        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
