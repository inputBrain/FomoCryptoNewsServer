using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Database.Cryptoslate;

public class CryptoslateNewsRepository : AbstractRepository<CryptoslateNewsModel>, ICryptoslateNewsRepository
{
    public CryptoslateNewsRepository(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }


    public async Task<CryptoslateNewsModel> GetOne(int id)
    {
        var model = await DbModel.FirstOrDefaultAsync(x => x.Id == id);
        if (model == null)
        {
            throw new Exception("Cryptoslate News model is not found");
        }

        return model;
    }


    public async Task<bool> CreateBulk(ImmutableArray<CryptoslateNewsModel> models)
    {
        var result = await CreateBulkModelsAsync(models);
        if (result == null)
        {
            Logger.LogCritical("Cryptoslate news models IS NOT CREATED");
            return false;
        }

        return true;
    }


    public async Task<bool> UpdateBulk(ImmutableArray<CryptoslateNewsModel> models)
    {
        var result = await UpdateBulkModelsAsync(models);
        if (result == null)
        {
            Logger.LogCritical("Cryptoslate news models IS NOT UPDATED");
            return false;
        }

        return true;
    }


    public async Task UpdateStatus(CryptoslateNewsModel model, Status status)
    {
        model.Status = status;
        await UpdateModelAsync(model);
    }


    public async Task<ImmutableArray<CryptoslateNewsModel>> List(ImmutableArray<string> titles)
    {
        var collection = await DbModel.Where(x => titles.Contains(x.Title)).ToListAsync();

        return [..collection];
    }


    public async Task<List<CryptoslateNewsModel>> ListAllByStatus(Status status)
    {
        return await DbModel.Where(x => x.Status == status).ToListAsync();
    }


    public async Task<List<CryptoslateNewsModel>> ListAll(int pageIndex, int pageSize)
    {
        return await DbModel
            .Where(x => x.Status != Status.Approved)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}