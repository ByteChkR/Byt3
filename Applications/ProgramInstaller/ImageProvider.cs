using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ProgramInstaller.Properties;

namespace ProgramInstaller
{
    public class ImageProvider
    {
        private Dictionary<string, string> productMap;
        private ConcurrentDictionary<string, Image> productIcons;

        public ImageProvider(Dictionary<string, string> productMap)
        {
            this.productMap = productMap;
            productIcons = new ConcurrentDictionary<string, Image>();
            productMap.ToList().ForEach(x => productIcons.TryAdd(x.Key, Resources.defaultImage));
            
            Thread t = new Thread(DownloadImages);
            t.Start();
        }

        public Image GetImage(string productName)
        {
            if (productIcons.ContainsKey(productName)) return productIcons[productName];
            return Resources.defaultImage;
        }

        private void DownloadImages()
        {
            foreach (KeyValuePair<string, string> keyValuePair in productMap)
            {
                Image img = DownloadImage(keyValuePair.Value);
                productIcons.AddOrUpdate(keyValuePair.Key, img, (s, image) => img);
            }
        }

        private Image DownloadImage(string url)
        {
            WebClient client = new WebClient();
            try
            {
                byte[] data = client.DownloadData(url + "/Icon.png");
                client.Dispose();
                Image img = Image.FromStream(new MemoryStream(data));
                return img;
            }
            catch (Exception e)
            {
                client.Dispose();
                return Resources.defaultImage;
            }

        }
    }
}