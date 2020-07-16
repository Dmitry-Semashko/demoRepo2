#tool nuget:?package=NUnit.ConsoleRunner&version=3.9.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Define the build directory.
var buildDir = Directory("Godel.HelloWorld.UnitTests/bin") ;

Task("Clean")
  .Does(() =>
        {
          CleanDirectory(buildDir);
        });

Task("Restore-NuGet-Packages")
  .IsDependentOn("Clean")
  .Does(() =>
        {
          NuGetRestore("Godel.HelloWorld.sln");
        });

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() =>
        {
          MSBuild("Godel.HelloWorld.sln", settings =>
                  settings.SetConfiguration(configuration));
        });

 Task("Run-Unit-Tests")
  .IsDependentOn("Build")
     .Does(() =>
 {
     var settings = new DotNetCoreTestSettings
     {
         // Outputing test results as XML so that VSTS can pick it up
         ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=TestResults.xml\""),
         Configuration = "Release"
     };

     var projectFiles = GetFiles("Godel.HelloWorld.UnitTests/*.csproj");
     foreach(var file in projectFiles)
     {
         DotNetCoreTest(file.FullPath, settings);
     }
 });

Task("Default")
  .IsDependentOn("Run-Unit-Tests");

RunTarget(target);
