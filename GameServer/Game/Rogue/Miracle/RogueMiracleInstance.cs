using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Miracle;

public class RogueMiracleInstance
{
    public RogueMiracleInstance(BaseRogueInstance instance, int miracleId)
    {
        Instance = instance;
        MiracleId = miracleId;

        GameData.RogueMiracleEffectData.Miracles.TryGetValue(MiracleId, out var effect);
        MiracleEffect = effect;

        if (MiracleEffect != null) Durability = MiracleEffect.MaxDurability;

        OnGetMiracle();
    }

    public BaseRogueInstance Instance { get; }
    public int MiracleId { get; }
    public int Durability { get; }
    public int UsedTimes { get; set; }
    public int UseMoney { get; set; }
    public bool IsDestroyed { get; set; }
    public RogueMiracleEffect? MiracleEffect { get; }

    public void OnStartBattle(BattleInstance battle)
    {
        if (IsDestroyed) return;
        if (MiracleEffect == null) return;

        foreach (var effect in MiracleEffect.Effects.Values)
            if (effect.Type == RogueMiracleEffectTypeEnum.OnBattleStart)
                foreach (var param in effect.Params)
                {
                    MiracleEffect.Effects.TryGetValue(param, out var target);
                    if (target == null) continue;

                    if (target.Type == RogueMiracleEffectTypeEnum.AddMazeBuff)
                    {
                        var buffId = target.Params[0];
                        var dynamicEffect = target.Params[1];
                        var buff = new MazeBuff(buffId, 1, -1)
                        {
                            WaveFlag = -1
                        };

                        if (dynamicEffect != 0)
                        {
                            MiracleEffect.Effects.TryGetValue(dynamicEffect, out var dynamic);
                            if (dynamic != null) buff.DynamicValues.Add(dynamic.DynamicKey, CalculateArg(dynamic.Type));
                        }

                        battle.Buffs.Add(buff);
                    }
                }
    }

    public void OnEndBattle(BattleInstance battle)
    {
        if (IsDestroyed) return;
        if (MiracleEffect == null) return;
    }

    public void OnEnterNextRoom()
    {
        if (IsDestroyed) return;
        if (MiracleEffect == null) return;
    }

    public void OnGetMiracle()
    {
        if (IsDestroyed) return;
        if (MiracleEffect == null) return;
    }

    public void OnDestroy()
    {
        if (IsDestroyed) return;
        if (MiracleEffect == null) return;
    }

    public int CalculateArg(RogueMiracleEffectTypeEnum type)
    {
        switch (type)
        {
            case RogueMiracleEffectTypeEnum.CurMoney:
                return Instance.CurMoney;
            case RogueMiracleEffectTypeEnum.CurDestroyCount:
                return Instance.CurDestroyCount;
            case RogueMiracleEffectTypeEnum.CurBrokenMiracleCount:
                var count = 0;
                foreach (var miracle in Instance.RogueMiracles.Values)
                    if (miracle.IsDestroyed)
                        count++;
                return count;
            case RogueMiracleEffectTypeEnum.UseMoney:
                return UseMoney;
            default:
                return 0;
        }
    }

    public async ValueTask CostDurability(int value)
    {
        UsedTimes = Math.Min(UsedTimes + value, Durability); // Prevent overflow
        if (Durability > 0) // 0 means infinite durability
        {
            if (Durability <= UsedTimes) // Destroy the miracle
            {
                OnDestroy();
                IsDestroyed = true;
            }

            // send packet
            await Instance.Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(Instance.RogueSubMode,
                ToGetResult(), RogueCommonActionResultDisplayType.Single));
        }
    }

    public RogueMiracle ToProto()
    {
        return new RogueMiracle
        {
            MiracleId = (uint)MiracleId,
            MaxTimes = (uint)Durability,
            CurTimes = (uint)UsedTimes
        };
    }

    public GameRogueMiracle ToGameMiracleProto()
    {
        return new GameRogueMiracle
        {
            MiracleId = (uint)MiracleId,
            Durability = (uint)Durability,
            CurTimes = (uint)UsedTimes
        };
    }

    public RogueCommonActionResult ToGetResult()
    {
        return new RogueCommonActionResult
        {
            Source = RogueCommonActionResultSourceType.Select,
            RogueAction = new RogueCommonActionResultData
            {
                GetMiracleList = new RogueCommonMiracle
                {
                    MiracleInfo = ToGameMiracleProto()
                }
            }
        };
    }
}