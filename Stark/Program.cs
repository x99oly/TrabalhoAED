using System.Text;

string? pathEntradaTxt = FileHandler.GetFilePath()
    ?? throw new FileNotFoundException("Arquivo entrada.txt não foi encontrado no projeto.");

var manager = new AdmissionManager(pathEntradaTxt);

manager.GenerateOutput();

class AdmissionManager
{
    public Dictionary<int, Course> Courses { get; private set; }
    public List<Applicant> Applicants { get; private set; }

    public AdmissionManager(string inputPath)
    {
        ProcessEntrance(inputPath);
        Applicants.ApplicantsSort();
        DistributeApplicants();
    }

    private void ProcessEntrance(string path)
    {
        var lines = File.ReadAllLines(path);
        var header = lines[0].Split(';');
        int coursesCount = int.Parse(header[0]);
        int applicantsCount = int.Parse(header[1]);

        Courses = new Dictionary<int, Course>();
        Applicants = new List<Applicant>();

        for (int i = 0; i < coursesCount; i++)
        {
            var parts = lines[i + 1].Split(';');
            int id = int.Parse(parts[0]);
            string name = parts[1];
            int vacancies = int.Parse(parts[2]);
            Courses[id] = new Course(id, name, vacancies);
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
            Applicants.Add(new Applicant(name, l, m, e, o1, o2));
        }
    }

    private void DistributeApplicants()
    {
        foreach (var applicant in Applicants)
        {
            var course1 = Courses[applicant.Op1.Choice];
            var course2 = Courses[applicant.Op2.Choice];

            if (course1.IsApplicantAbleToApprovedlist())
                course1.PushNewApplicant(applicant);
            else
            {
                course2.PushNewApplicant(applicant);
                course1.PushNewApplicant(applicant);
            }
        }
    }

    public void GenerateOutput(string outputFileName = "saida.txt")
    {
        FileHandler.GenerateOutputFile(Courses, outputFileName);
    }
}

public class Applicant
(string n, double l, double m, double e, int o1, int o2)
: IComparable<Applicant>
{
    public string Name { get; } = n;
    public CourseOption Op1 { get; } = new(o1);
    public CourseOption Op2 { get; } = new(o2);
    public TestResult Result { get; } = new TestResult(l, m, e);

    public double GetAvarage() => Result.AvarageGrade;

    public double GetEssay() => Result.Essay;

    public int CompareTo(Applicant? other)
    {
        if (other is null) return 1;

        int res = GetAvarage().CompareTo(other.GetAvarage());
        if (res == 0)
            res = GetEssay().CompareTo(other.GetEssay());
        return res;
    }
}

public class CourseOption
(int choice)
{
    public int Choice { get; } = choice;
    public ApplicantStatus Status { get; set; } = ApplicantStatus.PENDING;

}

public enum ApplicantStatus
{
    APPROVED = 1,
    DECLINED = -1,
    PENDING = 0
}

public class TestResult
{
    public double Languages { get; }
    public double Math { get; }
    public double Essay { get; }
    public double AvarageGrade { get; }

    public TestResult(double l, double m, double e)
    {
        Languages = l;
        Math = m;
        Essay = e;
        AvarageGrade = (Languages + Math + Essay) / 3;
    }
}

public class Course(int id, string n, int v)
{
    public int Id { get; } = id;
    public string Name { get; } = n;
    public int Vacancies { get; } = v;
    public FilaFlex<Applicant> Waitlist { get; } = new();
    public List<Applicant> Approvedlist { get; } = new();
    public void PushNewApplicant(Applicant a)
    {
        if (!IsApplicantAbleToApprovedlist())
            Waitlist.Botar(a);
        else
            Approvedlist.Add(a);
    }

    public bool IsApplicantAbleToApprovedlist()
    {
        return Approvedlist.Count < Vacancies;
    }
}

public static class FileHandler
{
    public static string? GetFilePath()
    {
        int counterLimit = 5;
        string currentDir = Directory.GetCurrentDirectory();
        return GetFilePath(counterLimit, currentDir, "entrada.txt");
    }

    private static string? GetFilePath(int cl, string? currentDir, string fileName)
    {
        if (cl == 0 || currentDir == null)
            return null;

        string filePath = Path.Combine(currentDir, fileName);
        if (File.Exists(filePath))
            return filePath;

        string? parentDir = Directory.GetParent(currentDir)?.FullName;
        return GetFilePath(cl - 1, parentDir, fileName);
    }

    public static void GenerateOutputFile(Dictionary<int, Course> courses, string outputFileName = "saida.txt")
    {
        string? outputPath = FileHandler.GetFilePath();
        if (outputPath == null)
            throw new FileNotFoundException("Arquivo de entrada não encontrado para definir caminho de saída.");

        string outputDir = Path.GetDirectoryName(outputPath)!;
        string fullOutputPath = Path.Combine(outputDir, outputFileName);

        var sb = new StringBuilder();

        foreach (var course in courses.Values)
        {
            var aprovados = course.Approvedlist;

            double notaCorte = 0;
            if (aprovados.Count > 0)
                notaCorte = aprovados[aprovados.Count - 1].Result.AvarageGrade;

            sb.AppendLine($"{course.Name} {notaCorte.ToString("F2").Replace('.', ',')}");

            sb.AppendLine("Selecionados");
            for (int i = 0; i < aprovados.Count; i++)
            {
                var a = aprovados[i];
                sb.AppendLine($"{a.Name} {a.Result.AvarageGrade.ToString("F2").Replace('.', ',')}");
            }

            sb.AppendLine("Fila de Espera");
            // Para iterar a fila de espera (FilaFlex) você precisa de um método enumerador ou similar
            foreach (var a in course.Waitlist) // se FilaFlex implementar IEnumerable<Applicant>
            {
                sb.AppendLine($"{a.Name} {a.Result.AvarageGrade.ToString("F2").Replace('.', ',')}");
            }

            sb.AppendLine();
        }

        File.WriteAllText(fullOutputPath, sb.ToString(), Encoding.UTF8);
    }

}

public class FilaFlex<T> : IEnumerable<T>
{
    private Pia<T> _head = new Pia<T>();
    private Pia<T> Last { get; set; }
    private int Count { get; set; }

    public FilaFlex()
    {
        Last = _head;
    }

    public void Botar(T Tralha)
    {
        if (_head.Prox == null)
        {
            _head.Prox = new Pia<T>(Tralha);
            Last = _head.Prox;
            Count++;
            return;
        }

        Last.Prox = new Pia<T>(Tralha);
        Last = Last.Prox;
        Count++;
    }

    public T Tirar()
    {
        if (_head.Prox == null || _head.Prox.Tralha == null)
            throw new InvalidOperationException("Dequeue from an empty queue is not allowed.");

        T dropped = _head.Prox.Tralha;
        _head.Prox = _head.Prox.Prox;
        Count--;

        return dropped;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        Pia<T>? actual = _head.Prox;

        while (actual != null)
        {
            if (actual.Tralha != null)
                yield return actual.Tralha;

            actual = actual.Prox;
        }
    }
    public override string ToString()
    {
        if (_head.Prox == null) return "empty";

        StringBuilder sb = new StringBuilder();

        for (Pia<T>? i = _head.Prox; i != null; i = i.Prox)
        {
            sb.Append($"{i.Tralha} ");
        }
        return sb.ToString().Trim();
    }
}

public class Pia<T>
{
    public T? Tralha { get; set; }
    public Pia<T>? Prox { get; set; } = null;
    public Pia<T>? Ant { get; set; } = null;

    public Pia(T Content)
    {
        Tralha = Content;
    }

    public Pia() { }
}

public static class ListExtension
{
    public static void ApplicantsSort<T>(this List<T> list) where T:IComparable<T>
    {
        QuickSort(list);
    }

    static void QuickSort<T>(List<T> list) where T: IComparable<T>
    {
        QuickSort(0, list.Count - 1, list);
    }

    static void QuickSort<T>(int start, int end, List<T> list) where T: IComparable<T>
    {
        int pivot = start + (end-start) / 2;
        int i = start, j = end;

        while (i <= j)
        {
            while (list[i].CompareTo(list[pivot]) > 0) i++;
            while (list[j].CompareTo(list[pivot]) < 0) j--;

            if (i <= j)
            {
                T aux = list[i];
                list[i] = list[j];
                list[j] = aux;

                i++;
                j--;
            }
        }

        if (start < j) QuickSort(start, j, list);
        if (end > i) QuickSort(i, end, list);
    }
}