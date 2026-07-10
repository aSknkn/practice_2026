using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public static class StudentSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = {new DateTimeConverter()},
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, Options);
    }
    public static Student Deserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, Options);

        student?.IsValid(); 
        
        return student;
    }

    public static void Upload(string filePath, Student student)
    {
        string json = Serialize(student);
        File.WriteAllText(filePath, json);
    }

    public static Student Download(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return Deserialize(json);
    }
}
