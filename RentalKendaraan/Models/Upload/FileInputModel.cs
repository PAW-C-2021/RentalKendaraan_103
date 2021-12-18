using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Models.Upload
{
    public class FileInputModel
    {

        public IFormFile FileToUpload { get; set; }

    }
}
