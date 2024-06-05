using YamlDotNet.Serialization;

namespace Silica.Toolchain;

public abstract class Tool {
    [YamlMember(Alias = "tool")]
    public string Id { get; set; } = "";
    public string Description { get; set; } = "";
}

public class CliTool : Tool {

    public string Cmd { get; set; } = "";
}

public class PwshTool : Tool {
    [YamlMember(Alias = "pwsh")]
    public string Script { get; set; } = "";
}

public class Toolchain {
    public string Project {get;set;} = "";
    public Tool[] Tools {get;set;} = [];
}


