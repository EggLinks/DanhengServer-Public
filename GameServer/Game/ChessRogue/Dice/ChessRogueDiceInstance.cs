using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.ChessRogue.Dice
{
    public class ChessRogueDiceInstance(ChessRogueInstance instance, ChessRogueNousDiceData diceData)
    {
        public ChessRogueInstance Instance = instance;
        public ChessRogueNousDiceData DiceData = diceData;

        public int CheatTimes = 1;
        public int RerollTimes = 1;

        public int CurSurfaceId = 0;

        public ChessRogueDiceStatus DiceStatus = ChessRogueDiceStatus.ChessRogueDiceIdle;

        public void RollDice()
        {
            CurSurfaceId = DiceData.Surfaces.ToList().RandomElement().Value;
            DiceStatus = ChessRogueDiceStatus.ChessRogueDiceRolled;
        }
        

        public ChessRogueDiceInfo ToProto()
        {
            var index = DiceData.Surfaces.ToList().FindIndex(x => x.Value == CurSurfaceId) + 1;
            return new()
            {
                BranchId = (uint)DiceData.BranchId,
                Dice = DiceData.ToProto(),
                DiceStatus = DiceStatus,
                CurSurfaceId = (uint)CurSurfaceId,
                CheatTimes = (uint)CheatTimes,
                RerollTimes = (uint)RerollTimes,
                CurBranchId = (uint)DiceData.BranchId,
                DiceType = ChessRogueDiceType.ChessRogueDiceEditable,
                OPIIBFEJFHD = true,
                CurSurfaceIndex = (uint)(index > 0 ? index : 0),
                //DisplayId = (uint)(CurSurfaceId > 0 ? GameData.RogueNousDiceSurfaceData[CurSurfaceId].Sort : 0),
                CanRerollDice = RerollTimes > 0,
                BDMBIDHFKJF = new() { HNHONCDLMEE = { } },
            };
        }
    }
}
