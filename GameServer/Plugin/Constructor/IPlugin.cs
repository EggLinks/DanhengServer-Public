using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Plugin.Constructor
{
    public interface IPlugin
    {
        public void OnLoad();
        public void OnUnload();
    }
}
