using System;
using FomoCryptoNews.Cms.Payload.Cryptostate;
using FomoCryptoNews.Database.Cryptoslate;
using Status = FomoCryptoNews.Database.Cryptoslate.Status;

namespace FomoCryptoNews.Cms.Codec.CryptostateCodec;

public static class CryptostateNewsCodec
{
    public static CryptostateNews EncodeNews(CryptoslateNewsModel dbModel)
    {
        return new CryptostateNews
        {
            Id = dbModel.Id,
            Title = dbModel.Title,
            Description = dbModel.Description,
            Cover = dbModel.Cover,
            StatusPayload = _encodeStatus(dbModel.Status),
            CreatedAt = _toUnixTime(dbModel.CreatedAt)
        };
    }



    private static StatusPayload _encodeStatus(Status dbStatus)
    {
        switch (dbStatus)
        {
            case Status.Parsed:
                return StatusPayload.Parsed;
            case Status.Approved:
                return StatusPayload.Approved;
            case Status.Declined:
                return StatusPayload.Declined;
            case Status.Deleted:
                return StatusPayload.Deleted;
            default: throw new ArgumentException($"Encode status: {dbStatus} not encoded.");
        }
    }
    
    
    private static int _toUnixTime(DateTimeOffset date)
    {
        return checked((int) date.ToUnixTimeSeconds());
    }

}