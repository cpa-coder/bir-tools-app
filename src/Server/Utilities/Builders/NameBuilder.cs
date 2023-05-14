using BirToolsApp.Server.Utilities.Builders.NameInternal;

namespace BirToolsApp.Server.Utilities.Builders;

public interface INameBuilder
{
    IFirstName FirstName(string firstName);

    ILastName LastName(string lastName);

    INameBuilder WithSuffix(string suffix);

    INameBuilder WithSalutation(string salutation);

    INameBuilder WithTitle(string title);

    string ToString();
}

public sealed class NameBuilder : INameBuilder
{
    public IFirstName FirstName(string firstName)
    {
        Result = firstName;
        return new FirstName(this);
    }

    public ILastName LastName(string lastName)
    {
        Result = lastName;
        return new LastName(this);
    }

    public INameBuilder WithSuffix(string suffix)
    {
        Result = $"{Result} {suffix}";
        return this;
    }

    public INameBuilder WithSalutation(string salutation)
    {
        Result = $"{salutation} {Result}";
        return this;
    }

    public INameBuilder WithTitle(string title)
    {
        Result = $"{Result}, {title}";
        return this;
    }

    internal string Result { get; set; }

    public override string ToString() => Result;
}