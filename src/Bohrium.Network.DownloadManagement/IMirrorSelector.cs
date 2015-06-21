using System;
using System.Collections.Generic;
using System.Text;

namespace Bohrium.Network.DownloadManagement
{
    public interface IMirrorSelector
    {
        void Init(Downloader downloader);

        ResourceLocation GetNextResourceLocation(); 
    }
}
