using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class FileResponseDTO
    {
        public FileResponseDTO()
        {
            ListFileResponse = new List<FileItemResponse>();
        }
        public List<FileItemResponse> ListFileResponse { get; set; }
    }
    public class FileItemResponse
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
