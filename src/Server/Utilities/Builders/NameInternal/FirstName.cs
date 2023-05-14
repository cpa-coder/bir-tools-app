namespace BirToolsApp.Server.Utilities.Builders.NameInternal;

internal class FirstName : IFirstName
{
    private readonly NameBuilder _builder;

    public FirstName(NameBuilder builder)
    {
        _builder = builder;
    }

    public IMiddleName WithMiddleName(string middleName)
    {
        if (!string.IsNullOrWhiteSpace(middleName))
            _builder.Result = $"{_builder.Result} {middleName}";

        return new MiddleName(_builder);
    }

    public IMiddleInitial WithMiddleInitial(char initial)
    {
        if (!char.IsLetter(initial))
            throw new ArgumentException("Initial must be a letter.");

        _builder.Result = $"{_builder.Result} {initial}.";

        return new MiddleInitial(_builder);
    }

    public INameBuilder WithLastName(string lastName)
    {
        _builder.Result = $"{_builder.Result} {lastName}";
        return _builder;
    }

    public override string ToString() => _builder.Result;
}