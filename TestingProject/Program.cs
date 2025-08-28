namespace TestingProject;

public record Record(int Id, string Name, DateTime CreatedDateTime);

public class Program
{
    public static void Main()
    {
        DateTime currentDateTime = DateTime.UtcNow;
        Record record1 = new(1, "A", DateTime.UtcNow);
        Record record2 = new(1, "A", DateTime.UtcNow);

        Console.WriteLine(record1 == record2);
    }
}