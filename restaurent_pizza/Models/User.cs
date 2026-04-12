namespace restaurent_pizza.Models;

// 🔵 C# pur — entité User (authentification maison)
public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;   // 🟡 BCrypt — jamais le mot de passe en clair !
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Client;              // 🔵 Par défaut = Client

    // 🔵 Factory Method
    public static User Create(string email, string passwordHash, string firstName, string lastName, Role role = Role.Client)
    {
        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Role = role
        };
    }
}
