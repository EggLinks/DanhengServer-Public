namespace EggLink.DanhengServer.Enums.Rogue;

public enum RogueMiracleEffectTypeEnum
{
    // effects

    None = 0,
    ExtraBuffSelect = 1,
    ExtraFreeBuffRoll = 2,
    SetSelectBuffLevel = 3,
    ReviveLineupAvatar = 4,
    GetMiracle = 5,
    GetRogueBuff = 6,
    SetSelectBuffGroup = 7,
    AddMazeBuff = 8,
    ChangeItemRatio = 9,
    ChangeItemNum = 10,
    ChangeCostRatio = 11,
    ChangeLineupInfo = 12,
    RepairRandomMiracle = 13,
    EnhanceRandomBuff = 14,
    ReplaceAllMiracles = 15,
    DoAllEffects = 16,
    DoRandomEffect = 17,
    DestroyMiracle = 18,
    CostDurability = 19,
    GetRogueBuffByAeon = 20,
    ChangeItemNumByArg = 21,
    ChangeItemRatioAndSetVar = 22,

    // events

    OnGetMiracle = 100,
    OnBattleStart = 101,
    OnBattleEnd = 102,
    OnEnterNextRoom = 103,
    OnDestroy = 104,
    OnGainMoney = 105,
    OnDestroyProp = 106,
    OnGenerateBuffMenu = 107,

    // arguments

    CurMoney = 200,
    CurDestroyCount = 201,
    CurBrokenMiracleCount = 202,
    AvatarWithLeastHP = 203,
    UseMoney = 204,
    BuffTypeNum = 205
}