using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;

public class ChessRogueDiceInstance(ChessRogueInstance instance, ChessRogueNousDiceData diceData)
{
    public int CheatTimes = 1;

    public int CurSurfaceId;
    public ChessRogueNousDiceData DiceData = diceData;

    public ChessRogueDiceStatus DiceStatus = ChessRogueDiceStatus.ChessRogueDiceIdle;
    public ChessRogueInstance Instance = instance;
    public int RerollTimes = 1;

    public void RollDice()
    {
        CurSurfaceId = DiceData.Surfaces.ToList().RandomElement().Value;
        DiceStatus = ChessRogueDiceStatus.ChessRogueDiceRolled;
    }


    public ChessRogueDiceInfo ToProto()
    {
        var index = DiceData.Surfaces.ToList().FindIndex(x => x.Value == CurSurfaceId) + 1;
        return new ChessRogueDiceInfo
        {
            BranchId = (uint)DiceData.BranchId,
            Dice = DiceData.ToProto(),
            DiceStatus = DiceStatus,
            CurSurfaceId = (uint)CurSurfaceId,
            CheatTimes = (uint)CheatTimes,
            RerollTimes = (uint)RerollTimes,
            CurBranchId = (uint)DiceData.BranchId,
            DiceType = ChessRogueDiceType.ChessRogueDiceEditable,
            DPNCGPOLFKH = true,
            CurSurfaceIndex = (uint)(index > 0 ? index : 0),
            //DisplayId = (uint)(CurSurfaceId > 0 ? GameData.RogueNousDiceSurfaceData[CurSurfaceId].Sort : 0),
            CanRerollDice = RerollTimes > 0,
            DiceModifier = new RogueModifier(),
            AMDLOMOGEOE = new ICNMLEMMHKL()
        };
    }
}