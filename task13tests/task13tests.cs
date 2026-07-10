using Xunit;
using task13;
using System.Text.Json;

namespace task13tests;

public class StudentTests
{
    [Fact]
    public void Serialize_ValidStudent_FormatsDateAndIgnoresNull()
    {
        var student = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2007, 1, 10),
            Grades = null
        };

        string json = StudentSerializer.Serialize(student);

        Assert.Contains("\"BirthDate\": \"2007-01-10\"", json);
        Assert.DoesNotContain("Grades", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsCorrectObject()
    {
        string json = @"
        {
            ""FirstName"": ""Ivan"",
            ""LastName"": ""Ivanov"",
            ""BirthDate"": ""2007-01-10"",
            ""Grades"": [
                { ""Name"": ""Math"", ""Grade"": 95 },
                { ""Name"": ""Physics"", ""Grade"": 88 }
            ]
        }";

        var student = StudentSerializer.Deserialize(json);

        Assert.NotNull(student);
        Assert.Equal("Ivan", student.FirstName);
        Assert.Equal(2007, student.BirthDate.Year);
        Assert.Equal(2, student.Grades.Count);
        Assert.Equal(95, student.Grades[0].Grade);
    }

    [Fact]
    public void Deserialize_InvalidGrade_ThrowsArgumentException()
    {
        string invalidJson = @"
        {
            ""FirstName"": ""Ivan"",
            ""LastName"": ""Ivanov"",
            ""BirthDate"": ""2007-01-10"",
            ""Grades"": [ { ""Name"": ""Math"", ""Grade"": 150 } ]
        }";

        var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
        Assert.Contains("Некорректная оценка!", exception.Message);
    }

    [Fact]
    public void Deserialize_EmptyFirstName_ThrowsArgumentException()
    {
        string invalidJson = @"
        {
            ""FirstName"": """",
            ""LastName"": ""Ivanov"",
            ""BirthDate"": ""2007-01-10""
        }";

        var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
        Assert.Contains("Некорректное имя!", exception.Message);
    }

    [Fact]
    public void SaveAndLoad_FileOperations_PreservesDataCorrectly()
    {
        var student = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2007, 1, 10),
            Grades = new List<Subject> { new Subject { Name = "History", Grade = 100 } }
        };
        
        string tempFilePath = Path.Combine("../../../", Path.GetRandomFileName() + ".json");

        try
        {
            StudentSerializer.Upload(tempFilePath, student);
            var loadedStudent = StudentSerializer.Download(tempFilePath);

            Assert.NotNull(loadedStudent);
            Assert.Equal(student.FirstName, loadedStudent.FirstName);
            Assert.Equal(student.LastName, loadedStudent.LastName);
            Assert.Equal(student.BirthDate, loadedStudent.BirthDate);
            Assert.Single(loadedStudent.Grades);
            Assert.Equal(100, loadedStudent.Grades[0].Grade);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
    
    [Fact]
    public void Deserialize_BirthDateIsNumberInsteadOfString_ThrowsJsonException()
    {
        string invalidJson = @"
        {
            ""FirstName"": ""Иван"",
            ""LastName"": ""Иванов"",
            ""BirthDate"": 20070110
        }";

        var exception = Assert.Throws<JsonException>(() => StudentSerializer.Deserialize(invalidJson));
    
        Assert.Contains("Для передачи даты был использован неверный тип данных!", exception.Message);
    }

    
    [Fact]
    public void Deserialize_WrongDateFormat_ThrowsFormatExceptionOrJsonException()
    {
        string invalidJson = @"
        {
            ""FirstName"": ""Ivan"",
            ""LastName"": ""Ivanov"",
            ""BirthDate"": ""10.01.2007""
        }";
        Assert.ThrowsAny<Exception>(() => StudentSerializer.Deserialize(invalidJson));
    }

    [Fact]
    public void Deserialize_UnderageStudent_ThrowsArgumentException()
    {
        string invalidJson = @"
        {
            ""FirstName"": ""Иван"",
            ""LastName"": ""Иванов"",
            ""BirthDate"": ""2010-01-10""
        }";

        var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
        Assert.Contains("Некорректная дата рождения!", exception.Message);
    }
}
