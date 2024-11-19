namespace NATSInternal.Hubs;

public class WrapperRequestMessage
{
    public string Url { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public string Body { get; set; }
}
