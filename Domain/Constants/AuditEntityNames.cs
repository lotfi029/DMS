namespace Domain.Constants;

public static class AuditEntityNames
{
    // ── HRMS Entities ─────────────────────────────────────────────────────────
    public const string User = nameof(ApplicationUser);
    public const string Role = nameof(ApplicationRole);
    public const string RoleClaim = nameof(ApplicationRoleClaim);
    public const string Department = nameof(Department);

    // ── MOMS Entities ─────────────────────────────────────────────────────────
    public const string Project = "Project";
    public const string Stage = "Stage";
    public const string ChecklistItem = "ChecklistItem";
    public const string Document = "Document";
    public const string ClientApproval = "ClientApproval";
    public const string Vendor = "Vendor";
    public const string BOQItem = "BOQItem";
    public const string Meeting = "Meeting";
    public const string MeetingSummary = "MeetingSummary";
    public const string TimeTableEntry = "TimeTableEntry";
}
