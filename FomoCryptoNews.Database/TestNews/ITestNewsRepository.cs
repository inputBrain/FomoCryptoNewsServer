using System.Threading.Tasks;

namespace FomoCryptoNews.Database.TestNews;

public interface ITestNewsRepository
{
    public Task<TestNewsModel> CreateModel(string title, string description, string? cover);
    public Task<TestNewsModel> GetOne(int id);
}