using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenNETCF.Drawing;
using OpenNETCF.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImagePanel
{
    /*
            OpenNETCF.Drawing.Imaging.StreamOnFile m_stream;
            Size m_size;
            /// <summary>
            /// this will handle also large bitmaps and show a thumbnailed version on a picturebox
            /// see http://blog.opennetcf.com/ctacke/2010/10/13/LoadingPartsOfLargeImagesInTheCompactFramework.aspx
            /// </summary>
            /// <param name="sFileName">the name of the file to load</param>
            private void showImage(string sFileName)
            {
                var stream = File.Open(sFileName, FileMode.Open);
                m_stream = new StreamOnFile(stream);
                m_size = ImageHelper.GetRawImageSize(m_stream);
                System.Diagnostics.Debug.WriteLine("showImage loading " + sFileName + ", width/height = " + m_size.Width.ToString() + "/"+ m_size.Height.ToString());
                //CameraPreview.Image = ImageHelper.CreateThumbnail(m_stream, CameraPreview.Width, CameraPreview.Height);
                CameraSnapshot.Image = ImageHelper.CreateThumbnail(m_stream, CameraPreview.Width, CameraPreview.Height);
                showSnapshot(true); //show still image
                m_stream.Dispose();
                stream.Close();
            }
    */
    public static class ImageHelper
    {
        private static ImagingFactory m_factory;

        private static ImagingFactory GetFactory()
        {
            if (m_factory == null)
            {
                m_factory = new ImagingFactory();
            }

            return m_factory;
        }

        public static Bitmap CreateClip(StreamOnFile sof, int x, int y, int width, int height)
        {
            IBitmapImage original = null;
            IImage image = null;
            ImageInfo info;

            GetFactory().CreateImageFromStream(sof, out image);
            try
            {
                image.GetImageInfo(out info);

                GetFactory().CreateBitmapFromImage(image, info.Width, info.Height,
                    info.PixelFormat, InterpolationHint.InterpolationHintDefault, out original);

                try
                {
                    var ops = (IBasicBitmapOps)original;
                    IBitmapImage clip = null;

                    try
                    {
                        var rect = new RECT(x, y, x + width, y + height);
                        ops.Clone(rect, out clip, true);

                        return ImageUtils.IBitmapImageToBitmap(clip);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(clip);
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(original);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(image);
            }
        }

        public static Bitmap getScaledBitmap(StreamOnFile sof, int scalePercent)
        {
            IBitmapImage thumbnail;
            IImage image;
            ImageInfo info;
            decimal fScale = scalePercent / 100.0m; // do not remove the m specifier!

            GetFactory().CreateImageFromStream(sof, out image);
            try
            {
                image.GetImageInfo(out info);
                uint newWidth = (uint)(info.Width * fScale);
                uint newHeight = (uint)(info.Height * fScale);
                GetFactory().CreateBitmapFromImage(image, newWidth, newHeight,
                    info.PixelFormat, InterpolationHint.InterpolationHintDefault, out thumbnail);
                try
                {
                    return ImageUtils.IBitmapImageToBitmap(thumbnail);
                }
                finally
                {
                    Marshal.ReleaseComObject(thumbnail);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(image);
            }
        }

        public static Bitmap CreateThumbnail(StreamOnFile sof, int width, int height)
        {
            IBitmapImage thumbnail;
            IImage image;
            ImageInfo info;

            GetFactory().CreateImageFromStream(sof, out image);
            try
            {
                image.GetImageInfo(out info);

                GetFactory().CreateBitmapFromImage(image, (uint)width, (uint)height,
                    info.PixelFormat, InterpolationHint.InterpolationHintDefault, out thumbnail);
                try
                {
                    return ImageUtils.IBitmapImageToBitmap(thumbnail);
                }
                finally
                {
                    Marshal.ReleaseComObject(thumbnail);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(image);
            }
        }

        public static bool saveScaledBitmap(StreamOnFile sof, int width, int height, string sNewFile)
        {
            bool bRet = true;
            try
            {
                Bitmap bmp = CreateThumbnail(sof, width, height);
                bmp.Save(sNewFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception in saveScaledBitmap(): " + ex.Message);
                bRet = false;
            }
            return bRet;
        }

        public static Size GetRawImageSize(StreamOnFile sof)
        {
            IImage image;
            ImageInfo info;

            GetFactory().CreateImageFromStream(sof, out image);
            try
            {
                image.GetImageInfo(out info);

                return new Size((int)info.Width, (int)info.Height);
            }
            finally
            {
                Marshal.ReleaseComObject(image);
            }
        }
    }
}