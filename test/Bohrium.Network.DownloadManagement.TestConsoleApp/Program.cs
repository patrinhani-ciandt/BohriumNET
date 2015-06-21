using Bohrium.Network.DownloadManagement.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bohrium.Network.DownloadManagement.TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadManager = new DownloadManager();

            ProtocolProviderFactory.RegisterProtocolHandler("http", typeof(HttpProtocolProvider));

            var regularHttpDownload = new Downloader(new ResourceLocation()
            {
                URL = "http://localhost/downloads/vs2015.preview_ult_ENU.iso",
            }, null, @"C:\TMP\vs2015.preview_ult_ENU.iso", 0);

            downloadManager.Add(regularHttpDownload, false);

            regularHttpDownload.Start();

            Console.WriteLine("Pressione <ENTER> para finalizar...");
            Console.ReadLine();
        }
    }
}
