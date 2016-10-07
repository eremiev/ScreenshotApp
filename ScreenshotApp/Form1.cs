using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ScreenshotApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create screenshoot
            Bitmap screenshot = new Bitmap(SystemInformation.VirtualScreen.Width,
                               SystemInformation.VirtualScreen.Height,
                               PixelFormat.Format32bppArgb);

            Graphics screenGraph = Graphics.FromImage(screenshot);

            screenGraph.CopyFromScreen(SystemInformation.VirtualScreen.X,
                                       SystemInformation.VirtualScreen.Y,
                                       0,
                                       0,
                                       SystemInformation.VirtualScreen.Size,
                                       CopyPixelOperation.SourceCopy);

            //convert to base64
            var base64 = Bitmap2Base64(screenshot);

            //convert from base64 to byte array
            var byteImage = Base64ToBitmap(base64);


            //convert from byte array to Image
            Image img;
            using (var ms = new MemoryStream(byteImage))
            {
                img = Image.FromStream(ms);
            }

            //present image on windows form
            img = img.GetThumbnailImage(pictureBox1.Size.Width, pictureBox1.Size.Height, null, IntPtr.Zero);
            pictureBox1.Image = img;
        }

        private string Bitmap2Base64(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] arr = new byte[ms.Length];

            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();

            string strBase64 = Convert.ToBase64String(arr);

            return strBase64;
        }

        private byte[] Base64ToBitmap(string image)
        {
            byte[] byteImage = Convert.FromBase64String(image);
            return byteImage;
        }
    }
}
