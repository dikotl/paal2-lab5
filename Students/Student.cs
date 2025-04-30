namespace Lab5;

public struct Student
{
    public string FirstName;
    public string LastName;
    public string Patronymic;
    public Gender Gender;
    public DateOnly BirthDay;
    public byte MathGrade;
    public byte PhysicsGrade;
    public byte ITGrade;
    public ushort Scholarship; // Grant?

    public string WriteString()
    {
        return $"{LastName} {FirstName} {Patronymic} {GenderToStringUA(Gender)} " +
            $"{BirthDay} {WriteGrade(MathGrade)} {WriteGrade(PhysicsGrade)} " +
            $"{WriteGrade(ITGrade)} {Scholarship}";

        static string GenderToStringUA(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Ч",
                Gender.Female => "Ж",
                Gender.AttackHelicopter => "ВіуВіу",
                _ => "ІНШЕ",
            };
        }

        static string GenderToString(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "M",
                Gender.Female => "F",
                Gender.AttackHelicopter => "WiuWiu",
                _ => "OTHER",
            };
        }

        static string WriteGrade(byte grade)
        {
            if (grade == 0) return "-";
            return $"{grade}";
        }
    }

    public static Student ParseString(string s)
    {
        string[] input = s.Split().ToArray();

        if (input.Length != 9)
        {
            throw new FormatException(
                $"The input data contains invalid data count ({input.Length})");
        }

        return new Student
        {
            LastName = ParseName(input[0], "last name"),
            FirstName = ParseName(input[1], "first name"),
            Patronymic = ParseName(input[2], "patronymic"),
            Gender = ParseGender(input[3]),
            BirthDay = DateOnly.Parse(input[4]),
            MathGrade = ParseGrade(input[5]),
            PhysicsGrade = ParseGrade(input[6]),
            ITGrade = ParseGrade(input[7]),
            Scholarship = ParseScholarship(input[8]),
        };


        static string ParseName(string s, string what)
        {
            if (s.Any(c => !char.IsLetter(c) && c != '-' && c != '\''))
                throw new FormatException(
                    $"Invalid value for {what}, expected only letters, - and '");

            return s;
        }

        static Gender ParseGender(string s)
        {
            return s switch
            {
                "M" or "Ч" => Gender.Male,
                "F" or "Ж" => Gender.Female,
                "WiuWiu" or "ВіуВіу" => Gender.AttackHelicopter,
                _ => Gender.Other,
            };
        }

        static Gender ParseDateOnly(string s)
        {
            return s switch
            {
                "M" or "Ч" => Gender.Male,
                "F" or "Ж" => Gender.Female,
                "WiuWiu" or "ВіуВіу" => Gender.AttackHelicopter,
                _ => Gender.Other,
            };
        }

        static byte ParseGrade(string s)
        {
            if (s == "-")
                return 0;

            try
            {
                var value = byte.Parse(s);

                if (value < 2)
                    throw new FormatException(
                        $"Given grade {value} is too small (< 2)");

                if (value > 5)
                    throw new FormatException(
                        $"Given grade {value} is too big (> 5)");

                return value;
            }
            catch (OverflowException)
            {
                throw new FormatException("You can't be this smart");
            }
        }

        static ushort ParseScholarship(string s)
        {
            try
            {
                var value = ushort.Parse(s);

                if (value != 0)
                {
                    if (value < 1234)
                        throw new FormatException(
                            $"Given scholarship {value} is too small (< 1234)");
                    if (value > 4321)
                        throw new FormatException(
                            $"Given scholarship {value} is too big (> 4321)");
                }

                return value;
            }
            catch (OverflowException)
            {
                throw new FormatException("Given scholarship is not in valid range");
            }
        }
    }
}
