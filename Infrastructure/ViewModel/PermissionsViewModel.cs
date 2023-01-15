namespace Infrastructure.ViewModel;

public class PermissionsViewModel
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public List<CheckBoxViewModel> RoleClaims { get; set; } = new List<CheckBoxViewModel>();
}