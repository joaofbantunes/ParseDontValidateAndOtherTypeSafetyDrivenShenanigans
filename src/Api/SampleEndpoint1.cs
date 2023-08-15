using LanguageExt;

namespace Api;

public static class SampleEndpoint1
{
    // ReSharper disable once ClassNeverInstantiated.Local - used to deserialize HTTP request body
    private sealed record Request(string SomeId, Guid SomeOtherId);

    public static IEndpointRouteBuilder MapSampleEndpoint1(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/sample-endpoint-1", Handle);
        return endpoints;
    }

    private static IResult Handle(
        Request request,
        SomeThingRepository someThingRepository,
        SomeOtherThingRepository someOtherThingRepository)
    {
        if (!Parse(request).TryGetValue(out var validationError, out var parsedInputs))
        {
            return Results.BadRequest(validationError);
        }

        var maybeSomeThing = someThingRepository.Load(parsedInputs.someId);

        if (!maybeSomeThing.TryGetValue(out var someThing))
        {
            return Results.NotFound();
        }

        var maybeSomeOtherThing = someOtherThingRepository.Load(parsedInputs.someOtherId);

        if (!maybeSomeOtherThing.TryGetValue(out var someOtherThing))
        {
            return Results.NotFound();
        }

        var result = someThing.DoSomethingRequiringOtherThing(someOtherThing);

        return result.Match(
            Right: _ =>
            {
                someThingRepository.Save(someThing);
                return Results.NoContent();
            },
            Left: reason => Results.Conflict(new DomainError(reason)));
    }

    private static Either<ValidationError, (SomeId someId, SomeOtherId someOtherId)> Parse(Request request)
        => (
                SomeId.Parse(request.SomeId).ToValidation("someId"),
                SomeOtherId.Parse(request.SomeOtherId).ToValidation("someOtherId")
            )
            .Apply(static (someId, someOtherId) => (someId, someOtherId))
            .ToEither()
            .MapLeft(errors => new ValidationError(errors));
}