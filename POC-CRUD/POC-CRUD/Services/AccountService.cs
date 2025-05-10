using POC_CRUD.DTOs;
using POC_CRUD.Exceptions;
using POC_CRUD.Models;
using POC_CRUD.Repositories;

namespace POC_CRUD.Services;

public class AccountService : IService
{
    private readonly AccountRepository _accountRepository;
    private readonly UserRepository _userRepository;
    private readonly EmailService _emailService;
    private readonly ILogger<ExceptionHandler> _logger;

    public AccountService(AccountRepository accountRepository, UserRepository userRepository, EmailService emailService,  ILogger<ExceptionHandler> logger)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
        
    }
    
    public Account AddAccount(Account account)
    {
        ValidateInputForAdd(account);

        account.Id = Guid.NewGuid();
        account.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        _accountRepository.Add(account);
        
        SendWelcomeEmail(account);

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
        accountToUpdate.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        _accountRepository.Add(accountToUpdate);

        return accountToUpdate;
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

    public Account GetById(Guid accountId)
    {
        var account = _accountRepository.GetById(accountId);

        if (account == null)
        {
            throw new NotFoundException("Account not found: " + accountId);
        }

        return account;
    }
    
    public Account GetByUserId(Guid userId)
    {
        var account = _accountRepository.GetByUserId(userId);

        if (account == null)
        {
            throw new NotFoundException("Account not found for userId: " + userId);
        }

        return account;
    }

    public IEnumerable<Account> Query(AccountQueryDto filter)
    {
        return _accountRepository.Query(filter);
    }
    
    private static void ValidateCommonInput(Account account)
    {

        if (account.Name == null && account.BusinessName == null)
        {
            throw new ValidationException("name or businessName is required");
        }

        if (account.Name != null && account.Cpf == null)
        {
            throw new ValidationException("cpf is required for name");
        }
        
        if (account.BusinessName != null && account.Cnpj == null)
        {
            throw new ValidationException("cnpj is required for businessName");
        }
        
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

    private Account ValidateInputForUpdate(Guid accountId, Account account)
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

    private async void SendWelcomeEmail(Account account)
    {
        try
        {
            var user = _userRepository.GetById(account.UserId);

            var userEmail = user.Email;

            var payload = new EmailPayload()
            {
                To = userEmail,
                Subject = "Seja bem-vindo(a) ao POC-CRUD, " + account.Name,
                Body = "Seja bem-vindo(a) ao POC-CRUD! Esse email garante que sua conta foi criada com sucesso."
            };

            await _emailService.SendEmail(payload);
        }
        catch (Exception exception)
        {
            _logger.Log(LogLevel.Error, exception, "Error sending email: " + exception.Message);
        }
    }

}