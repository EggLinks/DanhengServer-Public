using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Plugin.Constructor
{
    [Obsolete("以俟君子  Wait for someone to develop it")]
    public interface IPlugin
    {
        public void OnLoad();
        public void OnUnload();
    }
}
