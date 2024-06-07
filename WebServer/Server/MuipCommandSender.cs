using EggLink.DanhengServer.Command;

namespace EggLink.DanhengServer.WebServer.Server
{
    public class MuipCommandSender(MuipSession session, Action<string> action) : ICommandSender
    {
        public MuipSession Session { get; } = session;
        public Action<string> MsgAction { get; } = action;
        public int SenderUid { get; set; } = session.Account?.Uid ?? 0;

        public bool HasPermission(string permission)
        {
            if (Session.IsAdmin)
            {
                return true;
            }

            return Session.Account?.Permissions?.Contains(permission) ?? false;
        }

        public void SendMsg(string msg)
        {
            MsgAction(msg);
        }

        public int GetSender()
        {
            return SenderUid;
        }
    }
}
