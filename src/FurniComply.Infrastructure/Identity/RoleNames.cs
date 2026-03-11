namespace FurniComply.Infrastructure.Identity;

public static class RoleNames
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string ComplianceManager = "ComplianceManager";
    public const string DepartmentHead = "DepartmentHead";
    public const string Auditor = "Auditor";
    public const string Procurement = "Procurement";

    public static readonly string[] All =
    {
        SuperAdmin,
        Admin,
        ComplianceManager,
        DepartmentHead,
        Auditor,
        Procurement
    };
}
