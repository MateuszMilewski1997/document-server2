using AutoMapper;
using document_server2.Core.Domain;
using document_server2.Core.Repositories;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.Services;
using document_server2.Infrastructure.Services.JwtToken;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Poprawna_rejestracja_użytkownika_do_systemu()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandler = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandler.Object);
            var data = new CreateUser()
            {
                Email = "test@test.com",
                Login = "tester",
                Password = "secret",
                Role = "user"
            };
            // Act
            await userService.RegisterAsync(data);
            // Assert
            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Niepoprawana_rejestracja_użytkownika_do_systemu_Istnieje_już_inny_użytkownik_o_podanym_emailu()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandler = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandler.Object);
            var user = new User("test@test.com", "tester", "secret", "user");
            var data = new CreateUser()
            {
                Email = "test@test.com",
                Login = "tester",
                Password = "secret",
                Role = "user"
            };
            // Act
            userRepositoryMock.Setup(x => x.GetByEmailAsync(data.Email)).ReturnsAsync(user);
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await userService.RegisterAsync(data));
        }

        [Fact]
        public async Task Niepoprawne_próba_rejestracji_użytkownika_Brak_podanego_hasła()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandler = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandler.Object);
            var data = new CreateUser()
            {
                Email = "test@test.com",
                Login = "tester",
                Role = "user"
            };
            // Act
            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(async () =>
                await userService.RegisterAsync(data));
            Assert.Equal("Can not have an empty password.", exception.Message);
        }

        [Fact]
        public async Task Niepoprawne_próba_rejestracji_użytkownika_Brak_podanego_loginu()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandler = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandler.Object);
            var data = new CreateUser()
            {
                Email = "test@test.com",
                Password = "secret",
                Role = "user"
            };
            // Act
            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(async () =>
               await userService.RegisterAsync(data));
            Assert.Equal("Can not have an empty login.", exception.Message);
        }
    }
}
