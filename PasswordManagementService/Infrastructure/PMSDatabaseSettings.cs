namespace PasswordManagementService.Infrastructure;

public class PMSDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string PasswordsCollectionName { get; set; } = null!;
}
