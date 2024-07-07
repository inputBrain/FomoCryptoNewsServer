namespace FomoCryptoNews.UseCase;

public class UseCaseContainer : IUseCaseContainer
{
    public ICryptoslateUseCase CryptoslateUseCase { get; set; }
    
    public UseCaseContainer(ICryptoslateUseCase cryptoslateUseCase)
    {
        CryptoslateUseCase = cryptoslateUseCase;
    }
}