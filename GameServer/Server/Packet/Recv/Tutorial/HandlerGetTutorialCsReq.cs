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
        private static readonly Logger Logger = new("GameServer");
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            SendPlayerData(connection);
            if (ConfigManager.Config.ServerOption.EnableMission)  // If missions are enabled
                connection.SendPacket(new PacketGetTutorialScRsp(connection.Player!));
        }
        private void SendPlayerData(Connection connection)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Lua", "welcome.lua");
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);  // 读取文件内容
                connection.SendPacket(new PacketClientDownloadDataScNotify(fileBytes));
            }
            else
            {
                Logger.Error("请求的文件不存在!");
            }
        }
    }
}
