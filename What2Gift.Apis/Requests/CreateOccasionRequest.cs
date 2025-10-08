namespace What2Gift.Apis.Requests;

public class CreateOccasionRequest
{
    public string Name { get; set; } = string.Empty;
    public int StartMonth { get; set; }
    public int StartDay { get; set; }
    public int EndMonth { get; set; }
    public int EndDay { get; set; }
}
