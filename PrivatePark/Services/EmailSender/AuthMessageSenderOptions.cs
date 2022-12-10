namespace mnacr22.Services;

public class AuthMessageSenderOptions
{
    public string? SendGridKey { get; set; }
    
    public string? StripeKey { get; set; }
    
    public string? GoogleKey { get; set; }
}