using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Database.TestNews;

public class TestNewsRepository : AbstractRepository<TestNewsModel>, ITestNewsRepository
{

    public TestNewsRepository(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
    
    
    public async Task<TestNewsModel> CreateModel(string title, string description, string? cover)
    {
        var model = TestNewsModel.CreateModel(title, description, cover);

        var result = await CreateModelAsync(model);
        if (result == null)
        {
            throw new Exception("News model is not created");
        }

        return result;
    }
    
    
    public async Task<TestNewsModel> GetOne(int id)
    {
        var model = await DbModel.FirstOrDefaultAsync(x => x.Id == id);
        if (model == null)
        {
            throw new Exception("News model is not found");
        }

        return model;
    }
}