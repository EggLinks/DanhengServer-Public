using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;

public class ChessRogueDiceInstance(ChessRogueInstance instance, ChessRogueNousDiceData diceData)
{
    public int CheatTimes { get; set; } = 1;

    public int CurSurfaceId { get; set; }
    public ChessRogueNousDiceData DiceData { get; set; } = diceData;

    public ChessRogueDiceStatus DiceStatus { get; set; } = ChessRogueDiceStatus.ChessRogueDiceIdle;
    public ChessRogueInstance Instance { get; set; } = instance;
    public int RerollTimes { get; set; } = 1;

    public ChessRogueDiceSurfaceEffectConfig? CurrentSurfaceEffectConfig =>
        GameData.ChessRogueDiceSurfaceEffectData.GetValueOrDefault(CurSurfaceId);

    public ChessRogueDiceModifierInstance? Modifier { get; set; }

    public void RollDice()
    {
        CurSurfaceId = DiceData.Surfaces.ToList().RandomElement().Value;
        DiceStatus = ChessRogueDiceStatus.ChessRogueDiceRolled;
    }

    public async ValueTask ConfirmDice()
    {
        DiceStatus = ChessRogueDiceStatus.ChessRogueDiceConfirmed;

        if (CurrentSurfaceEffectConfig == null) return;

        if (Modifier != null) // Remove the previous modifier
            await Instance.Player.SendPacket(new PacketRogueModifierDelNotify(Modifier));

        Modifier = new ChessRogueDiceModifierInstance(Instance.CurModifierId++,
            CurrentSurfaceEffectConfig.ContentEffects.First());

        await Modifier.OnConfirmed(Instance);

        await Instance.Player.SendPacket(new PacketRogueModifierAddNotify(Modifier));
    }


    public ChessRogueDiceInfo ToProto()
    {
        var index = DiceData.Surfaces.ToList().FindIndex(x => x.Value == CurSurfaceId) + 1;
        return new ChessRogueDiceInfo
        {
            GameBranchId = (uint)DiceData.BranchId,
            Dice = DiceData.ToProto(),
            DiceStatus = DiceStatus,
            CurSurfaceId = (uint)CurSurfaceId,
            CheatTimes = (uint)CheatTimes,
            RerollTimes = (uint)RerollTimes,
            GameDiceBranchId = (uint)DiceData.BranchId,
            DiceType = ChessRogueDiceType.ChessRogueDiceEditable,
            AMFBDDACHKB = true,
            CurSurfaceSlotId = (uint)(index > 0 ? index : 0),
            CanRerollDice = RerollTimes > 0,
            DiceModifier = Modifier?.ToProto() ?? new RogueModifier(),
            IPNFHJEFGAM = new JPEGOGNDPJJ()
        };
    }
}