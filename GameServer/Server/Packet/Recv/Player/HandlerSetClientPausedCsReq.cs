using System;
using System.IO;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Server.Packet.Send.Others;
namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SetClientPausedCsReq)]
    public class HandlerSetClientPausedCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SetClientPausedCsReq.Parser.ParseFrom(data);
            var paused = req.Paused;
            connection.SendPacket(new PacketSetClientPausedScRsp(paused));

            // DO NOT REMOVE THIS CODE
            // This code is responsible for sending the client data to the player
            switch (ConfigManager.Config.ServerOption.Language)
            {
                case "CHS":
                    connection.SendPacket(new BasePacket(5)
                    {
                        Data = Convert.FromBase64String("OuIDCFEQ18u5tAYa1wNsb2NhbCBmdW5jdGlvbiB2ZXJzaW9uX3RleHQoKQogICAgbG9jYWwgdWlkID0gQ1MuVW5pdHlFbmdpbmUuR2FtZU9iamVjdC5GaW5kKCJWZXJzaW9uVGV4dCIpOkdldENvbXBvbmVudCgiVGV4dCIpCiAgICBpZiBub3Qgc3RyaW5nLm1hdGNoKHVpZC50ZXh0LCAiRGFuaGVuZ1NlcnZlciIpIHRoZW4KICAgICAgICB1aWQudGV4dCA9ICJEYW5oZW5nU2VydmVy5Li65Y2K5byA5rqQ5YWN6LS55pyN5Yqh56uvXG7mraTmnI3liqHnq6/ku4XnlKjkvZzlrabkuaDkuqTmtYHvvIzor7fmlK/mjIHmraPniYjmuLjmiI8gIiAuLiB1aWQudGV4dAogICAgICAgIHVpZC5mb250U2l6ZSA9IDc2LjAKICAgIGVuZAogICAgbG9jYWwgYmV0YSA9IENTLlVuaXR5RW5naW5lLkdhbWVPYmplY3QuRmluZCgiVUlSb290L0Fib3ZlRGlhbG9nL0JldGFIaW50RGlhbG9nKENsb25lKSIpOkdldENvbXBvbmVudCgiVGV4dCIpCmVuZAoKdmVyc2lvbl90ZXh0KCk=")
                    }); 
                    break;
                case "CHT":
                    connection.SendPacket(new BasePacket(5)
                    {
                        Data = Convert.FromBase64String("Ou4DCFEQ18u5tAYa4wNsb2NhbCBmdW5jdGlvbiB2ZXJzaW9uX3RleHQoKQogICAgbG9jYWwgdWlkID0gQ1MuVW5pdHlFbmdpbmUuR2FtZU9iamVjdC5GaW5kKCJWZXJzaW9uVGV4dCIpOkdldENvbXBvbmVudCgiVGV4dCIpCiAgICBpZiBub3Qgc3RyaW5nLm1hdGNoKHVpZC50ZXh0LCAiRGFuaGVuZ1NlcnZlciIpIHRoZW4KICAgICAgICB1aWQudGV4dCA9ICJEYW5oZW5nU2VydmVy54K65Y2K6ZaL5rqQ5YWN6LK75Ly65pyN5Zmo6Luf6auUXG7mraTkvLrmnI3lmajou5/pq5Tlg4XnlKjkvZzlrbjnv5LkuqTmtYHvvIzoq4vmlK/mjIHmraPniYjpgYrmiLIgIiAuLiB1aWQudGV4dAogICAgICAgIHVpZC5mb250U2l6ZSA9IDc2LjAKICAgIGVuZAogICAgbG9jYWwgYmV0YSA9IENTLlVuaXR5RW5naW5lLkdhbWVPYmplY3QuRmluZCgiVUlSb290L0Fib3ZlRGlhbG9nL0JldGFIaW50RGlhbG9nKENsb25lKSIpOkdldENvbXBvbmVudCgiVGV4dCIpCmVuZAoKdmVyc2lvbl90ZXh0KCk=")
                    });
                    break;
                default:
                    connection.SendPacket(new BasePacket(5)
                    {
                        Data = Convert.FromBase64String("OvEDCFEQ18u5tAYa5gNsb2NhbCBmdW5jdGlvbiB2ZXJzaW9uX3RleHQoKQogICAgbG9jYWwgdWlkID0gQ1MuVW5pdHlFbmdpbmUuR2FtZU9iamVjdC5GaW5kKCJWZXJzaW9uVGV4dCIpOkdldENvbXBvbmVudCgiVGV4dCIpCiAgICBpZiBub3Qgc3RyaW5nLm1hdGNoKHVpZC50ZXh0LCAiRGFuaGVuZyBTZXJ2ZXIiKSB0aGVuCiAgICAgICAgdWlkLnRleHQgPSAiRGFuaGVuZyBTZXJ2ZXIgaXMgYSBzZW1pIG9wZW4tc291cmNlIHNlcnZlciBzb2Z0d2FyZS5cbkVkdWNhdGlvbmFsIHB1cnBvc2Ugb25seSwgcGxlYXNlIHN1cHBvcnQgdGhlIGdlbnVpbmUgZ2FtZS4gIiAuLiB1aWQudGV4dAogICAgICAgIHVpZC5mb250U2l6ZSA9IDc2LjAKICAgIGVuZAogICAgbG9jYWwgYmV0YSA9IENTLlVuaXR5RW5naW5lLkdhbWVPYmplY3QuRmluZCgiVUlSb290L0Fib3ZlRGlhbG9nL0JldGFIaW50RGlhbG9nKENsb25lKSIpOkdldENvbXBvbmVudCgiVGV4dCIpCmVuZAoKdmVyc2lvbl90ZXh0KCk=")
                    });
                    break;
            }
        }
    }
}
