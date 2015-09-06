using System;
using Kaos_TicTacToe_Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaos_TicTacToe_Test
{
    [TestClass]
    public class UnitTestUser
    {
        //þetta Unit Test er með öllu ónauðsynlegt
        [TestMethod]
        public void TestUserConstructor()
        {
            User user1 = new User("Testguy");
            User user2 = new User("Testgal");

            Assert.AreEqual(user1.Name, "Testguy");
            Assert.AreNotSame(user1, user2);
            Assert.AreNotEqual(user1.Name, user2.Name);
        }
    }
}
