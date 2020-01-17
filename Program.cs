using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ColorTranslator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            var filePath = GetPath();
            TranslateImage(filePath);
            Console.WriteLine("Hello World!");
        }

        static string GetPath()
        {
            using var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "ALL FILES (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            
            return GetPath();
        }

        static string SaveFileDialog()
        {
            using var saveFileDialog1 = new SaveFileDialog
            {
                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                return saveFileDialog1.FileName;
            }

            return SaveFileDialog();
        }

        static void TranslateImage(string imagePath)
        {
            Image image = Image.FromFile(imagePath); //new Bitmap(imagePath);
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = image.Width;
            int height = image.Height;

            float[][] colorMatrixElements = {
               new float[] {1.1f,  0,  0f,  0, 0},      // red scaling factor of 1
               new float[] {0,  0.7f,  0,  0, 0},      // green scaling factor of 1
               new float[] {0,  0,  1f,  0, 0},      // blue scaling factor of 1
               new float[] {0,  0,  0,  1, 0},      // alpha scaling factor of 1
               new float[] {0f,  0f,  0,  0, 1}};     // three translations of 0.2

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            var graphics = Graphics.FromImage(image);
            graphics.DrawImage(
               image,
               new Rectangle(0, 0, width, height),  // destination rectangle 
               0, 0,        // upper-left corner of source rectangle 
               width,       // width of source rectangle
               height,      // height of source rectangle
               GraphicsUnit.Pixel,
               imageAttributes);

            var newPath = SaveFileDialog();
            image.Save(newPath);
        }
    }
}
