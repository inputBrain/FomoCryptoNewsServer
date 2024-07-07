namespace FomoCryptoNews.UseCase;

public interface IUseCaseContainer
{
    ICryptoslateUseCase CryptoslateUseCase { get; }
}