using GogNS;
using TrackModel;

namespace gogTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        [TestMethod]
        public void TrackModel_AddTrain()
        {
            int blockIdx = 0;
            int lineIdx = 0;
            int authority = 0;

            TrackModel.MainWindow track = new TrackModel.MainWindow();
            track.AddTrain(blockIdx, lineIdx, authority);

            Assert.AreEqual(track.mtrainList.Count, 1);
            Assert.AreEqual(track.mtrainList[0].blockIdx, 0);
            Assert.AreEqual(track.mtrainList[0].lineIdx, 0);
            Assert.AreEqual(track.mtrainList[0].commAuthority, 0);
        }
    }
}