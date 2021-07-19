//BY FHIZ
//MODIFIED BY DX4D

using System;

namespace OpenMMO.Database
{
    //NETWORK ZONES TABLE
    public partial class TableNetworkZones
    {
        [PrimaryKey]
        public string zone { get; set; }
        public DateTime online { get; set; }
    }
    //INITIALIZE TABLE [hook]
    public partial class DatabaseManager
    {
        [DevExtMethods(nameof(Init))]
        void Init_NetworkZones()
        {
            CreateTable<TableNetworkZones>();
        }
    }
}