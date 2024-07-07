using System.Collections.Immutable;
using System.Threading.Tasks;

namespace FomoCryptoNews.Database.Cryptoslate;

public interface ICryptoslateNewsRepository
{
    Task<CryptoslateNewsModel> GetOne(int id);

    Task<bool> CreateBulk(ImmutableArray<CryptoslateNewsModel> models);

    Task<bool> UpdateBulk(ImmutableArray<CryptoslateNewsModel> models);

    Task<ImmutableArray<CryptoslateNewsModel>> List(ImmutableArray<string> titles);
}