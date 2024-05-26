using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command
{
    public class CommandArg
    {
        public string Raw { get; }
        public List<string> BasicArgs { get; } = [];
        public Dictionary<string, string> CharacterArgs { get; } = [];
        public Connection? Target { get; set; }
        public ICommandSender Sender { get; }

        public CommandArg(string raw, ICommandSender sender, Connection? con = null)
        {
            Raw = raw;
            Sender = sender;
            var args = raw.Split(' ');
            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                {
                    continue;
                }
                var character = arg[0];
                if (!int.TryParse(character.ToString(), out var _) && character != '-')
                {
                    try
                    {
                        CharacterArgs.Add(arg[..1], arg[1..]);
                    } catch
                    {
                        BasicArgs.Add(arg);
                    }
                }
                else
                {
                    BasicArgs.Add(arg);
                }
            }
            if (con != null)
            {
                Target = con;
            }

            CharacterArgs.TryGetValue("@", out var target);
            if (target != null)
            {
                var connection = Listener.Connections.Values.ToList().Find(item => item.Player?.Uid.ToString() == target);
                if (connection != null)
                {
                    Target = connection;
                }
            }
        }
        public int GetInt(int index)
        {
            if (BasicArgs.Count <= index)
            {
                return 0;
            }
            _ = int.TryParse(BasicArgs[index], out int res);
            return res;
        }

        public void SendMsg(string msg)
        {
            Sender.SendMsg(msg);
        }

        public override string ToString()
        {
            return $"BasicArg: {BasicArgs.ToArrayString()}. CharacterArg: {CharacterArgs.ToJsonString()}.";
        }
    }
}
