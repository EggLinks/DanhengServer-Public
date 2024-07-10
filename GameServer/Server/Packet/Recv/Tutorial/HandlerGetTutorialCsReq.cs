using EggLink.DanhengServer.Server.Packet.Send.Others;
using EggLink.DanhengServer.Server.Packet.Send.Tutorial;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Tutorial
{
    [Opcode(CmdIds.GetTutorialCsReq)]
    public class HandlerGetTutorialCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            if (ConfigManager.Config.ServerOption.EnableMission)  // If missions are enabled
                connection.SendPacket(new PacketGetTutorialScRsp(connection.Player!));
                SendPlayerMissionData(connection);
        }

        private void SendPlayerMissionData(Connection connection)
        {
            // DO NOT REMOVE THIS CODE
            // This code is responsible for sending the mission data to the player
            switch (ConfigManager.Config.ServerOption.Language)
            {
                case "CHS":
                    connection.SendPacket(new BasePacket(5)
                    {
                        Data = Convert.FromBase64String("OtYDCFEQzsq5tAYaywNsb2NhbCBmdW5jdGlvbiBvbkRpYWxvZ0Nsb3NlZCgpCiAgICBDUy5Vbml0eUVuZ2luZS5BcHBsaWNhdGlvbi5PcGVuVVJMKCJodHRwczovL3NyLm1paG95by5jb20vIikKZW5kCgpsb2NhbCBmdW5jdGlvbiBzaG93X2hpbnQoKQogICAgbG9jYWwgdGV4dCA9ICLmrKLov47kvb/nlKhEYW5oZW5nU2VydmVy77yBXG4iCiAgICB0ZXh0ID0gdGV4dCAuLiAi5pys5pyN5Yqh5Zmo5a6M5YWo5YWN6LS577yM5aaC5p6c5oKo5piv6LSt5Lmw5b6X5Yiw55qE77yM6YKj5LmI5oKo5bey57uP6KKr6aqX5LqG77yBXG4iCiAgICB0ZXh0ID0gdGV4dCAuLiAi5q2k5pyN5Yqh56uv5LuF55So5L2c5a2m5Lmg5Lqk5rWB77yM6K+35pSv5oyB5q2j54mIXG4iCiAgICBDUy5SUEcuQ2xpZW50LkNvbmZpcm1EaWFsb2dVdGlsLlNob3dDdXN0b21Pa0NhbmNlbEhpbnQodGV4dCwgb25EaWFsb2dDbG9zZWQpCmVuZAoKc2hvd19oaW50KCk=")
                    });
                    break;
                case "CHT":
                    connection.SendPacket(new BasePacket(5)
                    {
                        Data = Convert.FromBase64String("OuMDCFEQzsq5tAYa2ANsb2NhbCBmdW5jdGlvbiBvbkRpYWxvZ0Nsb3NlZCgpCiAgICBDUy5Vbml0eUVuZ2luZS5BcHBsaWNhdGlvbi5PcGVuVVJMKCJodHRwczovL2hzci5ob3lvdmVyc2UuY29tLyIpCmVuZAoKbG9jYWwgZnVuY3Rpb24gc2hvd19oaW50KCkKICAgIGxvY2FsIHRleHQgPSAi5q2h6L+O5L2/55SoRGFuaGVuZ1NlcnZlcu+8gVxuIgogICAgdGV4dCA9IHRleHQgLi4gIuacrOS8uuacjeWZqOWujOWFqOWFjeiyu++8jOWmguaenOaCqOaYr+izvOiyt+W+l+WIsOeahOOAgumCo+m6vOaCqOW3sue2k+iiq+momeS6hu+8gVxuIgogICAgdGV4dCA9IHRleHQgLi4gIuatpOS8uuacjeWZqOi7n+mrlOWDheS+m+WtuOe/kuS6pOa1geS9v+eUqO+8jOiri+aUr+aMgeato+eJiFxuIgogICAgQ1MuUlBHLkNsaWVudC5Db25maXJtRGlhbG9nVXRpbC5TaG93Q3VzdG9tT2tDYW5jZWxIaW50KHRleHQsIG9uRGlhbG9nQ2xvc2VkKQplbmQKCnNob3dfaGludCgp")
                    });
                    break;
                default:
                    connection.SendPacket(new BasePacket(5)
                    {
                        Data = Convert.FromBase64String("Ou4DCFEQzsq5tAYa4wNsb2NhbCBmdW5jdGlvbiBvbkRpYWxvZ0Nsb3NlZCgpCiAgICBDUy5Vbml0eUVuZ2luZS5BcHBsaWNhdGlvbi5PcGVuVVJMKCJodHRwczovL2hzci5ob3lvdmVyc2UuY29tLyIpCmVuZAoKbG9jYWwgZnVuY3Rpb24gc2hvd19oaW50KCkKICAgIGxvY2FsIHRleHQgPSAiV2VsY29tZSB0byBEYW5oZW5nU2VydmVyIVxuIgogICAgdGV4dCA9IHRleHQgLi4gIlRoaXMgc2VydmVyIHNvZnR3YXJlIGlzIHRvdGFsbHkgZnJlZS5cbiIKICAgIHRleHQgPSB0ZXh0IC4uICJJZiB5b3UgcGF5IGZvciBpdCwgeW91IGhhdmUgYmVlbiBzY2FtbWVkLlxuIgogICAgdGV4dCA9IHRleHQgLi4gIkVkdWNhdGlvbmFsIHB1cnBvc2Ugb25seSwgcGxlYXNlIHN1cHBvcnQgdGhlIGdlbnVpbmUgZ2FtZS4iCiAgICBDUy5SUEcuQ2xpZW50LkNvbmZpcm1EaWFsb2dVdGlsLlNob3dDdXN0b21Pa0NhbmNlbEhpbnQodGV4dCwgb25EaWFsb2dDbG9zZWQpCmVuZAoKc2hvd19oaW50KCk=")
                    });
                    break;
            }
        }
    }
}
