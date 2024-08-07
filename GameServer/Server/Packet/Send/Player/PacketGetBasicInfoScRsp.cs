using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketGetBasicInfoScRsp : BasePacket
{
    public PacketGetBasicInfoScRsp(PlayerInstance player) : base(CmdIds.GetBasicInfoScRsp)
    {
        var proto = new GetBasicInfoScRsp
        {
            CurDay = 1,
            NextRecoverTime = player.Data.NextStaminaRecover / 1000,
            GameplayBirthday = (uint)player.Data.Birthday,
            PlayerSettingInfo = new PlayerSettingInfo(),
            Gender = (uint)player.Data.CurrentGender
        };

        if (ConfigManager.Config.ServerOption.EnableMission)
        {
            if (player.AvatarManager!.GetHero()!.PathInfoes.Count > 0) player.Data.IsGenderSet = true;
            proto.Gender = (uint)player.Data.CurrentGender;
            proto.IsGenderSet = player.Data.IsGenderSet;
        }
        else
        {
            proto.Gender = (uint)player.Data.CurrentGender;
            proto.IsGenderSet = true;
        }

        SetData(proto);
    }
}