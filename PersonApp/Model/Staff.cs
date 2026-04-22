using System;

public class Staff : Person
{
    public string Position { get; set; }
    public string Division { get; set; }
    public bool IsAdministrative { get; set; }

    //Default constructor
    public Staff() { }

    // Custom constructor
    public Staff(string name, string id, string email, string position, string division, bool isAdministrative)
        : base(name, id, email) // Call to the base class constructor
    {
        Position = position;
        Division = division;
        IsAdministrative = isAdministrative;
    }


    public override string GetDetails()
    {
        return $"{Name}: {Position} | {Division} | Administrative: {IsAdministrative}";
    }
}