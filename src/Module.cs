using System.Management.Automation;
using Silica.Toolchain.Serialization;

namespace Silica.Toolchain;

[Cmdlet(VerbsData.Import, "Toolchain")]
public class ImportToolchainCommand : PSCmdlet
{
    [Parameter()]
    public FileInfo? Path { get; set; }

    protected override void BeginProcessing()
    {
        /* if (Path != null) return; */
        /* var cwd = Directory.GetCurrentDirectory(); */
        /* var dir = new DirectoryInfo(cwd); */
        /* while (dir != null) */
        /* { */
        /*     Path = dir.GetFiles().First(x => x.Name == "toolchain.yml").FullName; */
        /*     if (Path != null) break; */
        /*     dir = dir.Parent; */
        /* } */
    }

    protected override void EndProcessing()
    {
        if (Path == null) throw new ArgumentNullException("No toolchain.yml found");
        using var reader = File.OpenText("d:/csharp/toolchain/example.yml");
        var deserializer = new ToolchainDeserializer();
        var toolchain = deserializer.Deserialize(reader);
        var loader = new ToolLoader();
        foreach(var tool in toolchain.Tools){
            InvokeCommand.InvokeScript(loader.Load(tool));
        }
    }
}