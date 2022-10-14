using CommandLine;
using JetBrains.Annotations;

namespace TestApp;

[UsedImplicitly]
public class Options
{
    [Option('r',"remote", Required = true, HelpText = "The url of the remote repository")]
    public string? RemoteRepositoryUrl { get; [UsedImplicitly] set; }
    
    [Option('c', "clone", Required = true, HelpText = "The path where we want to clone the remote repository locally")]
    public string? ClonePath { get; [UsedImplicitly] set; }
}