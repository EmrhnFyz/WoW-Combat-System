﻿using Bolt;
using Bolt.Utils;
using System.Collections.Generic;
using UdpKit;
using UnityEngine;

namespace Core
{
    public sealed class SpellProcessingToken : IProtocolToken
    {
        public readonly List<(ulong, int)> ProcessingEntries = new();
        public int ServerFrame { get; internal set; }
        public Vector3 Destination { get; internal set; }
        public Vector3 Source { get; internal set; }

        public void Read(UdpPacket packet)
        {
            Destination = packet.ReadVector3();
            Source = packet.ReadVector3();
            ServerFrame = packet.ReadInt();
            var count = packet.ReadInt();
            for (var i = 0; i < count; i++)
            {
                ProcessingEntries.Add((packet.ReadULong(), packet.ReadInt()));
            }
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteVector3(Destination);
            packet.WriteVector3(Source);
            packet.WriteInt(ServerFrame);
            packet.WriteInt(ProcessingEntries.Count);
            for (var i = 0; i < ProcessingEntries.Count; i++)
            {
                packet.WriteULong(ProcessingEntries[i].Item1);
                packet.WriteInt(ProcessingEntries[i].Item2);
            }
        }
    }
}
