using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace FomoCryptoNews.Database.Cryptoslate;

public interface ICryptoslateNewsRepository
{
    Task<CryptoslateNewsModel> GetOne(int id);

    Task<bool> CreateBulk(ImmutableArray<CryptoslateNewsModel> models);

    Task<bool> UpdateBulk(ImmutableArray<CryptoslateNewsModel> models);

    Task UpdateStatus(CryptoslateNewsModel model, Status status);

    Task<ImmutableArray<CryptoslateNewsModel>> List(ImmutableArray<string> titles);

    Task<List<CryptoslateNewsModel>> ListAllByStatus(Status status);

    Task<List<CryptoslateNewsModel>> ListAll(int pageIndex, int pageSize);
}