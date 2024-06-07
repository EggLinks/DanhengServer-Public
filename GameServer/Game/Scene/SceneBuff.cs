using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Scene
{
    public class SceneBuff(int buffID, int buffLevel, int owner, int duration = -1)
    {
        public int BuffID { get; private set; } = buffID;
        public int BuffLevel { get; private set; } = buffLevel;
        public int OwnerAvatarId { get; private set; } = owner;

        public int Duration { get; set; } = duration;
        public long CreatedTime { get; set; } = Extensions.GetUnixMs();
        public Dictionary<string, float> DynamicValues = [];

        public bool IsExpired()
        {
            if (Duration < 0)
                return false;  // Permanent buff
            return Extensions.GetUnixMs() - CreatedTime >= Duration;
        }

        public BuffInfo ToProto() {
            var buffInfo = new BuffInfo()
            {
                BuffId = (uint)BuffID,
                Level = (uint)BuffLevel,
                BaseAvatarId = (uint)OwnerAvatarId,
                AddTimeMs = (ulong)CreatedTime,
                LifeTime = (ulong)Duration,
            };

            return buffInfo;
        }
    }
}
