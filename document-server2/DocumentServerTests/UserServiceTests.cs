using AutoMapper;
using document_server2.Core.Domain;
using document_server2.Core.Repositories;
using document_server2.Infrastructure.Services;
using document_server2.Infrastructure.Services.JwtToken;
using Moq;
using System;
using System.Threading.Tasks;
using document_server2.Infrastructure.Comends;
using FluentAssertions;
using Xunit;
using document_server2.Infrastructure.DTO;

namespace DocumentServerTests
{
    public class UserServiceTests
    {
        [Theory]
        [InlineData("test@test.com", "tester", "secret", "registered")]
        [InlineData("nowy@mail.com", "tester2", "p@$$w0rd", "registered")]
        public async Task Poprawne_dodanie_użytkownika_do_systemu_przez_administratora(string email, string login, string password, string role)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandlerMock.Object);
            
            var user = new CreateUser()
            {
                Email = email,
                Login = login,
                Password = password,
                Role = role
            };

            await userService.RegisterAsync(user, "registered");

            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("test@@test.com", "tester", "secret", "registered")]
        [InlineData("nowy@mail.com", "t2", "p@$$w0rd", "registered")]
        public async Task Klasa_User_rzuca_wyjątek_po_próbie_zarejestrowania_użytkownika_z_nieprawidłowymi_danymi_na_wejściu
            (string email, string login, string password, string role)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandlerMock.Object);

            var user = new CreateUser()
            {
                Email = email,
                Login = login,
                Password = password,
                Role = role
            };

            await Assert.ThrowsAsync<Exception>(async () => await userService.RegisterAsync(user, "registered"));
        }

        [Fact]
        public async Task Poprawne_pobranie_użytkownika_z_bazy_danych()
        {
            var user = new User("mail@mail.com", "login", "p@$$w0rd", "registered");
            var userRepositoryMock = new Mock<IUserRepository>();
            var accountDTO = new UserDTO()
            {
                Email = "mail@mail.com",
                Login = "login",
                Role_name = "registered"
            };
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<UserDTO>(user)).Returns(accountDTO);
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var userService = new UserService(userRepositoryMock.Object, mapperMock.Object, jwtHandlerMock.Object);
            userRepositoryMock.Setup(x => x.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var userDTO = await userService.GetByEmailAsync(user.Email);

            userRepositoryMock.Verify(x => x.GetByEmailAsync(user.Email), Times.Once);
            userDTO.Should().NotBeNull();
            Assert.Equal(user.Email, userDTO.Email);
        }
    }
}
