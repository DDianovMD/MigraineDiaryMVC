using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace MigraineDiary.Tests.Mocks.Services
{
    public static class UserManagerMock
    {
        public static UserManager<TUser> Instance<TUser>(List<string> assignedRoles, IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;

            Mock<IOptions<IdentityOptions>> options = new Mock<IOptions<IdentityOptions>>();
            IdentityOptions idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);

            List<IUserValidator<TUser>> userValidators = new List<IUserValidator<TUser>>();
            Mock<IUserValidator<TUser>> userValidator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(userValidator.Object);

            List<PasswordValidator<TUser>> pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());

            Mock<UserManager<TUser>> userManager = new Mock<UserManager<TUser>>(store, options.Object, new PasswordHasher<TUser>(),
                                             userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                                             new IdentityErrorDescriber(), null,
                                             new Mock<ILogger<UserManager<TUser>>>().Object);

            userValidator.Setup(v => v.ValidateAsync(userManager.Object, It.IsAny<TUser>()))
                         .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            userManager.Setup(um => um.AddToRoleAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    assignedRoles.Add("TestPassed");
                });

            return userManager.Object;
        }
    }
}
