using System;

namespace WebApplicationExample
{
    public static partial class FunctionalExtensions
    {
        public static TActionResult ToActionResult<T, TActionResult>(this T @this, Func<object,
            TActionResult> conversion) => conversion(@this);


    }

    //private static IActionResult Match<L, R>(this Either<L, R> either) =>
    // either.Match<IActionResult>(
    //     Left: l => new BadRequestObjectResult(l),
    //     Right: r => new OkObjectResult(r));


}