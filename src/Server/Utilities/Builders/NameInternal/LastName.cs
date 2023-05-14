namespace BirToolsApp.Server.Utilities.Builders.NameInternal;

internal class LastName : ILastName
{
    private readonly NameBuilder _builder;

    public LastName(NameBuilder builder)
    {
        _builder = builder;
    }

    public IFirstName WithFirstName(string firstName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            _builder.Result = $"{_builder.Result}, {firstName}";

        return new FirstName(_builder);
    }

    public override string ToString() => _builder.Result;
}