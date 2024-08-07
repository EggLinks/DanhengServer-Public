namespace EggLink.DanhengServer.Enums.Task;

public enum TargetFetchAdvPropFetchTypeEnum
{
    Owner = 0,
    SinglePropByPropKey = 1,
    SinglePropByPropID = 2,
    SinglePropByUniqueName = 3,
    MultiPropByPropKey = 4,
    MultiPropByPropID = 5,
    MultiPropByUniqueName = 6,
    MultiPropByGroup = 7,
    SinglePropByOwnerGroupAndID = 8
}