using document_server2.Core.Domain;
using document_server2.Core.Domain.Context;
using document_server2.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task Pobranie_użytkownika_po_adresie_email_którego_nie_ma_w_bazie_danych_Zostanie_zwrócony_null()
        {
            // Arrange
            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            DataBaseContext context = new DataBaseContext(options);
            User user = new User("test@test.com", "test", "secret", "user");
            UserRepository userRepository = new UserRepository(context);
            // Act
            var new_user = await userRepository.GetByEmailAsync(user.Email);
            // Assert
            Assert.Null(new_user);
        }

        [Fact]
        public async Task Pobranie_użytkownika_po_adresie_email_który_już_istnieje_w_bazie_danych_Zostanie_poprawnie_zwrócony()
        {
            // Arrange
            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            DataBaseContext context = new DataBaseContext(options);
            User user = new User("test@test.com", "test", "secret", "user");
            UserRepository userRepository = new UserRepository(context);
            // Act
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var new_user = await userRepository.GetByEmailAsync(user.Email);
            // Assert
            Assert.NotNull(new_user);
            Assert.Equal(user, new_user);
        }

        [Fact]
        public async Task Poprawne_dawanie_do_bazy_danych_nowego_użytkownika()
        {
            // Arrange
            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            DataBaseContext context = new DataBaseContext(options);
            User user = new User("test@test.com", "test", "secret", "user");
            UserRepository userRepository = new UserRepository(context);
            // Act
            await userRepository.AddAsync(user);
            var any_user = await context.Users.SingleOrDefaultAsync(x => x.Email == user.Email);
            // Assert
            Assert.Equal(user, any_user);
        }

        [Fact]
        public async Task Niepoprawne_dawanie_do_bazy_danych_nowego_użytkownika__Dwukrotna_próba_podania_tych_samych_danych()
        {
            // Arrange
            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            DataBaseContext context = new DataBaseContext(options);
            User user = new User("test@test.com", "test", "secret", "user");
            UserRepository userRepository = new UserRepository(context);
            // Act
            await userRepository.AddAsync(user);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await userRepository.AddAsync(user));
        }

        [Fact]
        public async Task Poprawna_aktualizacja_danych_konta_użytkownika_będącego_w_bazie_danych()
        {
            // Arrange
            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            DataBaseContext context = new DataBaseContext(options);
            User user = new User("test@test.com", "test", "secret", "user");
            UserRepository userRepository = new UserRepository(context);
            // Act
            context.Users.Add(user);
            await context.SaveChangesAsync();

            PropertyInfo email = typeof(User).GetProperty("Password");
            email.SetValue(user, "secret2", null);
            PropertyInfo role = typeof(User).GetProperty("Role_name");
            role.SetValue(user, "user2", null);
            PropertyInfo login = typeof(User).GetProperty("Login");
            login.SetValue(user, "test2", null);

            await userRepository.UpdateAsync(user);
            // Assert
            Assert.Equal("secret2", user.Password);
            Assert.Equal("user2", user.Role_name);
            Assert.Equal("test2", user.Login);
        }
    }
}
