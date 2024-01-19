using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class UploadClass
    {
        public static string UploadPhoto(Stream stream)
        {
            Account account = new Account(
             "dmj7rfzxk",
              "352575888163614",
             "PHLg9yzpjriHR8czMQkghI7KbIg");
            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), stream),
            };

            ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            return cloudinary.Api.UrlImgUp.BuildUrl($"{uploadResult.PublicId}.{uploadResult.Format}");
        }
    }

}

