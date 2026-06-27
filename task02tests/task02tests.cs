using Xunit;
using task02;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

    [Fact]
    public void GetStudentsWithGradesHigherAverageGrades_ReturnsCorrectStudents()
    {
        double minAverageGrade = 4.0;
    
        var result = _service.GetStudentsWithMinAverageGrade(minAverageGrade).ToList();
    
        Assert.Equal(2, result.Count);
    
        Assert.Contains(result, s => s.Name == "Иван");
        Assert.Contains(result, s => s.Name == "Петр");
    }

    [Fact]
    public void GroupStudentsByFaculty()
    {
        var result = _service.GroupStudentsByFaculty().ToList();
        Assert.Equal(2, result[0].Count());
        Assert.Equal("ФИТ", result[0].Key);
        Assert.Equal("ФИТ", result[0].Key);
        Assert.Single(result[1]);
        Assert.Equal("Экономика", result[1].Key);
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsCorrect()
    {
        var result = _service.GetStudentsOrderedByName();
        Assert.Equal(3, result.Count());
        Assert.Equal("Анна", result.Select(student => student.Name).First());
        Assert.Equal("Петр", result.Select(student => student.Name).Last());
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }
}
