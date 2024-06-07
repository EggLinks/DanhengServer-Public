using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Scene.Entity
{
    public interface IGameEntity
    {
        public int EntityID { get; set; }
        public int GroupID { get; set; }

        public void AddBuff(SceneBuff buff);
        public void ApplyBuff(BattleInstance instance);


        public SceneEntityInfo ToProto();
    }
}
