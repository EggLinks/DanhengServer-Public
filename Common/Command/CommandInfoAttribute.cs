namespace EggLink.DanhengServer.Command;

[AttributeUsage(AttributeTargets.Class)]
public class CommandInfoAttribute(
    string name,
    string description,
    string usage,
    string keyword = "",
    string permission = "")
    : Attribute
{
    public CommandInfoAttribute(string name, string description, string usage, string[] alias, string keyword = "",
        string permission = "") : this(name, description, usage, keyword, permission)
    {
        Alias = alias;
    }

    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Usage { get; } = usage;
    public string Keyword { get; } = keyword;
    public string Permission { get; } = permission;
    public string[] Alias { get; } = [];
}

[AttributeUsage(AttributeTargets.Method)]
public class CommandMethodAttribute(CommandCondition conditions) : Attribute
{
    public CommandMethodAttribute(string condition) : this(new CommandCondition())
    {
        var index = 0;
        var conditions = condition.Split(' ');
        foreach (var c in conditions)
            if (int.TryParse(c, out var i))
                Conditions[index].Index = i;
            else Conditions[index++].ShouldBe = c;
    }

    public List<CommandCondition> Conditions { get; } = [conditions];
}

[AttributeUsage(AttributeTargets.Method)]
public class CommandDefaultAttribute : Attribute
{
}

public class CommandCondition
{
    public int Index { get; set; }
    public string ShouldBe { get; set; } = "";
}