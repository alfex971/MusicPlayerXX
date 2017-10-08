using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicPlayerXX;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestPlay()
        {
            VideoViewModel video = new VideoViewModel();
            video.PlayVideoAction();
        }
        [TestMethod]
        public void TestStop()
        {
            VideoViewModel video = new VideoViewModel();
            video.StopVideoAction();
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestOpenFile()
        {
            VideoViewModel video = new VideoViewModel();
            video.StopVideoAction();
        }
    }
}
