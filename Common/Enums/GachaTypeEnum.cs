namespace EggLink.DanhengServer.Enums;

public enum GachaTypeEnum
{
    Unknown = 0,
    Newbie = 1,
    Normal = 2,
    AvatarUp = 11,
    WeaponUp = 12
}

public static class GachaTypeEnumExtensions
{
    public static int GetCostItemId(this GachaTypeEnum type)
    {
        return type switch
        {
            GachaTypeEnum.Newbie or GachaTypeEnum.Normal => 101,
            GachaTypeEnum.AvatarUp or GachaTypeEnum.WeaponUp => 102,
            _ => 0
        };
    }
}