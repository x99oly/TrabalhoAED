string? pathEntradaTxt = FileHandler.GetFilePath()
    ?? throw new FileNotFoundException("Arquivo entrada.txt não foi encontrado no projeto.");

Dictionary<int, Course> courses;
Applicant[] applicants;

ProcessEntrance(pathEntradaTxt, out courses, out applicants);

Console.WriteLine($"count courses: {courses.Count}, count applicants: {applicants.Length}");


static void ProcessEntrance(string path, out Dictionary<int, Course> courses, out Applicant[] applicants)
{
    var lines = File.ReadAllLines(path);

    var header = lines[0].Split(';');
    int coursesCount = int.Parse(header[0]);
    int applicantsCount = int.Parse(header[1]);

    applicants = new Applicant[applicantsCount];
    courses = new Dictionary<int, Course>();

    for (int i = 0; i < coursesCount; i++)
    {
        var parts = lines[i + 1].Split(';');
        int id = int.Parse(parts[0]);
        string name = parts[1];
        int vacancies = int.Parse(parts[2]);

        courses[id] = new Course(id, name, vacancies);
    }

    int startApplicantsLine = 1 + coursesCount;
    for (int i = 0; i < applicantsCount; i++)
    {
        var parts = lines[startApplicantsLine + i].Split(';');

        string name = parts[0];
        double l = double.Parse(parts[1]);
        double m = double.Parse(parts[2]);
        double e = double.Parse(parts[3]);
        int o1 = int.Parse(parts[4]);
        int o2 = int.Parse(parts[5]);

        applicants[i] = new Applicant(name, l, m, e, o1, o2);
    }
}

public class Applicant(string n, double l, double m, double e, int o1, int o2)
{
    public string Name { get; } = n;

    public int Op1 { get; } = o1;

    public int Op2 { get; } = o2;
    public TestResult Result { get; } = new(l, m, e);
}

public class TestResult(double l, double m, double e)
{
    public double Languages { get; } = l;
    public double Math { get; } = m;
    public double Essay { get; } = e;
}

public class Course(int id, string n, int v)
{
    public int Id { get; } = id;
    public string Name { get; } = n;
    public int Vacancies { get; } = v;

}

public static class FileHandler
{    
    public static string? GetFilePath()
    {
        int counterLimit = 5;
        string currentDir = Directory.GetCurrentDirectory();
        return GetFilePath(counterLimit, currentDir, "entrada.txt");
    }

    static string? GetFilePath(int cl, string? currentDir, string fileName)
    {
        if (cl == 0 || currentDir == null)
            return null;

        string filePath = Path.Combine(currentDir, fileName);
        if (File.Exists(filePath))
            return filePath;

        string? parentDir = Directory.GetParent(currentDir)?.FullName;
        return GetFilePath(cl - 1, parentDir, fileName);
    }
}