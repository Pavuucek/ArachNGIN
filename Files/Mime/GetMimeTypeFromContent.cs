using System.IO;
using System.Linq;

namespace ArachNGIN.Files.Mime
{
    /// <summary>
    ///     Class to find a file mime type by content
    /// </summary>
    public class GetMimeTypeFromContent
    {
        private static readonly byte[] Bmp = {66, 77};
        private static readonly byte[] Doc = {208, 207, 17, 224, 161, 177, 26, 225};
        private static readonly byte[] ExeDll = {77, 90};
        private static readonly byte[] Gif = {71, 73, 70, 56};
        private static readonly byte[] Ico = {0, 0, 1, 0};
        private static readonly byte[] Jpeg = {255, 216, 255};
        private static readonly byte[] Mp3 = {255, 251, 48};
        private static readonly byte[] Ogg = {79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0};
        private static readonly byte[] Pdf = {37, 80, 68, 70, 45, 49, 46};
        private static readonly byte[] Png = {137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82};
        private static readonly byte[] Rar = {82, 97, 114, 33, 26, 7, 0};
        private static readonly byte[] Swf = {70, 87, 83};
        private static readonly byte[] Tiff = {73, 73, 42, 0};
        private static readonly byte[] Torrent = {100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101};
        private static readonly byte[] Ttf = {0, 1, 0, 0, 0};
        private static readonly byte[] WavAvi = {82, 73, 70, 70};
        private static readonly byte[] WmvWma = {48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108};
        private static readonly byte[] ZipDocx = {80, 75, 3, 4};

        /// <summary>
        ///     Gets the mime type by contents
        /// </summary>
        /// <param name="fileName">Name of the file as string</param>
        /// <returns>mime type as string</returns>
        public static string GetMimeType(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            return GetMimeType(bytes);
        }

        /// <summary>
        ///     Gets the mime type by contents
        /// </summary>
        /// <param name="file">The file as byte array</param>
        /// <returns>mime type as string</returns>
        public static string GetMimeType(byte[] file /*, string fileName*/)
        {
            string mime = "application/octet-stream"; //DEFAULT UNKNOWN MIME TYPE

            /*//Ensure that the filename isn't empty or null
            if (string.IsNullOrEmpty(fileName))
            {
                return mime;
            }

            //Get the file extension
            string extension = Path.GetExtension(fileName) == null
                                   ? string.Empty
                                   : Path.GetExtension(fileName).ToUpper();
            */
            //Get the MIME Type
            if (file.Take(2).SequenceEqual(Bmp))
            {
                mime = "image/bmp";
            }
            else if (file.Take(8).SequenceEqual(Doc))
            {
                mime = "application/msword";
            }
            else if (file.Take(2).SequenceEqual(ExeDll))
            {
                mime = "application/x-msdownload"; //both use same mime type
            }
            else if (file.Take(4).SequenceEqual(Gif))
            {
                mime = "image/gif";
            }
            else if (file.Take(4).SequenceEqual(Ico))
            {
                mime = "image/x-icon";
            }
            else if (file.Take(3).SequenceEqual(Jpeg))
            {
                mime = "image/jpeg";
            }
            else if (file.Take(3).SequenceEqual(Mp3))
            {
                mime = "audio/mpeg";
            }
            else if (file.Take(14).SequenceEqual(Ogg))
            {
                /*switch (extension)
                {
                    case ".OGX":
                        mime = "application/ogg";
                        break;
                    case ".OGA":
                        mime = "audio/ogg";
                        break;
                    default:
                        mime = "video/ogg";
                        break;
                }*/
                mime = "video/ogg";
            }
            else if (file.Take(7).SequenceEqual(Pdf))
            {
                mime = "application/pdf";
            }
            else if (file.Take(16).SequenceEqual(Png))
            {
                mime = "image/png";
            }
            else if (file.Take(7).SequenceEqual(Rar))
            {
                mime = "application/x-rar-compressed";
            }
            else if (file.Take(3).SequenceEqual(Swf))
            {
                mime = "application/x-shockwave-flash";
            }
            else if (file.Take(4).SequenceEqual(Tiff))
            {
                mime = "image/tiff";
            }
            else if (file.Take(11).SequenceEqual(Torrent))
            {
                mime = "application/x-bittorrent";
            }
            else if (file.Take(5).SequenceEqual(Ttf))
            {
                mime = "application/x-font-ttf";
            }
            else if (file.Take(4).SequenceEqual(WavAvi))
            {
                //mime = extension == ".AVI" ? "video/x-msvideo" : "audio/x-wav";
                mime = "video/x-msvideo";
            }
            else if (file.Take(16).SequenceEqual(WmvWma))
            {
                // mime = extension == ".WMA" ? "audio/x-ms-wma" : "video/x-ms-wmv";
                mime = "video/x-ms-wmv";
            }
            else if (file.Take(4).SequenceEqual(ZipDocx))
            {
                //mime = extension == ".DOCX" ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document" : "application/x-zip-compressed";
                mime = "application/x-zip-compressed";
            }

            return mime;
        }
    }
}