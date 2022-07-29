using GogNS;
using Train=TrainObject.Train;

namespace gogTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void maxPowerLimited()
        {
            Train chooChoo = new Train(35, 1);
            chooChoo.setPowerCmd(500000000);
            Assert.AreEqual(120000, chooChoo.getPowerCmd(), "Account not debited correctly");

        }
    }
}