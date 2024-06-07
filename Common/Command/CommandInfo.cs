namespace EggLink.DanhengServer.Command
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandInfo(string name, string description, string usage, string keyword = "", string permission = "") : Attribute
    {
        public CommandInfo(string name, string description, string usage, List<string> alias, string keyword = "", string permission = "") : this(name, description, usage, keyword, permission)
        {
            Alias = alias ?? [];
        }

        public string Name { get; } = name;
        public string Description { get; } = description;
        public string Usage { get; } = usage;
        public string Keyword { get; } = keyword;
        public string Permission { get; } = permission;
        public List<string> Alias { get; } = [];
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandMethod(CommandCondition conditions) : Attribute
    {
        public List<CommandCondition> Conditions { get; } = [conditions];
        public CommandMethod(string condition) : this(new CommandCondition())
        {
            var index = 0;
            var conditions = condition.Split(' ');
            foreach (var c in conditions)
            {
                if (int.TryParse(c, out var i))
                {
                    Conditions[index].Index = i;
                }
                else if (c is string s)
                {
                    Conditions[index++].ShouldBe = s;
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandDefault : Attribute
    {
    }

    public class CommandCondition
    {
        public int Index { get; set; }
        public string ShouldBe { get; set; } = "";
    }
}
