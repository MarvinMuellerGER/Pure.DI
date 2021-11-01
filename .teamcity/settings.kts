import jetbrains.buildServer.configs.kotlin.v2019_2.BuildType
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.*
import jetbrains.buildServer.configs.kotlin.v2019_2.project
import jetbrains.buildServer.configs.kotlin.v2019_2.triggers.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot
import jetbrains.buildServer.configs.kotlin.v2019_2.version
import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.swabra

version = "2021.1"

// Build settings
open class Settings {
    companion object {
        const val sdkVersion = "6.0"
        const val getNextVersionScript =
            "#nr NoCache\n" +
            "using System.Linq;\n" +
            "Props[\"version\"] = \n" +
            "  GetService<INuGet>()\n" +
            "  .Restore(Args[0], \"*\", \"net5.0\")\n" +
            "  .Where(i => i.Name == Args[0])\n" +
            "  .Select(i => i.Version)\n" +
            "  .Select(i => new Version(i.Major, i.Minor, i.Build + 1))\n" +
            "  .DefaultIfEmpty(new Version(1, 0, 0))\n" +
            "  .Max()\n" +
            "  .ToString();\n" +
            "WriteLine($\"Version: {Props[\"version\"]}\", Success);"
        val versions = setOf(
            PackageVersion("3.8", "3.8.0"),
            PackageVersion("4.0", "4.0.0-6.final")
        )
    }
}

class PackageVersion(private val version: String, private val apiVersion: String) {
    val args get() = "/p:AnalyzerRoslynVersion=\"${version}\" /p:AnalyzerRoslynPackageVersion=\"${apiVersion}\""

    override fun toString(): String = version
}

project {
    params {
        param("system.configuration", "Release")
        param("system.version", "1.0.0")
    }

    vcsRoot(Repo)
    buildType(BuildAndTestBuildType)
    buildType(DeployPureDIBuildType)
    buildType(DeployTemplateBuildType)
    buildType(BenchmarkBuildType)
}

object Repo : GitVcsRoot({
    name = "Pure.DI"
    url = "https://github.com/DevTeam/Pure.DI.git"
    branch = "refs/heads/master"
})

object BuildAndTestBuildType: BuildType({
    name = "Build and test"
    vcs { root(Repo) }

    steps {
        for (version in Settings.versions) {
            val versionArgs = version.args
            dotnetTest {
                name = "Run tests for $version"
                args = versionArgs
            }
        }
    }

    triggers {
        vcs {
        }
    }

    failureConditions {
        nonZeroExitCode = true
        testFailure = true
        errorMessage = true
    }
})

object DeployPureDIBuildType: BuildType({
    name = "Deploy Pure.DI"
    artifactRules = "%packagePath% => ."
    params {
        param("packageId", "Pure.DI")
        param("packagePath", "%packageId%/bin/%system.configuration%/%packageId%.%system.version%.nupkg")
    }
    vcs { root(Repo) }
    steps {
        csharpScript {
            name = "Evaluate a next NuGet package version"
            content = Settings.getNextVersionScript
            arguments = "%packageId%"
        }

        for (version in Settings.versions) {
            val versionArgs = version.args
            dotnetBuild {
                name = "Build $version"
                sdk = Settings.sdkVersion
                args = versionArgs
            }
            dotnetTest {
                name = "Run tests for $version"
                skipBuild = true
                args = versionArgs
            }
            dotnetPack {
                name = "Create a NuGet package of $version"
                workingDir = "%packageId%"
                skipBuild = true
                args = versionArgs
            }
        }

        dotnetBuild {
            name = "Build tasks"
            workingDir = "Build"
            projects = "Pure.DI.MSBuild.sln"
        }

        dotnetBuild {
            name = "Merge packages"
            workingDir = "Build"
            projects = "Merge.csproj"
        }

        dotnetNugetPush {
            name = "Push to NuGet"
            packages = "%packagePath%"
            serverUrl = "https://api.nuget.org/v3/index.json"
            apiKey = "%NuGetKey%"
        }
    }

    failureConditions {
        nonZeroExitCode = true
        testFailure = true
        errorMessage = true
    }
})

object DeployTemplateBuildType: BuildType({
    name = "Deploy Template"
    artifactRules = "%packagePath% => ."
    params {
        param("packageId", "Pure.DI.Templates")
        param("packagePath", "%packageId%/bin/%system.configuration%/%packageId%.%system.version%.nupkg")
    }
    vcs { root(Repo) }
    steps {
        csharpScript {
            name = "Evaluate a next NuGet package version"
            content = Settings.getNextVersionScript
            arguments = "%packageId%"
        }
        dotnetPack {
            name = "Create a NuGet package"
            workingDir = "%packageId%"
            sdk = Settings.sdkVersion
            skipBuild = true
        }
        dotnetNugetPush {
            name = "Push the NuGet package"
            packages = "%packagePath%"
            serverUrl = "https://api.nuget.org/v3/index.json"
            apiKey = "%NuGetKey%"
        }
    }

    failureConditions {
        nonZeroExitCode = true
        errorMessage = true
    }
})

object BenchmarkBuildType : BuildType({
    name = "Benchmark"
    artifactRules = "BenchmarkDotNet.Artifacts/results/*.* => ."
    vcs { root(Repo) }
    steps {
        dotnetRun {
            name = "Benchmark"
            projects = "Pure.DI.Benchmark/Pure.DI.Benchmark.csproj"
            framework = "net${Settings.sdkVersion}"
            sdk = Settings.sdkVersion
            configuration = "release"
            args = "-- --filter *Singleton* *Transient* *Func* *Array* *Enum*"
        }
        script {
            name = "Render reports"
            scriptContent = """
            Tools\wkhtmltoimage\wkhtmltoimage.exe BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Singleton-report.html BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Singleton-report.jpg
            Tools\wkhtmltoimage\wkhtmltoimage.exe BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Transient-report.html BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Transient-report.jpg
            Tools\wkhtmltoimage\wkhtmltoimage.exe BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Enum-report.html BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Enum-report.jpg
            Tools\wkhtmltoimage\wkhtmltoimage.exe BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Array-report.html BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Array-report.jpg
            Tools\wkhtmltoimage\wkhtmltoimage.exe BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Func-report.html BenchmarkDotNet.Artifacts/results/Pure.DI.Benchmark.Benchmarks.Func-report.jpg
        """.trimIndent()
        }
    }
    features {
        swabra {
            forceCleanCheckout = true
        }
    }
})