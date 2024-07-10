using EggLink.DanhengServer.Util;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet
{
    public class BasePacket(ushort cmdId)
    {
        private const uint HEADER_CONST = 0x9d74c714;
        private const uint TAIL_CONST = 0xd7a152c8;

        public ushort CmdId { get; set; } = cmdId;
        public byte[] Data { get; set; } = [];

        public void SetData(byte[] data)
        {
            Data = data;
        }

        public void SetData(IMessage message)
        {
            Data = message.ToByteArray();
        }

        public byte[] BuildPacket()
        {
            using MemoryStream? ms = new();
            using BinaryWriter? bw = new(ms);

            bw.WriteUInt32BE(HEADER_CONST);
            bw.WriteUInt16BE(CmdId);
            bw.WriteUInt16BE(0);
            bw.WriteUInt32BE((uint)Data.Length);
            if (Data.Length > 0)
            {
                bw.Write(Data);
            }
            bw.WriteUInt32BE(TAIL_CONST);

            byte[] packet = ms.ToArray();

            return packet;
        }
    }
}
