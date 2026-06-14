using Dixor.Identity.Generators;
using Xunit;

namespace Dixor.Identity.Tests;

public class Uuid7GeneratorTests
{
    [Fact]
    public void NewGuid_Should_Return_Valid_Guid()
    {
        var guid = Uuid7Generator.NewGuid();
        Console.WriteLine(guid);

        Assert.NotEqual(Guid.Empty, guid);
    }

    [Fact]
    public void NewGuid_Should_Return_Unique_Guids()
    {
        var guid1 = Uuid7Generator.NewGuid();
        Console.WriteLine(guid1);

        var guid2 = Uuid7Generator.NewGuid();
        Console.WriteLine(guid2);

        Assert.NotEqual(guid1, guid2);
    }
}