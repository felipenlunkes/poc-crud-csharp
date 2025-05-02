using POC_CRUD.Exceptions;
using POC_CRUD.Models;
using POC_CRUD.Repositories;

namespace POC_CRUD.Services;

public class AccountService : IService
{
    private readonly AccountRepository _accountRepository;

    public AccountService(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Account GetById(Guid accountId)
    {
        var account = _accountRepository.GetById(accountId);

        if (account == null)
        {
            throw new NotFoundException("Account not found: " + accountId);
        }

        return account;
    }

    public Account UpdateAccount(Guid accountId, Account account)
    {
        
        var accountToUpdate = ValidateInputForUpdate(accountId, account);

        if (accountToUpdate == null)
        {
            throw new NotFoundException("Account not found: " + accountId);
        }

        accountToUpdate.Name = account.Name;
        accountToUpdate.BusinessName = account.BusinessName;
        accountToUpdate.AllowsAdvertising = account.AllowsAdvertising;
        accountToUpdate.UpdatedAt = DateTime.UtcNow;

        _accountRepository.Add(accountToUpdate);

        return accountToUpdate;
    }

    public Account AddAccount(Account account)
    {
        ValidateInputForAdd(account);

        account.Id = Guid.NewGuid();
        account.UpdatedAt = DateTime.UtcNow;

        _accountRepository.Add(account);

        return account;
    }

    public void RemoveAccount(Guid accountId)
    {
        var account = _accountRepository.GetById(accountId);

        if (account == null)
        {
            throw new NotFoundException("Account not found");
        }

        _accountRepository.RemoveById(accountId);
    }

    private void ValidateCommonInput(Account account)
    {
        if (account.Cpf == null && account.Cnpj == null)
        {
            throw new ValidationException("cpf or cnpj is required");
        }
        
        if (account.Cpf != null && account.Cnpj != null)
        {
            throw new ValidationException("invalid use of cpf and cnpj");
        }
    }
    private void ValidateInputForAdd(Account account)
    {
        
        ValidateCommonInput(account);
        
        var accountFoundForCpf = _accountRepository.GetByCpf(account.Cpf);
        var accountForCnpj = _accountRepository.GetByCnpj(account.Cnpj);

        if (accountFoundForCpf != null || accountForCnpj != null)
        {
            throw new ValidationException("cpf or cnpj already used");
        }
    }

    private Account? ValidateInputForUpdate(Guid accountId, Account account)
    {
        ValidateCommonInput(account);

        var accountFoundForCpf = _accountRepository.GetByCpf(account.Cpf);
        if (accountFoundForCpf != null && accountFoundForCpf.Id != accountId)
        {
            throw new ValidationException("cpf already used for another account");
        }

        var accountForCnpj = _accountRepository.GetByCnpj(account.Cnpj);
        if (accountForCnpj != null && accountForCnpj.Id != accountId)
        {
            throw new ValidationException("cnpj already used for another account");
        }
        
        if (accountForCnpj != null) return accountForCnpj;
        if (accountFoundForCpf != null) return accountFoundForCpf;
        
        return null;
    }

}