namespace mnacr22.Models;

public class Chat
{
    public int Id { get; set; }
    
    public string User1Id { get; set; }
    
    public string User2Id { get; set; }
    
    public ICollection<Message>? Messages { get; set; }
}

public class Message
{
    public int Id { get; set; }
    
    public int ChatId { get; set; }
    
    public int SenderId { get; set; }
    
    public string Text { get; set; } = String.Empty;
    
    public DateTime Timestamp { get; set; }
}