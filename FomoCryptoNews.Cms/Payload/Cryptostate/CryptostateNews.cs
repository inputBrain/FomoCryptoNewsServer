namespace FomoCryptoNews.Cms.Payload.Cryptostate;

public class CryptostateNews
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Cover { get; set; }

    public StatusPayload StatusPayload { get; set; }

    public int CreatedAt { get; set; }

    public int UpdatedAt { get; set; }
}