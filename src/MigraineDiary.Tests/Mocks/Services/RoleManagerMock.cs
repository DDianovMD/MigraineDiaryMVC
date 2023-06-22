using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace MigraineDiary.Tests.Mocks.Services
{
    public class RoleManagerMock
    {
        public static RoleManager<TRole> Instance<TRole>(List<int> rolesCount, IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            
            List<IRoleValidator<TRole>> roleValidators = new List<IRoleValidator<TRole>>();
            roleValidators.Add(new RoleValidator<TRole>());

            var roleManager = new Mock<RoleManager<TRole>>(store, 
                                                           roleValidators, 
                                                           MockLookupNormalizer(),
                                                           new IdentityErrorDescriber(),
                                                           new Mock<ILogger<RoleManager<TRole>>>().Object);

            roleManager.Setup(rm => rm.CreateAsync(It.IsAny<TRole>())).Callback(() =>
            {
                rolesCount.Add(1);
            });

            roleManager.Setup(rm => rm.DeleteAsync(It.IsAny<TRole>())).Callback(() =>
            {
                rolesCount.RemoveAt(rolesCount.Count - 1);
            });

            return roleManager.Object;
        }

        private static ILookupNormalizer MockLookupNormalizer()
        {
            var normalizerFunc = new Func<string, string>(i =>
            {
                if (i == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(i)).ToUpperInvariant();
                }
            });
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            lookupNormalizer.Setup(i => i.NormalizeName(It.IsAny<string>())).Returns(normalizerFunc);
            lookupNormalizer.Setup(i => i.NormalizeEmail(It.IsAny<string>())).Returns(normalizerFunc);
            return lookupNormalizer.Object;
        }
    }
}
