using Business.DTOs;
using Business.Interfaces;
using Data.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public class FileService : IFileService
    {

        private readonly IConfiguration configuration;
        public FileService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<FileResponseDTO> UploadFile(List<IFormFile> ListFiles)
        {

            FileResponseDTO fileResponse = new FileResponseDTO();

            foreach (var file in ListFiles)
            {
                if (file.FileName.Length > 100)
                {
                    throw new Exception("Length Exceeds");
                }

                if (file.Length > 52428800)
                {
                    throw new Exception("File Size Exceed");
                }

                string[] arrFile = file.FileName.Split(".");


                // Generate guid
                Guid obj = Guid.NewGuid();
                string guid = obj.ToString();


                // Append guid in filename before extension
                string fileName = arrFile[0] + "_" + guid + "." + arrFile[1];

                // Copy blob service rsp in project model
                FileItemResponse fileItemResponse = new FileItemResponse();
                fileItemResponse.FileName = fileName;
                fileItemResponse.FilePath = FileURL(fileName);
                fileResponse.ListFileResponse.Add(fileItemResponse);
            }

            return fileResponse;
        }
        public string FileURL(string filename)
        {
            var baseurl = configuration[CustomConstants.BaseUrl];
            var folderPath = "wwwroot/Images";
            var finalPath = string.Format("{0}/{1}/{2}", baseurl, folderPath, filename);
            return finalPath;
        }

        public bool DeleteFile(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", filename);
            if (!System.IO.File.Exists(path))
            {
                return false;
            }
            System.IO.File.Delete(path);
            return true;

        }
    }
}
