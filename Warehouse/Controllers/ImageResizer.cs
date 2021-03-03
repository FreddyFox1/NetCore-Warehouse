using ImageMagick;
using System.IO;

namespace Warehouse.Controllers
{
    /// <summary>
    /// Класс предназначен для обработки изображений загружаемых на сервер, 
    /// а именно сжатие их и уменьшения разрешения для экономия места на 
    /// сервер и быстрой загрузки их на мобильных устройствах
    /// </summary>
    public class ImageResizer
    {
        #region Const
        //Vars for icons
        private const int s_size = 250;
        private const int s_quality = 100;
        //Vars for large images
        private const int l_size = 600;
        private const int l_quality = 75;
        #endregion

        public static void Resizer(string InputPath, string FileName, string File)
        {
            using (var photo = new MagickImage(File))
            {
                photo.AutoOrient();

                photo.Resize(l_size, 0);
                photo.Quality = l_quality;
                photo.Strip();
                var Photo = Path.Combine(InputPath, "Photo", FileName);
                photo.Write(Photo);

                photo.Resize(s_size, 0);
                photo.Quality = s_quality;
                photo.Strip();
                var Icon = Path.Combine(InputPath, "Icons", FileName);
                photo.Write(Icon);
            }
            System.IO.File.Delete(File);
        }


    }
}

