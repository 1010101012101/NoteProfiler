using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note_Profiler
{
    /// <summary>
    /// Helper functions used all accross the app
    /// </summary>
    class Utilities
    {
        #region Custom Cursors
        /// <summary>
        /// Used for custom cursors
        /// </summary>
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        /// <summary>
        /// Create a cursor from a bitmap resource
        /// </summary>
        /// <param name="bmp">Bitmap to use as cursor</param>
        /// <param name="xHotSpot">X coordinate on cursor for hotspot</param>
        /// <param name="yHotSpot">Y coordinate on cursor for hotspot</param>
        /// <returns></returns>
        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }
    #endregion

        /// <summary>
        /// Returns an image of just the note give an image of the entire page and the ntoe object.
        /// </summary>
        /// <param name="original">The original page image</param>
        /// <param name="which">The note to extract</param>
        /// <returns></returns>
        public static Image OriginalNote(Mat original, Note which)
        {
            Image<Rgba, byte> output = original.ToImage<Rgba, byte>();
            output.ROI = which.Rectangle;
            return output.Bitmap;
        }

        /// <summary>
        /// Returns a byte-array from an image which can be easily saved to the Database.
        /// </summary>
        /// <param name="imageIn">The image to convert</param>
        /// <returns>The byte array representing the image</returns>
        public static byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Png); //Make sure this says "ImageFormat.Png" and not "Gif" as I originally stupidly did.
            return ms.ToArray();
        }

        /// <summary>
        /// Returns an image from a byte-array that is easily loaded from the Database.
        /// </summary>
        /// <param name="byteArrayIn">The byte array to convert</param>
        /// <returns>The image decoded form the byte array</returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);         
            return returnImage;
        }

        /// <summary>
        /// Converts an Image object to a Mat object (which can be easily manipulated with EMGU/OpenCV)
        /// </summary>
        /// <param name="image">The image to convert</param>
        /// <returns>The MAT object</returns>
        public static Mat MatFromImage(Image image)
        {            
            Bitmap bmp = new Bitmap(image);
            Image<Rgba, byte> img = new Image<Rgba, byte>(bmp);
            return img.Mat;
        }

        /// <summary>
        /// DEPRECATED - given an Image object, opens a window displaying it.
        /// Used for debugging mostly, kept in for future debugging purposes.
        /// </summary>
        /// <param name="image">The image to display</param>
        public static void ShowImage(Image image)
        {
            Form newForm = new Form();
            PictureBox pb = new PictureBox();
            pb.Name = "pb";
            pb.Dock = DockStyle.Fill;
            pb.Image = image;
            newForm.Controls.Add(pb);
            newForm.ShowDialog();
        }

        /// <summary>
        /// Calcaultes the Hash of a file.
        /// Used for hashing images, which is why it's important to always keep a copy of the original image FILE in your session (and not the OBJECT, which seesm to hash differently every time).
        /// </summary>
        /// <param name="fileName">The path to the file to hash</param>
        /// <returns>A string representing the file's hash.</returns>
        public static string FileHash(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
        }
    }
}
