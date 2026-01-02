using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.DTO
{
    public class UploadFileDTO
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public string FilePathFromSave { get; set; }
    }
}
