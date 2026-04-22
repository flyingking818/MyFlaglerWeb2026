using System;

public class Professor : Person
{
    public string Department { get; set; }
    public string ResearchArea { get; set; }
    public bool IsTerminalDegree { get; set; }

    //Default constructor
    public Professor() { }

    // Custom constructor
    public Professor(string name, string id, string email, string department, string researchArea, bool isTerminalDegree)
        : base(name, id, email) // Calls constructor from abstract Person class
    {
        Department = department;
        ResearchArea = researchArea;
        IsTerminalDegree = isTerminalDegree;
    }

    public override string GetDetails()
    {
        return $"{Name}: {Department} Department | Research: {ResearchArea} | Terminal Degree: {IsTerminalDegree}";
    }
}