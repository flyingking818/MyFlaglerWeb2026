using System;

public abstract class Person
{
    public string Name { get; set; }
    public string ID { get; set; }
    public string Email { get; set; }
    public byte[] ProfileImage { get; set; } //not implemented.

    //Default constructor
    protected Person() { }

    // Custom constructor
    protected Person(string name, string id, string email)
    {
        Name = name;
        ID = id;
        Email = email;
    }
    public abstract string GetDetails();
}