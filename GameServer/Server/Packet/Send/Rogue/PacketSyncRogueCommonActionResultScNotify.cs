using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueCommonActionResultScNotify : BasePacket
    {
        public PacketSyncRogueCommonActionResultScNotify(int versionId, RogueCommonActionResult result, RogueActionDisplayType displayType = RogueActionDisplayType.RogueCommonActionResultDisplayTypeNone) : base(CmdIds.SyncRogueCommonActionResultScNotify)
        {
            var proto = new SyncRogueCommonActionResultScNotify
            {
                RogueVersionId = (uint)versionId,
                DisplayType = displayType
            };

            proto.ActionResult.Add(result);

            SetData(proto);
        }

        public PacketSyncRogueCommonActionResultScNotify(int versionId, List<RogueCommonActionResult> results, RogueActionDisplayType displayType = RogueActionDisplayType.RogueCommonActionResultDisplayTypeNone) : base(CmdIds.SyncRogueCommonActionResultScNotify)
        {
            var proto = new SyncRogueCommonActionResultScNotify
            {
                RogueVersionId = (uint)versionId,
                DisplayType = displayType
            };

            proto.ActionResult.AddRange(results);

            SetData(proto);
        }
    }
}
