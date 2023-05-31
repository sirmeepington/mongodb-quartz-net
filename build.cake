#tool "nuget:?package=NUnit.ConsoleRunner"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./artifacts/");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetRestore();
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetMSBuildSettings();
    settings.SetConfiguration(configuration);
    DotNetMSBuild(settings);
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetTestSettings
    {
        Configuration = "Release",
        Framework = "net6.0"
    };

    DotNetTest("./tests/Quartz.Spi.MongoDbJobStore.Tests/Quartz.Spi.MongoDbJobStore.Tests.csproj", settings);
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() => 
{
    var settings = new DotNetPackSettings
    {
        Configuration = configuration,
        OutputDirectory = "./artifacts/"
    };

    DotNetPack("./src/Quartz.Spi.MongoDbJobStore/Quartz.Spi.MongoDbJobStore.csproj", settings);
});

Task("AppVeyor")
    .IsDependentOn("Test")
    .IsDependentOn("Pack")
    .Does(() => 
{

});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
