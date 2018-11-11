using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FaceAppApi
{
    public interface IFaceAppClient
    {
        List<string> GetAvailableFilters();

        string UploadImage(string filePath);

        BitmapImage GetFiterImage(string code, string filter);
    }
}