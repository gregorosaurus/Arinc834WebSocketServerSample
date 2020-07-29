using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARINC834WebSocketsSample
{
    public interface IARINCDataSource
    {
        Task<List<A834DataParameter>> GetCurrentDataAsync();
    }
}
