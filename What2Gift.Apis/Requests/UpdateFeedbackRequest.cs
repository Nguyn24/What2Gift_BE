namespace What2Gift.Apis.Requests;

public class UpdateFeedbackRequest
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
