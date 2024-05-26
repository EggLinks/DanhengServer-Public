using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("BattleEventData.json")]
    public partial class BattleEventDataExcel : ExcelResource
    {
        public int BattleEventID { get; set; }
        public string Config { get; set; } = "";

        [GeneratedRegex(@"(?<=Avatar_RogueBattleevent)(.*?)(?=_Config.json)")]
        private static partial Regex RegexConfig();

        public override int GetId()
        {
            return BattleEventID;
        }

        public override void Loaded()
        {
            try
            {
                Match match = RegexConfig().Match(Config);
                if (match.Success)
                {
                    int rogueBuffType = int.Parse(match.Value);
                    GameData.RogueBattleEventData.Add(rogueBuffType, this);
                }
            } catch
            {

            }
        }
    }
}
