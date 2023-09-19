// See https://aka.ms/new-console-template for more information
using Parquet;
using Parquet.Rows;
using Parquet.Schema;
using Parquet.Serialization;
using ParquetTest;

string basePath = @"R:\";
int seed = 0;
int count = 1_000;

Console.WriteLine("Hello, World!");

// Table serialization
var myRandom = new Random(seed);
var schema = new ParquetSchema(
    new DataField<int>("Id"),
    new DataField<string>("Name"),
    new DataField<DateOnly>("Date"),
    new DataField<double>("Number"),
    new StructField("Leader",
        new DataField<string>("Name"),
        new DataField<int>("Age")
    ),
    new DataField<int>("EmpCount"),
    new ListField("Employees", new StructField("Employee",
        new DataField<string>("Name"),
        new DataField<int>("Age")
    ))
);
var table = new Table(schema);

for (int i = 0; i < count; i++)
{
    var person = new Person(
        myRandom.GetRandomString(myRandom.Next(4, 20)),
        myRandom.Next(18, 70)
    );
    var employees = Enumerable.Range(0, myRandom.Next(3))
        .Select(_ => new Person(
            myRandom.GetRandomString(myRandom.Next(4, 20)),
            myRandom.Next(18, 70)
        ))
        .ToList();
    table.Add(new Row(
        i + 1,
        myRandom.GetRandomString(),
        DateOnly.FromDateTime(DateTime.Now),
        (myRandom.NextDouble() - .5) * 1000,
        new Row(
            person.Name,
            person.Age
        ),
        employees.Count,
        employees
            .Select(person => new Row(
                person.Name,
                person.Age
            ))
            .ToArray()
    ));
}
await table.WriteAsync(Path.Combine(basePath, "outputTable.parquet"));

// Class serialization
myRandom = new Random(seed);
var data = new List<Test>();
for (int i = 0; i < 1_000; i++)
{
    var person = new Person(
        myRandom.GetRandomString(myRandom.Next(4, 20)),
        myRandom.Next(18, 70)
    );
    var employees = Enumerable.Range(0, myRandom.Next(3))
        .Select(_ => new Person(
            myRandom.GetRandomString(myRandom.Next(4, 20)),
            myRandom.Next(18, 70)
        ))
        .ToList();
    data.Add(new()
    {
        Id = i + 1,
        Name = myRandom.GetRandomString(),
        Date = DateOnly.FromDateTime(DateTime.Now),
        Number = (myRandom.NextDouble() - .5) * 1000,
        Leader = person,
        EmpCount = employees.Count,
        Employees = employees
    });
}

await ParquetSerializer.SerializeAsync(data, Path.Combine(basePath, "outputClass.parquet"));
