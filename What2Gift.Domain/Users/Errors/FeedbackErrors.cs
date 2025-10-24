using What2Gift.Domain.Common;

namespace What2Gift.Domain.Users.Errors;

public static class FeedbackErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Feedback.NotFound",
        $"The feedback with the identifier {id} was not found.");
}
