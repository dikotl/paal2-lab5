namespace Lab5;

public class Block2
{
    public static void Main()
    {
        var students = ReadAndParseStudents("input.txt");

        Task7(students);
    }

    private static Student[] ReadAndParseStudents(string path)
    {
        var lines = new StreamReader(path)
            .ReadToEnd()
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.Length != 0)
            .ToList();

        var students = new List<Student>(lines.Count);

        foreach (string line in lines)
        {
            try
            {
                students.Add(Student.ParseString(line));
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error! {ex.Message}");
                continue;
            }
        }

        return students.ToArray();
    }

    // For students who have at least one unsatisfactory grade or absence,
    // replace (in the 'data_new.txt' file) the scholarship amount with 0.
    // Do not make any other changes to the data (except spaces).
    private static void Task7(Student[] students)
    {
        using StreamWriter output = new("data_new.txt");

        for (int i = 0; i < students.Length; i++)
        {
            var student = students[i];

            if (HasAbsenceOrBadGrade(student))
            {
                student.Scholarship = 0;
            }

            output.WriteLine(student.WriteString());
        }

        output.Flush();


        static bool HasAbsenceOrBadGrade(Student student)
        {
            return AbsenceOrBadGrade(student.MathGrade)
                || AbsenceOrBadGrade(student.PhysicsGrade)
                || AbsenceOrBadGrade(student.ITGrade);


            static bool AbsenceOrBadGrade(byte value)
            {
                return value == 0 || value == 2;
            }
        }
    }
}
