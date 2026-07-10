namespace task13;

public class Subject
{
    public string Name { get; set; }
    public int Grade { get; set; }
}

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject> Grades { get; set; }

    public void IsValid()
    {
        if (string.IsNullOrWhiteSpace(FirstName)) 
            throw new ArgumentException("Некорректное имя!");
            
        if (string.IsNullOrWhiteSpace(LastName)) 
            throw new ArgumentException("Некорректная фамилия!");
            
        if (BirthDate > DateTime.Today.AddYears(-18)) 
            throw new ArgumentException("Некорректная дата рождения!");
        
        if (Grades != null)
        {
            foreach (var subj in Grades)
            {
                if (string.IsNullOrWhiteSpace(subj.Name)) 
                    throw new ArgumentException("Некорректное название предмета!");
                    
                if (subj.Grade > 100 || subj.Grade < 0) 
                    throw new ArgumentException("Некорректная оценка!");
            }
        }
    }
}