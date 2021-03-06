﻿using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FaceAppApi
{
    public interface IFaceAppClient
    {
        List<string> GetAvailableFilters();

        string UploadImage(string filePath);

        BitmapSource GetFiterImage(string code, string filter);

        event EventHandler<string> ErrorOccurred;
    }
}