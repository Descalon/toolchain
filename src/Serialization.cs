using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Silica.Toolchain.Serialization;

public class ToolchainDeserializer {

    private readonly IDeserializer d;
    private readonly Dictionary<string, Type> mappings = new Dictionary<string, Type>{
        {"pwsh", typeof(PwshTool)},
        {"cmd", typeof(CliTool)},
    };

    public ToolchainDeserializer() {
        d = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeDiscriminatingNodeDeserializer(o => { o.AddUniqueKeyTypeDiscriminator<Tool>(mappings); }).Build();
    }

    public Toolchain Deserialize(string input)
        => d.Deserialize<Toolchain>(input);

    public Toolchain Deserialize(TextReader input)
        => d.Deserialize<Toolchain>(input);
}
