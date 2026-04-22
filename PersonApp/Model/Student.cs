using System;

public class Student : Person
{
    public string Major { get; set; }
    public double GPA { get; set; }
    public bool IsFullTime { get; set; }
    public DateTime EnrollmentDate { get; set; }

    // Default constructor
    public Student() { }

    // Custom constructor
    public Student(string name, string id, string email, string major, double gpa, bool isFullTime, DateTime enrollmentDate)
        : base(name, id, email) // Call the base constructor from Person
    {
        Major = major;
        GPA = gpa;
        IsFullTime = isFullTime;
        EnrollmentDate = enrollmentDate;
    }

    public override string GetDetails()
    {
        return $"{Name}: {Major} Major | GPA: {GPA} | Enrollment Date: {EnrollmentDate.ToString()} | Full-Time: {IsFullTime}";
    }
}