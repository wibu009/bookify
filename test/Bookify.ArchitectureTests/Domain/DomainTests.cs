using Bookify.ArchitectureTests.Infrastructure;
using Bookify.Domain.Abstractions;
using NetArchTest.Rules;
using BindingFlags = System.Reflection.BindingFlags;

namespace Bookify.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();
        
        var failingTypes = (from entityType in entityTypes
            let constructor = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
            where !constructor.Any(c => c.IsPrivate && c.GetParameters().Length == 0)
            select entityType).ToList();

        failingTypes.Should().BeEmpty();
    }
}