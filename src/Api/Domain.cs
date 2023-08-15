using LanguageExt;

namespace Api;

public interface IParsable<TIn, TOut>
{
    static abstract Option<TOut> Parse(TIn value);
}

public readonly record struct SomeId : IParsable<string, SomeId>
{
    private SomeId(string value) => Value = value;
    public string Value { get; }

    public static Option<SomeId> Parse(string value)
        => !string.IsNullOrWhiteSpace(value)
            ? new SomeId(value)
            : Option<SomeId>.None;
}

public readonly record struct SomeOtherId : IParsable<Guid, SomeOtherId>, IParsable<string, SomeOtherId>
{
    private SomeOtherId(Guid value) => Value = value;
    public Guid Value { get; }

    public static Option<SomeOtherId> Parse(Guid value)
        => value != default
            ? new SomeOtherId(value)
            : Option<SomeOtherId>.None;

    public static Option<SomeOtherId> Parse(string value)
        => Guid.TryParse(value, out var parsed)
            ? Parse(parsed)
            : Option<SomeOtherId>.None;
}

public class SomeThing
{
    public SomeThing(SomeId id)
    {
        Id = id;
    }

    public SomeId Id { get; }

    public Either<string, Unit> DoSomethingRequiringOtherThing(SomeOtherThing otherThing)
        => throw new NotImplementedException();
}

public class SomeOtherThing
{
    public SomeOtherThing(SomeOtherId id)
    {
        Id = id;
    }

    public SomeOtherId Id { get; }
}