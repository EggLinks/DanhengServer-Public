using EggLink.DanhengServer.Enums.Language;

namespace EggLink.DanhengServer.Internationalization;

[AttributeUsage(AttributeTargets.Class)]
public class PluginLanguageAttribute(ProgramLanguageTypeEnum languageType) : Attribute
{
    public ProgramLanguageTypeEnum LanguageType { get; } = languageType;
}