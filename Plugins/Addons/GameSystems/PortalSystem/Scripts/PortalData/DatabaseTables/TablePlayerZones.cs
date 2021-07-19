//BY FHIZ
//MODIFIED BY DX4D

namespace OpenMMO.Database
{
    //PLAYER ZONES TABLE
    public partial class TablePlayerZones
    {
        [PrimaryKey]
        public string playername { get; set; }
        public string zonename { get; set; }
        public string anchorname { get; set; }
        public bool startpos { get; set; }
        public int token { get; set; }

        /// <summary>Checks if the token is valid</summary>
        /// <param name="_token">The token must match the existing token to be valid.</param>
        /// <returns></returns>
        public bool ValidateToken(int _token)
        {
            return (token == _token);
        }
    }
    //INITIALIZE TABLE [hook]
    public partial class DatabaseManager
    {
        [DevExtMethods(nameof(Init))]
        void Init_PlayerZones()
        {
            CreateTable<TablePlayerZones>();
        }
    }
}