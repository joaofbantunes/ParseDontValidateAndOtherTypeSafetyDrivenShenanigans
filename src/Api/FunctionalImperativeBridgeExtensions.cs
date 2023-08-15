using System.Diagnostics.CodeAnalysis;
using LanguageExt;

namespace Api;

public static class FunctionalImperativeBridgeExtensions
{
    public static bool TryGetValue<T>(this Option<T> option, [MaybeNullWhen(false)] out T value)
    {
        value = option.Match(
            Some: static some => some,
            None: static () => default(T));
        
        return option.IsSome;
    }

    public static bool TryGetValue<TLeft, TRight>(
        this Either<TLeft, TRight> either,
        [MaybeNullWhen(true)] out TLeft left,
        [MaybeNullWhen(false)] out TRight right)
    {
        (left, right) = either.Match(
            Right: static r => (default(TLeft), r),
            Left: static l => (l, default(TRight)));

        return either.IsRight;
    }
}