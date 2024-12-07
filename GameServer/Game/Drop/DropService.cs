using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums.Scene;

namespace EggLink.DanhengServer.GameServer.Game.Drop;

public class DropService
{
    public static List<ItemData> CalculateDropsFromProp(int chestId)
    {
        var items = new List<ItemData>();
        var chest = GameData.MazeChestData.GetValueOrDefault(chestId);
        if (chest == null) return items;

        var level = ChestTypeEnum.CHEST_NONE;

        if (chest.ChestType.Contains(ChestTypeEnum.CHEST_HIGH_LEVEL))
            level = ChestTypeEnum.CHEST_HIGH_LEVEL;
        else if (chest.ChestType.Contains(ChestTypeEnum.CHEST_MIDDLE_LEVEL))
            level = ChestTypeEnum.CHEST_MIDDLE_LEVEL;
        else if (chest.ChestType.Contains(ChestTypeEnum.CHEST_LOW_LEVEL))
            level = ChestTypeEnum.CHEST_LOW_LEVEL;

        var world = ChestTypeEnum.CHEST_NONE;
        if (chest.ChestType.Contains(ChestTypeEnum.CHEST_WORLD_ONE))
            world = ChestTypeEnum.CHEST_WORLD_ONE;
        else if (chest.ChestType.Contains(ChestTypeEnum.CHEST_WORLD_TWO))
            world = ChestTypeEnum.CHEST_WORLD_TWO;
        else if (chest.ChestType.Contains(ChestTypeEnum.CHEST_WORLD_THREE))
            world = ChestTypeEnum.CHEST_WORLD_THREE;
        else if (chest.ChestType.Contains(ChestTypeEnum.CHEST_WORLD_ZERO))
            world = ChestTypeEnum.CHEST_WORLD_ZERO;

        items.Add(new ItemData
        {
            ItemId = 1,
            Count = level switch
            {
                ChestTypeEnum.CHEST_LOW_LEVEL => 5,
                ChestTypeEnum.CHEST_MIDDLE_LEVEL => 20,
                ChestTypeEnum.CHEST_HIGH_LEVEL => 40,
                _ => 5
            }
        });

        items.Add(new ItemData
        {
            ItemId = 212,
            Count = Random.Shared.Next(3, 6)
        });

        items.Add(new ItemData
        {
            ItemId = 222,
            Count = Random.Shared.Next(3, 6)
        });

        items.Add(new ItemData
        {
            ItemId = 232,
            Count = Random.Shared.Next(3, 6)
        });

        switch (world)
        {
            case ChestTypeEnum.CHEST_WORLD_ONE:
                items.Add(new ItemData
                {
                    ItemId = 120001,
                    Count = level switch
                    {
                        ChestTypeEnum.CHEST_LOW_LEVEL => 20,
                        ChestTypeEnum.CHEST_MIDDLE_LEVEL => 40,
                        ChestTypeEnum.CHEST_HIGH_LEVEL => 60,
                        _ => 20
                    }
                });
                break;
            case ChestTypeEnum.CHEST_WORLD_TWO:
                items.Add(new ItemData
                {
                    ItemId = 120002,
                    Count = level switch
                    {
                        ChestTypeEnum.CHEST_LOW_LEVEL => 5,
                        ChestTypeEnum.CHEST_MIDDLE_LEVEL => 10,
                        ChestTypeEnum.CHEST_HIGH_LEVEL => 20,
                        _ => 5
                    }
                });
                break;
            case ChestTypeEnum.CHEST_WORLD_THREE:
                items.Add(new ItemData
                {
                    ItemId = 120003,
                    Count = level switch
                    {
                        ChestTypeEnum.CHEST_LOW_LEVEL => 60,
                        ChestTypeEnum.CHEST_MIDDLE_LEVEL => 90,
                        ChestTypeEnum.CHEST_HIGH_LEVEL => 120,
                        _ => 60
                    }
                });
                break;
            case ChestTypeEnum.CHEST_WORLD_ZERO:
                items.Add(new ItemData
                {
                    ItemId = 120000,
                    Count = level switch
                    {
                        ChestTypeEnum.CHEST_LOW_LEVEL => 10,
                        ChestTypeEnum.CHEST_MIDDLE_LEVEL => 20,
                        ChestTypeEnum.CHEST_HIGH_LEVEL => 50,
                        _ => 10
                    }
                });
                break;
        }

        items.Add(new ItemData
        {
            ItemId = 2,
            Count = level switch
            {
                ChestTypeEnum.CHEST_LOW_LEVEL => 750,
                ChestTypeEnum.CHEST_MIDDLE_LEVEL => 3700,
                ChestTypeEnum.CHEST_HIGH_LEVEL => 6000,
                _ => 750
            }
        });

        return items;
    }
}