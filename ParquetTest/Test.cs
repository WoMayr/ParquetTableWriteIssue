namespace ParquetTest;

public class Test
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public double Number { get; set; }
    public Person Leader { get; set; }
    public int EmpCount { get; set; }
    public List<Person> Employees { get; set; }
}
