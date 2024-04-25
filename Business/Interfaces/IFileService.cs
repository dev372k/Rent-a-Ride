using Business.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFileService
    {
        Task<FileResponseDTO> UploadFile(IFormFile file);
        string FileURL(string filename);
        bool DeleteFile(string filename);
    }
}
