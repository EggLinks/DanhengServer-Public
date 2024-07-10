# Mission Part
## Menu
- [MissionManager](#missionmanager)
- [FinishTypeHandler](#finishtypehandler)
- [FinishActionHandler](#finishactionhandler)

## MissionManager
- `MissionManager.cs`: Used to manage the player's mission data

### Method
- (need to write)


## FinishTypeHandler

Class Name Style: **MissionHandler<Type>**  
Example:
```
[MissionFinishType(MissionFinishTypeEnum.<Type>)]
public class MissionHandler<Type> : MissionFinishTypeHandler
{
    public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        // your code
    } 

    public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        // your code
    }
}
```

## FinishActionHandler

Class Name Style: **MissionHandler<Type>**  
Example:
```
[MissionFinishAction(FinishActionTypeEnum.<Type>)]
public class MissionHandler<Type> : MissionFinishActionHandler
{
    public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
    {
        // your code
    }
}
```
