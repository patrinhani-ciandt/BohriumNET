using System;
using System.Collections.Generic;
using System.Text;

namespace Bohrium.Network.DownloadManagement
{
    public interface ISegmentCalculator
    {
        CalculatedSegment[] GetSegments(int segmentCount, RemoteFileInfo fileSize);
    }
}
