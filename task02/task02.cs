namespace task02;

public class Student
{
    public string Name { get; set; }
    public string Faculty { get; set; }
    public List<int> Grades { get; set; }

}

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    => _students.Where(student => student.Faculty == faculty);

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    => _students.Where(student => student.Grades.Sum()/student.Grades.Count >= minAverageGrade);

    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(student => student.Name);

    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(student => student.Faculty);

    public string GetFacultyWithHighestAverageGrade()
      => _students.GroupBy(student => student.Faculty).OrderBy(group => group.Average(student => student.Grades.Average()))
         .Select(grup => grup.Key)
          .Last() ?? string.Empty;
}
