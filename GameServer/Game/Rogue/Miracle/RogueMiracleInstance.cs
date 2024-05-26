using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Miracle
{
    public class RogueMiracleInstance
    {
        public BaseRogueInstance Instance { get; }
        public int MiracleId { get; private set; }
        public int Durability { get; private set; }
        public int UsedTimes { get; set; }
        public int UseMoney { get; set; }
        public bool IsDestroyed { get; set; } = false;
        public RogueMiracleEffect? MiracleEffect { get; private set; }

        public RogueMiracleInstance(BaseRogueInstance instance, int miracleId)
        {
            Instance = instance;
            MiracleId = miracleId;

            GameData.RogueMiracleEffectData.Miracles.TryGetValue(MiracleId, out var effect);
            MiracleEffect = effect;

            if (MiracleEffect != null)
            {
                Durability = MiracleEffect.MaxDurability;
            }

            OnGetMiracle();
        }

        public void OnStartBattle(BattleInstance battle)
        {
            if (IsDestroyed) return;
            if (MiracleEffect == null) return;

            foreach (var effect in MiracleEffect.Effects.Values)
            {
                if (effect.Type == RogueMiracleEffectTypeEnum.OnBattleStart)
                {
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
                                if (dynamic != null)
                                {
                                    buff.DynamicValues.Add(dynamic.DynamicKey, CalculateArg(dynamic.Type));
                                }
                            }

                            battle.Buffs.Add(buff);
                        }
                    }
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
                    int count = 0;
                    foreach (var miracle in Instance.RogueMiracles.Values)
                    {
                        if (miracle.IsDestroyed)
                        {
                            count++;
                        }
                    }
                    return count;
                case RogueMiracleEffectTypeEnum.UseMoney:
                    return UseMoney;
                default:
                    return 0;
            }
        }

        public void CostDurability(int value)
        {
            UsedTimes = Math.Min(UsedTimes + value, Durability);  // Prevent overflow
            if (Durability > 0)  // 0 means infinite durability
            {
                if (Durability <= UsedTimes)  // Destroy the miracle
                {
                    OnDestroy();
                    IsDestroyed = true;
                }

                // send packet
                Instance.Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(Instance.RogueVersionId, ToGetResult(), RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle));
            }
        }

        public RogueMiracle ToProto()
        {
            return new()
            {
                MiracleId = (uint)MiracleId,
                Durability = (uint)Durability,
                UsedTimes = (uint)UsedTimes
            };
        }

        public GameRogueMiracle ToGameMiracleProto()
        {
            return new()
            {
                MiracleId = (uint)MiracleId,
                Durability = (uint)Durability,
                UsedTimes = (uint)UsedTimes
            };
        }

        public RogueCommonActionResult ToGetResult()
        {
            return new()
            {
                Source = RogueActionSource.RogueCommonActionResultSourceTypeSelect,
                RogueAction = new()
                {
                    GetMiracleList = new()
                    {
                        MiracleInfo = ToGameMiracleProto()
                    },
                }
            };
        }
    }
}
