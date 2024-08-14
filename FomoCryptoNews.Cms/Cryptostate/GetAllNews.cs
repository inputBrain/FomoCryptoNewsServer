using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FomoCryptoNews.Cms.Payload.Cryptostate;

namespace FomoCryptoNews.Cms.Cryptostate;

public sealed class GetAllNews
{
        
    [Required]
    public int Skip { get; set; }
    
    [Required]
    public int Take { get; set; }


    public class Response : AbstractResponse
    {
        [Required]
        public List<CryptostateNews> News { get; }
        
        public int TotalCount { get; }


        public Response(List<CryptostateNews> news, int totalCount)
        {
            News = news;
            TotalCount = totalCount;
        }
    } 
}