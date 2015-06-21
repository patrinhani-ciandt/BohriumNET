using System;
using System.Collections.Generic;
using System.Text;

namespace Bohrium.Network.DownloadManagement
{
    public enum SegmentState
    {
        Idle,
        Connecting,
        Downloading,
        Paused,
        Finished,
        Error,
    }
}
