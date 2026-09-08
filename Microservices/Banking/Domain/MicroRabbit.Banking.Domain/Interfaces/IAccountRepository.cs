using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Domain;

public interface IAccountRepository
{
    IEnumerable<Account> GetAccounts();
}
