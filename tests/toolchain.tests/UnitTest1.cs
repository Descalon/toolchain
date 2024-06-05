using Shouldly;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Silica.Toolchain.Serialization;

namespace Silica.Toolchain.Tests;

public class UnitTest1 {
    [Fact]
    public void SimpleCmdTest() {
        var input = @"---
            tool: foobar
            description: this is a foobar
            cmd: foo my bar";
        var d = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var actual = d.Deserialize<CliTool>(input);

        CliTool expected = new() {
            Id = "foobar",
            Description = "this is a foobar",
            Cmd = "foo my bar"

        };
        actual.ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public void TypeDescriminationTests() {
        var inputPwsh = @"
            tool: say_hello
            description: says hello
            pwsh: Write-Output ""hello""";
        var inputCli = @"
            tool: say_hello
            description: says hello
            cmd: echo ""hello""";

        var expectedPwsh = new PwshTool {
            Id = "say_hello",
            Description = "says hello",
            Script = @"Write-Output ""hello""",
        };
        var expectedCli = new CliTool {
            Id = "say_hello",
            Description = "says hello",
            Cmd = @"echo ""hello""",
        };

        var mappings = new Dictionary<string, Type>{
            {"pwsh", typeof(PwshTool)},
            {"cmd", typeof(CliTool)},
        };

        var d = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeDiscriminatingNodeDeserializer(o => { o.AddUniqueKeyTypeDiscriminator<Tool>(mappings); }).Build();

        var actualPwsh = d.Deserialize<Tool>(inputPwsh);
        var actualCli = d.Deserialize<Tool>(inputCli);

        actualPwsh.ShouldBeOfType<PwshTool>();
        actualCli.ShouldBeOfType<CliTool>();

        actualPwsh.ShouldBeEquivalentTo(expectedPwsh);
        actualCli.ShouldBeEquivalentTo(expectedCli);
    }

    [Fact]
    public void ToolchainTest() {
        var input = @"
            project: Toolchain

            tools:
            - tool: build
              description: dotnet build
              cmd: dotnet build
            - tool: hello
              description: say hello
              pwsh: Write-Output ""hello""";

        Toolchain expected = new (){
            Project = "Toolchain",
            Tools = [
                new CliTool{
                    Id = "build",
                    Description = "dotnet build",
                    Cmd = "dotnet build"
                },
                new PwshTool{
                    Id = "hello",
                    Description = "say hello",
                    Script = "Write-Output \"hello\""
                },
            ]
        };

        ToolchainDeserializer d = new();
        d.Deserialize(input).ShouldBeEquivalentTo(expected);
    }
}
