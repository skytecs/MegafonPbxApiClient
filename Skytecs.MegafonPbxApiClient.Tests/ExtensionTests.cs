using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skytecs.MegafonPbxApiClient.Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void AddMegafon_AddsMegafonServicesToGlobalScope()
        {
            var services = new ServiceCollection();

            MegafonApiOptions configuredOptions = null;

            var newServices = MegafonClientExtensions.AddMegafon(services, options =>
            {
                configuredOptions = options;
            });

            Assert.AreEqual(services, newServices);
            Assert.NotNull(configuredOptions);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetService<IMegafonApiClient>());
            Assert.NotNull(provider.GetService<MegafonApiOptions>());

            Assert.AreEqual(configuredOptions, provider.GetService<MegafonApiOptions>());
        }
    }
}
