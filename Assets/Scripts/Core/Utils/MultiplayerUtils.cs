using System;
using System.Collections.Generic;
using UdpKit;
using UdpKit.Platform.Photon;

namespace Core
{
    public static class MultiplayerUtils
    {
        public static readonly IReadOnlyList<PhotonRegion> AvailableRegions = new List<PhotonRegion>
        {
            PhotonRegion.regions[PhotonRegion.Regions.EU],
            PhotonRegion.regions[PhotonRegion.Regions.ASIA],
            PhotonRegion.regions[PhotonRegion.Regions.CAE],
            PhotonRegion.regions[PhotonRegion.Regions.IN],
            PhotonRegion.regions[PhotonRegion.Regions.JP],
            PhotonRegion.regions[PhotonRegion.Regions.RU],
            PhotonRegion.regions[PhotonRegion.Regions.RUE],
            PhotonRegion.regions[PhotonRegion.Regions.SA],
            PhotonRegion.regions[PhotonRegion.Regions.KR],
            PhotonRegion.regions[PhotonRegion.Regions.US],
            PhotonRegion.regions[PhotonRegion.Regions.USW],
            PhotonRegion.regions[PhotonRegion.Regions.AU]
        };

        public static readonly List<string> AvailableRegionDescriptions = new()
        {
            $"{PhotonRegion.regions[PhotonRegion.Regions.EU].Name} {PhotonRegion.regions[PhotonRegion.Regions.EU].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.ASIA].Name} {PhotonRegion.regions[PhotonRegion.Regions.ASIA].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.CAE].Name} {PhotonRegion.regions[PhotonRegion.Regions.CAE].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.IN].Name} {PhotonRegion.regions[PhotonRegion.Regions.IN].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.JP].Name} {PhotonRegion.regions[PhotonRegion.Regions.JP].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.RU].Name} {PhotonRegion.regions[PhotonRegion.Regions.RU].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.RUE].Name} {PhotonRegion.regions[PhotonRegion.Regions.RUE].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.SA].Name} {PhotonRegion.regions[PhotonRegion.Regions.SA].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.KR].Name} {PhotonRegion.regions[PhotonRegion.Regions.KR].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.US].Name} {PhotonRegion.regions[PhotonRegion.Regions.US].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.USW].Name} {PhotonRegion.regions[PhotonRegion.Regions.USW].City}",
            $"{PhotonRegion.regions[PhotonRegion.Regions.AU].Name} {PhotonRegion.regions[PhotonRegion.Regions.AU].City}",
        };

        public static DisconnectReason ToDisconnectReason(this UdpConnectionDisconnectReason udpReason)
        {
            return udpReason switch
            {
                UdpConnectionDisconnectReason.Unknown => DisconnectReason.Unknown,
                UdpConnectionDisconnectReason.Timeout => DisconnectReason.Timeout,
                UdpConnectionDisconnectReason.Error => DisconnectReason.Error,
                UdpConnectionDisconnectReason.Disconnected => DisconnectReason.Disconnected,
                _ => DisconnectReason.Unknown,
            };
        }

        public static ClientConnectFailReason ToConnectFailReason(this ConnectRefusedReason refusedReason)
        {
            return refusedReason switch
            {
                ConnectRefusedReason.None => ClientConnectFailReason.ServerRefusedConnection,
                ConnectRefusedReason.InvalidToken => ClientConnectFailReason.InvalidToken,
                ConnectRefusedReason.InvalidVersion => ClientConnectFailReason.InvalidVersion,
                ConnectRefusedReason.UnsupportedDevice => ClientConnectFailReason.UnsupportedDevice,
                _ => throw new ArgumentOutOfRangeException(nameof(refusedReason), refusedReason, "Unknown refuse reason!"),
            };
        }
    }
}
