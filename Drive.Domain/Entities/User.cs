namespace Drive.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public User() { }

    public User(string name, string email, string passwordHash, bool isActive)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = isActive;
    }
}
