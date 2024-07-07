using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FomoCryptoNews.Database.Cryptoslate;
using FomoCryptoNews.Parsers.Cryptoslate;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.UseCase;

public class CryptoslateUseCase : ICryptoslateUseCase
{
    private readonly ILogger<CryptoslateUseCase> _logger;

    private readonly ICryptoslateParser _parser;


    public CryptoslateUseCase(ILogger<CryptoslateUseCase> logger, ICryptoslateParser parser)
    {
        _logger = logger;
        _parser = parser;
    }



    public async Task<bool> ParseNews(ICryptoslateNewsRepository newsRepository, CancellationToken cancellationToken)
    {
        var parsePagesCount = 20;
        var collection = await _parser.ParseNewsByAmountOfPages(parsePagesCount, cancellationToken);

        _logger.LogInformation("\t --- Cryptoslate news count: {Count}", collection.Count);

        var titleKeys = collection.Select(_ => _.Title).ToImmutableArray();

        var existingModels = await newsRepository.List(titleKeys);

        var preparedNewModels = new List<CryptoslateNewsModel>();
        // var preparedUpdateModels = new List<CryptoslateNewsModel>();

        foreach (var data in collection)
        {
            var searchedSymbol = existingModels.FirstOrDefault(_ => _.Title == data.Title);
            if (searchedSymbol == null)
            {
                var newModel = CryptoslateNewsModel.CreateModel(data.Title, data.Description, data.Cover, Status.Parsed, DateTimeOffset.UtcNow);
                preparedNewModels.Add(newModel);

                _logger.LogInformation("Cryptoslate found a new news: {News}", data.Title);

            }

            // if (CryptoslateNewsModel.IsSameNews(searchedSymbol, data) == false)
            // {
            //     searchedSymbol.Update(data);
            //     preparedUpdateModels.Add(searchedSymbol);
            // }
            // else
            // {
            //     _logger.LogDebug("Cryptoslate news {News} dont have any changes. Skipped", searchedSymbol.Title);
            // }

        }

        if (preparedNewModels.Any())
        {
            await newsRepository.CreateBulk([..preparedNewModels]);
            _logger.LogInformation("\n\t Cryptoslate All news has been created");

        }

        // if (preparedUpdateModels.Any())
        // {
        //     await newsRepository.UpdateBulk(preparedUpdateModels.ToImmutableArray());
        //     _logger.LogInformation("\n\t Cryptoslate All news were updated");
        // }

        return true;
    }
}