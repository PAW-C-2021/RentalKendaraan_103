using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Customer = new HashSet<Customer>();
        }

        public int IdGender { get; set; }

        [MaxLength(1, ErrorMessage = "Gender tidak boleh lebih dari 1")]
        public string NamaGender { get; set; }

        public ICollection<Customer> Customer { get; set; }
    }
}
