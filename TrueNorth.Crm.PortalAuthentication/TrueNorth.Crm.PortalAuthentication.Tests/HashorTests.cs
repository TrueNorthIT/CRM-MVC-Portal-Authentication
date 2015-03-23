using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrueNorth.Crm.FieldHasher;

namespace TrueNorth.Crm.PortalAuthentication.Tests
{
    [TestClass]
    public class HashorTests
    {
        [TestMethod]
        public void HashPassword()
        {
            var hash = Hashor.HashPassword("Password2");

            var result = Hashor.ValidatePassword("Password2", hash);

            Assert.IsTrue(result);


        }
    }
}
