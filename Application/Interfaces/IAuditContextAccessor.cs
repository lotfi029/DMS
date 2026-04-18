using Application.DTOs.Audits;

namespace Application.Interfaces;

public interface IAuditContextAccessor
{
    AuditContext GetCurrent();
}