using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using ImageMagick;

namespace GiftMaker.Models
{
    public class GiftModel
    {
        public string Message { get; set; }
        public string GiftUrl { get; set; }
        public string FolderName { get; set; }
        private string InputPath { get; set; }
        private string OutputPath { get; set; }

        private const int MaxCharaters = 7;

        public GiftModel(string message)
        {
            Message = message;
            InputPath = HostingEnvironment.ApplicationPhysicalPath;
            OutputPath = $"{HostingEnvironment.ApplicationPhysicalPath}Words/Output";
            FolderName = message.ToLower().Replace(" ", string.Empty);
        }

        public void GenerateGif()
        {
            var words = Message.Split(' ');
            MergeImage("1", words);
            MergeImage("2", words);
            MergeImage("3", words);

            using (MagickImageCollection collection = new MagickImageCollection())
            {
                MagickImageCollection open = new MagickImageCollection("~/Words/open.gif");
                foreach (MagickImage image in open)
                {
                    //image.Resize(700, 550);
                    collection.Add(image);

                }
                foreach (string file in Directory.EnumerateFiles($"{OutputPath}/{FolderName}", "*.png"))
                {
                    MagickImage image = new MagickImage(file) { AnimationDelay = 50};
                    collection.Add(image);
                }

                MagickImageCollection close = new MagickImageCollection("~/Words/close.gif");
                foreach (MagickImage image in close)
                {
                    //image.Resize(700, 550);
                    collection.Add(image);
                }

                QuantizeSettings settings = new QuantizeSettings { Colors = 256 };
                collection.Quantize(settings);
                collection.OptimizeTransparency();
                
                collection.Write($"{OutputPath}/{FolderName}.gif");
            }
            GiftUrl = $"/Words/Output/{FolderName}.gif";
        }

        private void MergeImage(string phase, string[] words)
        {
            var position = 1;

            using (MagickImageCollection images = new MagickImageCollection())
            {
                foreach (var line in words)
                {
                    foreach (var word in line)
                    {
                        MagickImage image = new MagickImage($"~/Words/{word}/{word}-{phase}.png");
                        //image.Extent(90, 400, position == 1 ? Gravity.Center : Gravity.South);
                        image.Extent(-5, -90 * position, 90, 400);
                        //image.Resize(500, 350);
                        image.Transparent(new MagickColor("#ffffff"));
                        images.Add(image);
                    }

                    position++;
                }

                using (MagickImage horizontal = images.AppendHorizontally())
                {
                    MagickImage background = new MagickImage("~/Words/background.png");
                    horizontal.Composite(background, CompositeOperator.DstAtop);

                    Directory.CreateDirectory($"{OutputPath}/{FolderName}");
                    horizontal.Write($"{OutputPath}/{FolderName}/{FolderName}-{phase}.png");
                    background.Dispose();
                }
            }
        }
    }
}