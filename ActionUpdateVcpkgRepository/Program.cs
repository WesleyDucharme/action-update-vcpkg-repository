using CommandLine;
using LibGit2Sharp;
using Serilog;
using TestApp;

var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
log.Information($"Arguments ({args.Length}): {string.Join(", ", args)}");

var argParseResult = Parser.Default.ParseArguments<Options>(args);
if (argParseResult.Errors.Any())
{
    return 1;
}

var options = argParseResult.Value;

if (Directory.Exists(options.ClonePath))
{
    log.Information($"Deleting existing local clone directory: '{options.ClonePath}'");
    
    // Make sure all previously cloned files are writable so we can delete them all
    // Certain git files remained read only
    foreach (var file in Directory.GetFiles(options.ClonePath, "*.*", SearchOption.AllDirectories))
    {
        var fileInfo = new FileInfo(file)
        {
            IsReadOnly = false
        };
    }
    
    Directory.Delete(options.ClonePath, true);
}

log.Information($"Cloning '{options.RemoteRepositoryUrl}' to '{options.ClonePath}'");

var repoPath = Repository.Clone(options.RemoteRepositoryUrl, options.ClonePath, new CloneOptions());
if (repoPath == null)
{
    log.Error($"Failed to clone repository '{options.RemoteRepositoryUrl}' to '{options.ClonePath}'");
    return 1;
}

log.Information($"Finished cloning '{options.RemoteRepositoryUrl}' to '{repoPath}'");

using var repo = new Repository(repoPath);

foreach (var commit in repo.Commits)
{
    log.Information($"BEFORE BRANCH {commit.Sha} : {commit.MessageShort}");
}

var testBranch = repo.CreateBranch("TestBranch");
Commands.Checkout(repo, testBranch);

var testFilePath = Path.Combine(options.ClonePath, "TestFile.txt");
File.WriteAllText(testFilePath, "A Testfile with contents");

Commands.Stage(repo, "*");

var signature = new Signature("testauthor", "testauthor@someemail.com", DateTimeOffset.Now);
repo.Commit("A test commit", signature, signature);

foreach (var commit in repo.Commits)
{
    log.Information($"POST BRANCH {commit.Sha} : {commit.MessageShort}");
}

Commands.Checkout(repo, repo.Branches["main"]);

return 0;