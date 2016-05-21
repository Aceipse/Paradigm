using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestForIronScheme;

namespace IronScheme.Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void GetHandler_Text_TextHandler()
        {
            var window = new Window1();

            var handler = window.GetHandler("TEXT");

            Assert.AreEqual(handler.GetType(), typeof(TextHandler));
        }
        [TestMethod]
        public void GetHandler_Error_TextHandler()
        {
            var window = new Window1();

            var handler = window.GetHandler("ERROR");

            Assert.AreEqual(handler.GetType(), typeof(ErrorHandler));
        }
        [TestMethod]
        public void GetHandler_House_ExceptionThrownAndLogged()
        {
            var window = new Window1();

            var handler = window.GetHandler("HOUSE");

            Assert.AreEqual(window.GetDisplayText(), "------------NEW ENTRY\ndatatype 'HOUSE' received from scheme is not supported\n");
        }
    }
}
