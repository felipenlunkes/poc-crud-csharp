using POC_CRUD.DTOs;

namespace POC_CRUD.Services;

public interface IEmailService : IService
{
    Task SendEmail(EmailPayload payload);
}