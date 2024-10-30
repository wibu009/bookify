using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using Bookify.Shared.Authorization;

namespace Bookify.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
     [Fact]
     public void Create_Should_SetPropertyValues()
     {
          // Act
          var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
          
          // Assert
          user.FirstName.Should().Be(UserData.FirstName);
          user.LastName.Should().Be(UserData.LastName);
          user.Email.Should().Be(UserData.Email);
     }

     [Fact]
     public void Create_Should_RaiseUserCreatedDomainEvent()
     {
          // Act
          var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
          
          // Assert
          var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);
          domainEvent.UserId.Should().Be(user.Id);
     }

     [Fact]
     public void Create_Should_AddRegisteredRoleToUser()
     {
          // Act
          var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
          
          // Assert
          user.Roles.Should().Contain(new Role(1, Roles.Basic));
     }
}