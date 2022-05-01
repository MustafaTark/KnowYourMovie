using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IMDB2.Data
{
    public class Upload
    {
        public static byte[] UploadImageInDataBase(HttpPostedFileBase file)
        {
            //file.InputStream.Read(movieInDb.Image, 0, file.ContentLength);
            var stream = file.InputStream;
            var reader = new BinaryReader(stream);
            byte[] imageBytes = reader.ReadBytes(file.ContentLength);
            return imageBytes;
        }
    }
}