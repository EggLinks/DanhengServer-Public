using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Plugin.Constructor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginInfo(string name, string description, string version) : Attribute
    {
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string Version { get; } = version;
    }
}
