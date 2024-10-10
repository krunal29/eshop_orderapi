using IfourTechnolab.Business.ViewModels.General;
using Microsoft.AspNetCore.Http;
using eshop_orderapi.Business.Enums.General;
using System;
using System.IO;
using System.Threading.Tasks;

namespace eshop_orderapi.Business.Helpers
{
    public class FileHelper : IFileHelper
    {
        private static string UploadShare = "uploads";

        public FileHelper()
        {
        }

        public async Task<bool> SaveFile(IFormFile file, UploadDirectories directory)
        {
            try
            {
                await DeleteFile(directory, file.FileName);

                CheckAndCreateDirectory($"{FullFileServerMapPath(directory)}");

                SaveFile(file, $"{FullFileServerMapPath(directory)}{file.FileName}");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private async void SaveFile(IFormFile file, string pathAndFilename)
        {
            using (FileStream stream = new FileStream(pathAndFilename, FileMode.Create))
            {
                stream.Position = 0;
                await file.CopyToAsync(stream);
            }
        }

        public async Task<UploadfileModel> SaveFile(UploadfileModel fileModel, IFormFile file, UploadDirectories directory, Guid? identifier = null)
        {
            if (identifier == null || identifier == Guid.Empty)
            {
                identifier = Guid.NewGuid();
            }
            else
            {
                await DeleteFile(directory, identifier.ToString());
            }
            CheckAndCreateDirectory($"{FullFileServerMapPath(directory)}");
            var filepath = $"{FullFileServerMapPath(directory)}{identifier}";

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                using (FileStream fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    ms.WriteTo(fileStream);
                }
                fileModel.FileGuId = identifier.Value;
            }
            return fileModel;
        }

        public string FullFileServerMapPath(UploadDirectories directory)
        {
            var contentPath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\");
            return Path.Combine(contentPath + UploadFilePath(directory));
        }

        public string FullFileApiServerMapPath(UploadDirectories directory)
        {
            var contentPath = Path.Combine(Directory.GetCurrentDirectory() + "\\" + UploadShare + "\\");
            CheckAndCreateDirectory($"{contentPath + UploadFilePath(directory)}");
            return Path.Combine(contentPath + UploadFilePath(directory));
        }

        private string UploadFilePath(UploadDirectories directory)
        {
            var folder = Enum.GetName(typeof(UploadDirectories), directory);
            if (!folder.StartsWith("\\"))
            {
                folder = "\\" + folder;
            }

            if (!folder.EndsWith("\\"))
            {
                folder += "\\";
            }
            return UploadShare + folder;
        }

        public void CheckAndCreateDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public async Task<bool> DeleteFile(UploadDirectories directory, string fileName)
        {
            return await DeleteFile($"{FullFileServerMapPath(directory)}{fileName}");
        }

        private async Task<bool> DeleteFile(string pathAndFilename)
        {
            try
            {
                if (File.Exists(pathAndFilename))
                {
                    File.Delete(pathAndFilename);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void WriteLine(string fileName, string text, bool includeDate = true)
        {
            using (TextWriter tw = new StreamWriter(fileName, true))
            {
                if (includeDate)
                {
                    tw.WriteLine("{0} - {1}", DateTime.UtcNow, text);
                }
                else
                {
                    tw.WriteLine(text);
                }
                tw.Close();
            }
        }
    }
}