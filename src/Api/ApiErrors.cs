namespace Api;

public sealed record ValidationError(IEnumerable<string> InvalidInputs);

// a domain error shouldn't be generic like this, but well typed, but given this is just a sample, it's fine
public sealed record DomainError(string Reason);