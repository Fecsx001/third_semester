using ELTE.DocuStat.Model;
using ELTE.DocuStat.Persistence;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace DocuStatTestXUnit
{
    public class DocumentStatisticsXUnitTest : IDisposable
    {
        private Mock<IFileManager> _mock;
        private DocumentStatistics _docStats;

        public DocumentStatisticsXUnitTest()
        {
            _mock = new Mock<IFileManager>();
            _docStats = new DocumentStatistics(_mock.Object);
        }

        public void Dispose()
        {
            _mock = null;
            _docStats = null;
        }

        #region Fájl betöltés tesztelése

        [Fact]
        public void Load_SetsFileContentCorrectly()
        {
            // Arrange
            string testContent = "Ez egy teszt szöveg.";
            _mock.Setup(m => m.Load()).Returns(testContent);

            bool fileContentReadyFired = false;
            bool textStatisticsReadyFired = false;
            _docStats.FileContentReady += (s, e) => fileContentReadyFired = true;
            _docStats.TextStatisticsReady += (s, e) => textStatisticsReadyFired = true;

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(testContent, _docStats.FileContent);
            Assert.True(fileContentReadyFired, "FileContentReady event should be fired");
            Assert.True(textStatisticsReadyFired, "TextStatisticsReady event should be fired");
        }

        [Fact]
        public void Load_ThrowsException_WhenFileManagerThrows()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Throws(new FileManagerException("File not found"));

            // Act & Assert
            Assert.Throws<System.IO.IOException>(() => _docStats.Load());
        }

        #endregion

        #region Szó számláló tesztelése

        [Fact]
        public void DistinctWordCount_EmptyString_ReturnsEmptyDictionary()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("");

            // Act
            _docStats.Load();

            // Assert
            Assert.Empty(_docStats.DistinctWordCount);
        }

        [Fact]
        public void DistinctWordCount_OnlyNonLetterCharacters_ReturnsEmptyDictionary()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("123 !@# $%^ &*()");

            // Act
            _docStats.Load();

            // Assert
            Assert.Empty(_docStats.DistinctWordCount);
        }

        [Fact]
        public void DistinctWordCount_RepeatedWord_CountsCorrectly()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("alma alma alma");

            // Act
            _docStats.Load();

            // Assert
            Assert.Single(_docStats.DistinctWordCount);
            Assert.Equal(3, _docStats.DistinctWordCount["alma"]);
        }

        [Fact]
        public void DistinctWordCount_WordWithNonLetterCharacters_CountsCorrectly()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("(alma) [alma] {alma}");

            // Act
            _docStats.Load();

            // Assert
            Assert.Single(_docStats.DistinctWordCount);
            Assert.Equal(3, _docStats.DistinctWordCount["alma"]);
        }

        [Fact]
        public void DistinctWordCount_MixedCase_CountsAsSameWord()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("Alma aLma ALMA alma");

            // Act
            _docStats.Load();

            // Assert
            Assert.Single(_docStats.DistinctWordCount);
            Assert.Equal(4, _docStats.DistinctWordCount["alma"]);
        }

        [Fact]
        public void DistinctWordCount_MultipleDifferentWords_CountsAllCorrectly()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("alma körte alma barack körte alma");

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(3, _docStats.DistinctWordCount.Count);
            Assert.Equal(3, _docStats.DistinctWordCount["alma"]);
            Assert.Equal(2, _docStats.DistinctWordCount["körte"]);
            Assert.Equal(1, _docStats.DistinctWordCount["barack"]);
        }

        #endregion

        #region Egyéb számlálók tesztelése

        [Fact]
        public void CharacterCount_GeneralText_ReturnsCorrectLength()
        {
            // Arrange
            string testContent = "Ez egy teszt szöveg.";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(testContent.Length, _docStats.CharacterCount);
        }

        [Fact]
        public void CharacterCount_EmptyString_ReturnsZero()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("");

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(0, _docStats.CharacterCount);
        }

        [Fact]
        public void NonWhiteSpaceCharacterCount_GeneralText_ReturnsCorrectCount()
        {
            // Arrange
            string testContent = "Ez egy teszt.";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(11, _docStats.NonWhiteSpaceCharacterCount);
        }

        [Fact]
        public void NonWhiteSpaceCharacterCount_OnlyWhitespace_ReturnsZero()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("   \t\n\r");

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(0, _docStats.NonWhiteSpaceCharacterCount);
        }

        [Fact]
        public void SentenceCount_GeneralText_ReturnsCorrectCount()
        {
            // Arrange
            string testContent = "Ez az első mondat. Ez a második! Ez a harmadik?";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(3, _docStats.SentenceCount);
        }

        [Fact]
        public void SentenceCount_EmptyString_ReturnsZero()
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns("");

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(0, _docStats.SentenceCount);
        }

        [Fact]
        public void ProperNounCount_CapitalLettersInMiddle_CountsCorrectly()
        {
            // Arrange
            string testContent = "Ez egy Teszt szöveg ahol Budapest és New York is szerepel.";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(2, _docStats.ProperNounCount);
        }

        [Fact]
        public void ProperNounCount_MultipleSentences_CountsCorrectly()
        {
            // Arrange
            string testContent = "Első mondat. Második mondat! Harmadik mondat?";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.Equal(3, _docStats.ProperNounCount);
        }

        #endregion

        #region További tesztesetek

        [Fact]
        public void ColemanLieuIndex_MultiSentenceText_ReturnsCorrectValue()
        {
            // Arrange
            string testContent = "Ez egy hosszabb szöveg, amely több mondatot is tartalmaz. " +
                               "Itt van egy második mondat is. És itt egy harmadik!";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.True(_docStats.ColemanLieuIndex > 0, "Coleman-Lieu index should be positive");
        }

        [Fact]
        public void ColemanLieuIndex_SingleSentence_ReturnsCorrectValue()
        {
            // Arrange
            string testContent = "Ez egyetlen mondat.";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.True(_docStats.ColemanLieuIndex != 0, "Coleman-Lieu index should not be zero");
        }

        [Fact]
        public void FleschReadingEase_MultiSentenceText_ReturnsCorrectValue()
        {
            // Arrange
            string testContent = "Ez egy olvasmányos szöveg, amely több mondatot tartalmaz. " +
                               "A Flesch Reading Ease értéke jól méri a szöveg olvashatóságát.";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.True(_docStats.FleschReadingEase > 0, "Flesch Reading Ease should be positive");
        }

        [Fact]
        public void FleschReadingEase_TextWithoutVowels_ReturnsCorrectValue()
        {
            // Arrange
            string testContent = "szív szív szív";
            _mock.Setup(m => m.Load()).Returns(testContent);

            // Act
            _docStats.Load();

            // Assert
            Assert.True(_docStats.FleschReadingEase >= 0, "Flesch Reading Ease should be non-negative");
        }

        #endregion

        #region Paraméterezett tesztesetek xUnit-ben

        [Theory]
        [InlineData("Ez egy teszt szöveg", 18)]
        [InlineData("", 0)]
        [InlineData("Hello", 5)]
        [InlineData("   ", 3)]
        [InlineData("a b c", 5)]
        public void CharacterCount_VariousInputs_ReturnsCorrectLength(string input, int expected)
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns(input);
            
            // Act
            _docStats.Load();
            
            // Assert
            Assert.Equal(expected, _docStats.CharacterCount);
        }

        [Theory]
        [InlineData("alma", 1)]
        [InlineData("alma alma", 1)]
        [InlineData("alma körte", 2)]
        [InlineData("alma körte barack", 3)]
        [InlineData("", 0)]
        public void DistinctWordCount_VariousInputs_ReturnsCorrectCount(string input, int expectedWordCount)
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns(input);
            
            // Act
            _docStats.Load();
            
            // Assert
            Assert.Equal(expectedWordCount, _docStats.DistinctWordCount.Count);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("a", 1)]
        [InlineData("Ez egy teszt.", 11)]
        [InlineData("  spaces  ", 6)]
        public void NonWhiteSpaceCharacterCount_VariousInputs_ReturnsCorrectCount(string input, int expected)
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns(input);
            
            // Act
            _docStats.Load();
            
            // Assert
            Assert.Equal(expected, _docStats.NonWhiteSpaceCharacterCount);
        }

        [Theory]
        [InlineData("alma körte", 2)]
        [InlineData("alma alma körte", 2)]
        [InlineData("alma körte barack", 3)]
        public void DistinctWordCount_VariousWordCombinations_ReturnsCorrectCount(string input, int expectedWordCount)
        {
            // Arrange
            _mock.Setup(m => m.Load()).Returns(input);
            
            // Act
            _docStats.Load();
            
            // Assert
            Assert.Equal(expectedWordCount, _docStats.DistinctWordCount.Count);
        }

        #endregion
    }
}