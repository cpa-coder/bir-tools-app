namespace BirToolsApp.Server.Utilities.Builders.NameInternal;

internal class MiddleInitial : IMiddleInitial
{
    private readonly NameBuilder _builder;

    public MiddleInitial(NameBuilder builder)
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