using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using QuanLyBanHang.Configurations;

namespace QuanLyBanHang.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file.Length <= 0) return null;
            using var stream    = file.OpenReadStream();
            var uploadParams    = new ImageUploadParams
            {
                File            = new FileDescription(file.FileName, stream),
                Folder          = "uploads"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                Console.WriteLine("❌ PublicId trống, không thể xóa ảnh.");
                return false;
            }

            var deleteParams    = new DeletionParams(publicId);
            var result          = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Result == "ok")
            {
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Xóa ảnh thất bại: {result.Result}");
                return false;
            }
        }

        public async Task<List<dynamic>> GetAllImagesAsync()
        {
            var list = new List<dynamic>();

            var searchResult = await _cloudinary.Search()
                .Expression("resource_type:image AND folder=uploads") 
                .SortBy("created_at", "desc")
                .MaxResults(30) 
                .ExecuteAsync();

            if (searchResult.Resources != null)
            {
                foreach (var item in searchResult.Resources)
                {
                    list.Add(new {
                        url = item.SecureUrl,
                        publicId = item.PublicId
                    });
                }
            }
            return list;
        }

        public async Task<bool> RenameImageAsync(RenameParams renameParams)
        {
            var result = await _cloudinary.RenameAsync(renameParams);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Lỗi đổi tên ảnh trên Cloudinary: {result.Error.Message}");
                return false;
            }
        }

        public async Task<string?> UploadImageWithPublicIdAsync(IFormFile file, string publicId)
        {
            if (file.Length <= 0) return null;

            using var stream    = file.OpenReadStream();
            var uploadParams    = new ImageUploadParams
            {
                File            = new FileDescription(file.FileName, stream),
                PublicId        = publicId, // Giữ nguyên PublicId để ghi đè ảnh cũ
                Overwrite       = true, // Cho phép ghi đè ảnh cũ
                Invalidate      = true // Xóa cache để ảnh mới hiển thị ngay lập tức
            };
            var result          = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
    }
}
