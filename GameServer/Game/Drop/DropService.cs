using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Game.Drop
{
    public class DropService
    {
        public static List<ItemData> CalculateDropsFromProp()
        {
            List<ItemData> drops =
            [
                new ItemData()
                {
                    ItemId = 1,
                    Count = 5
                },
                new ItemData()
                {
                    ItemId = 22,
                    Count = 5
                },
                new ItemData()
                {
                    ItemId = 2,
                    Count = new Random().Next(20, 100)
                },
            ];

            return drops;
        }
    }
}
