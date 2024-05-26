using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config
{
    public class SkillAbilityInfo
    {
        public List<AbilityInfo> AbilityList { get; set; } = [];

        public void Loaded(AvatarConfigExcel excel)
        {
            foreach (var ability in AbilityList)
            {
                ability.Loaded();
                if (ability.Name.EndsWith("MazeSkill"))
                {
                    excel.MazeSkill = ability;
                }
                else if (ability.Name.Contains("NormalAtk"))
                {
                    excel.MazeAtk = ability;
                }
            }
        }
    } 

    public class AbilityInfo
    {
        public string Name { get; set; } = "";
        public List<TaskInfo> OnStart { get; set; } = [];

        public void Loaded()
        {
            foreach (var task in OnStart)
            {
                task.Loaded();
            }
        }
    }
}
