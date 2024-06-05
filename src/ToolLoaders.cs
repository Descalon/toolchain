namespace Silica.Toolchain;

public class ToolLoader
{
    public string Load(Tool tool)
        => tool switch
        {
            PwshTool t => Load(t),
            CliTool t => Load(t),
            _ => ""
        };

    private string Load(PwshTool tool)
        => $@"
        function {tool.Id}{{
            {tool.Script}
        }}
        ";
    private string Load(CliTool tool)
        => $@"
        function {tool.Id}{{
            & {tool.Cmd}
        }}
        ";
}
