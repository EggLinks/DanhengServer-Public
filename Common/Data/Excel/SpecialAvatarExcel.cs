using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("SpecialAvatar.json")]
    public class SpecialAvatarExcel : ExcelResource
    {
        public int SpecialAvatarID { get; set; }
        public int WorldLevel { get; set; }
        public int AvatarID { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SpecialAvatarTypeEnum Type { get; set; }

        public int Level { get; set; }
        public int Promotion { get; set; }
        public int Rank { get; set; }
        public int EquipmentID { get; set; }
        public int EquipmentLevel { get; set; }
        public int EquipmentPromotion { get; set; }
        public int EquipmentRank { get; set; }
        public int RelicPropertyType { get; set; }
        public int RelicPropertyTypeExtra { get; set; }

        [JsonIgnore()]
        public Dictionary<int, int> CurHp { get; set; } = [];
        [JsonIgnore()]
        public Dictionary<int, int> CurSp { get; set; } = [];
        [JsonIgnore()]
        public Dictionary<int, int> EntityId { get; set; } = [];

        public override int GetId()
        {
            return SpecialAvatarID * 10 + WorldLevel;
        }

        public override void Loaded()
        {
            GameData.SpecialAvatarData[GetId()] = this;
        }

        public override void AfterAllDone()
        {
            // TODO Relic handler
        }

        public AvatarInfo ToAvatarData(int uid)
        {
            CurHp.TryGetValue(uid, out var hp);
            CurSp.TryGetValue(uid, out var sp);
            EntityId.TryGetValue(uid, out var Id);
            return new()
            {
                AvatarId = AvatarID,
                SpecialBaseAvatarId = SpecialAvatarID,
                Level = Level,
                Promotion = Promotion,
                Rank = Rank,
                EquipData = new()
                {
                    ItemId = EquipmentID,
                    Level = EquipmentLevel,
                    Promotion = EquipmentPromotion,
                    Rank = EquipmentRank
                },
                CurrentHp = hp == 0 ? 10000 : hp,
                CurrentSp = sp,
                InternalEntityId = Id,
                PlayerData = DatabaseHelper.Instance!.GetInstance<PlayerData>(uid),
            };
        }
    }
}
