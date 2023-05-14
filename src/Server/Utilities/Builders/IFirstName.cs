namespace BirToolsApp.Server.Utilities.Builders;

public interface IFirstName
{
    IMiddleName WithMiddleName(string middleName);
    IMiddleInitial WithMiddleInitial(char initial);
    INameBuilder WithLastName(string lastName);
}