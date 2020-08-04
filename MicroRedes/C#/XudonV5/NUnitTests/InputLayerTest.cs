using GUIXudon.Common;
using NUnit.Framework;
using XudonV4NetFramework.Structure;

namespace Tests
{
    public class Tests
    {
        private Xudon _xudon;

        private DataManager _dataManager;

        [SetUp]
        public void Setup()
        {
            _dataManager = new DataManager(@"..\..\..\..\XudonV2_6\GUIXudon\Resources\Data.csv");

            _xudon = new Xudon(_dataManager.CloseDBAndDataFile,
                               _dataManager.ReadLineInDataFile,
                               _dataManager.GetEndOfLine,
                               _dataManager.GetLastLineReadInDataFile,
                               _dataManager.WriteInDB,
                               _dataManager.AllHeadersIDs,
                               _dataManager.InputHeadersIDs,
                               _dataManager.OutputHeadersIDs);

            _xudon.RunXudonThread(true);
        }

        [Test]
        //[TestCase(parameters for test)]
        [Description("Test for...")]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}