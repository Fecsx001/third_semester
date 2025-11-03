using ELTE.DocuStat.Model;
using ELTE.DocuStat.Persistence;
using Moq;

namespace DocuStatUnittest;
/*
public class MockFileManager : IFileManager
{
    public string Content { get; set; } = string.Empty;
    public bool ShouldThrowException { get; set; } = false;

    public string Load()
    {
        if (ShouldThrowException)
        {
            throw new FileManagerException();
        }
        return Content; 
    }
}
*/
[TestClass]
public sealed class DocumentStatisticsTest
{

    
    private DocumentStatistics _docStats = null;
    private Mock<IFileManager> _mock = null;
    [TestInitialize]
    public void TestInitialize()
    {
        _mock = new Mock<IFileManager>();
        _docStats = new DocumentStatistics(_mock.Object);
    }
    
    [TestMethod]
    public void TestLoadFileContent()
    {
        //Arrange
        _mock.Setup(m => m.Load()).Returns("Hello");

        //Act
        _docStats.Load();
        
        //Assert
        Assert.AreEqual("Hello", _docStats.FileContent);
    }

    [TestMethod]
    public void TestLoadFileContentError()
    {
        _mock.Setup(m => m.Load()).Throws<FileManagerException>();
        
        Assert.ThrowsException<FileManagerException>(() => _docStats.Load());
    }

    [TestMethod]
    public void TestLoadEmpty()
    {
        //Arrange
        _mock.Setup(m => m.Load()).Returns("Hello");
        
        bool fileContentReadyRaised = false;
        bool textStatisticsReadyRaised = false;
        
        _docStats.FileContentReady += (sender, args) => fileContentReadyRaised = true;
        _docStats.TextStatisticsReady += (sender, args) => textStatisticsReadyRaised = true;

        //Act
        _docStats.Load();   
        
        //Assert
        Assert.IsTrue(fileContentReadyRaised);
        Assert.IsTrue(textStatisticsReadyRaised);
    }

    [TestMethod]
    public void TestWordCount_EmptyDictionary()
    {
        _mock.Setup(m => m.Load()).Returns("");
        
        _docStats.Load();
        
        Assert.AreEqual("", _docStats.FileContent);
    }

    [TestMethod]
    public void TestWordCount_NotWords()
    {
        _mock.Setup(m => m.Load()).Returns("123 @&# |||| ()");
        
        _docStats.Load();
        
        Assert.AreEqual(0, _docStats.DistinctWordCount.Count);
    }

    [TestMethod]
    public void TestWordCount_MultipleOfTheSameWord()
    {
        _mock.Setup(m => m.Load()).Returns("alma alma alma alma alma alma alma");
        
        _docStats.Load();
        
        Assert.AreEqual(1, _docStats.DistinctWordCount.Count);
    }

    [TestMethod]
    public void TestWordCount_MultipleOfSameWOrdWithNumbers()
    {
        _mock.Setup(m => m.Load()).Returns("[alma] {alma} (alma)");
        
        _docStats.Load();
        
        Assert.AreEqual(1, _docStats.DistinctWordCount.Count);
    }

    [TestMethod]
    public void TestWordCount_MultipleOfSameWordsCapital()
    {
        _mock.Setup(m => m.Load()).Returns("Alma alma ALMA");
        
        _docStats.Load();
        
        Assert.AreEqual(1, _docStats.DistinctWordCount.Count);
    }

    [TestMethod]
    public void TestWordCount_General()
    {
        // Arrange
        _mock.Setup(m => m.Load()).Returns("alma körte alma barack körte alma");

        // Act
        _docStats.Load();

        // Assert
        Assert.AreEqual(3, _docStats.DistinctWordCount.Count);
        Assert.AreEqual(3, _docStats.DistinctWordCount["alma"]);
        Assert.AreEqual(2, _docStats.DistinctWordCount["körte"]);
        Assert.AreEqual(1, _docStats.DistinctWordCount["barack"]);
    }

    [TestMethod]
    public void TestCharCount_Generic()
    {
        // Arrange
        string testContent = "Ez egy teszt szöveg.";
        _mock.Setup(m => m.Load()).Returns(testContent);

        // Act
        _docStats.Load();

        // Assert
        Assert.AreEqual(testContent.Length, _docStats.CharacterCount);
    }

    [TestMethod]
    public void TestCharCount_Empty()
    {
        _mock.Setup(m => m.Load()).Returns("");
        
        _docStats.Load();
        
        Assert.AreEqual(0, _docStats.CharacterCount);
    }

    [DataTestMethod]
    [DataRow("Ez egy teszt szöveg", 19)]
    [DataRow("", 0)]
    public void TestCharCount(string input, int expected)
    {
        _mock.Setup(m => m.Load()).Returns(input);
        
        _docStats.Load();
        
        Assert.AreEqual(expected, _docStats.CharacterCount);
    }
}