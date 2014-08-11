using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sign
{
    public class DocumentImageController : ApiController
    {
        public HttpResponseMessage Post([FromBody] string imageData)
        {

            byte[] bytes = System.Convert.FromBase64String(imageData);
            using (var ms = new System.IO.MemoryStream(bytes))
            {
                var signatureImage = System.Drawing.Image.FromStream(ms);

                var imagesFileFolderPath = @"C:\ThatConf\2014\Sign\Sign\Images\";

                var documentImage = System.Drawing.Image.FromFile(imagesFileFolderPath + "ImportantDocument.png");

                // Merge the two documents into one
                var mergedImage = MergeSignature(signatureImage, documentImage);

                // Get the hash
                var imageHash = HashSignature(mergedImage);

                // Save the image to disk
                mergedImage.Save(imagesFileFolderPath + imageHash + ".png");

                var response = Request.CreateResponse(HttpStatusCode.OK);

                response.Headers.Add("imageFilename", imageHash);
                return response;
            }

        }


        private Image MergeSignature(Image sigImage, Image docImage)
        {

            // Create a new empty bitmap
            var bitmap = new Bitmap(720, 480);

            // Set up transparency color mapping
            var colorMap = new System.Drawing.Imaging.ColorMap[1];
            colorMap[0] = new System.Drawing.Imaging.ColorMap();
            colorMap[0].OldColor = Color.White;
            colorMap[0].NewColor = Color.Transparent;
            var attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetRemapTable(colorMap);

            // Execute the merge
            using (Graphics grfx = Graphics.FromImage(bitmap))
            {
                grfx.DrawImage(docImage, 0, 0);
                grfx.DrawImage(sigImage,
                    new Rectangle(200, 200, sigImage.Width, sigImage.Height),
                    0,
                    0,
                    sigImage.Width,
                    sigImage.Height,
                    GraphicsUnit.Pixel,
                    attr);
                grfx.Save();
            }
            return (Image)bitmap;
        }

        private string HashSignature(Image image)
        {
            // Get the RGB for the image
            int[] rgbArray = new int[345600];
            ((Bitmap)image).getRGB(0, 0, 720, 480, rgbArray, 0, 720);

            // Get bytes from the RGB
            byte[] bytes = new byte[rgbArray.Length * sizeof(int)];
            System.Buffer.BlockCopy(rgbArray, 0, bytes, 0, bytes.Length);

            // Hash the bytes
            var hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = hasher.ComputeHash(bytes);

            // Convert the hashed byte array to a string
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                // "x2" is the string format for hexadecimal
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }


}
