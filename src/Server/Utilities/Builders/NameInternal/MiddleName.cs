namespace BirToolsApp.Server.Utilities.Builders.NameInternal;

internal class MiddleName : IMiddleName
{
    private readonly NameBuilder _builder;

    public MiddleName(NameBuilder builder)
    {
        _builder = builder;
    }

    public INameBuilder WithLastName(string lastName)
    {
        if (!string.IsNullOrWhiteSpace(lastName))
            _builder.Result = $"{_builder.Result} {lastName}";

        return _builder;
    }

    public override string ToString() => _builder.Result;
}