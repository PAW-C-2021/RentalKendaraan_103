using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Models.Upload
{
    public class FileDetails
    {
        public string None { get; set; }

        public string Path { get; set; }
        public string Name { get; internal set; }
    }

    public class FilesViewModel
    {
        public List<FileDetails> Files { get; set; }
                   = new List<FileDetails>();
    }
}
