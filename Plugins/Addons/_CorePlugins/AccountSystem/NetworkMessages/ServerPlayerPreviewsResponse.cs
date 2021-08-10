//BY DX4D

namespace OpenMMO.Network
{    // @Server -> @Client
    /// <summary>
    /// Based on <see cref="ServerResponse"/> - Inherits from <see cref="NetworkTransaction"/>.
    /// </summary>
    public interface ServerPlayerPreviewsResponse : ServerResponse
    {
        PlayerPreview[] players { get; set; }
        int maxPlayers { get; set; }
    }
}
