using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using sa.Models;

namespace sa.Stores;

public class EfUserStore : IUserStore
{
    private readonly ApplicationDbContext _db;
    public EfUserStore(ApplicationDbContext db) => _db = db;

    public UserDto? FindByEmail(string email) =>
        _db.Users.FirstOrDefault(u => u.Email == email) is { } u ? Map(u) : null;

    public UserDto? FindById(int id) =>
        _db.Users.Find(id) is { } u ? Map(u) : null;

    public bool ExistsByEmail(string email) => _db.Users.Any(u => u.Email == email);

    public UserDto Create(string name, string email, string passwordHash, bool isAdmin)
    {
        var u = new User { Name = name, Email = email, PasswordHash = passwordHash, IsAdmin = isAdmin };
        _db.Users.Add(u);
        _db.SaveChanges();
        return Map(u);
    }

    public IReadOnlyList<UserDto> ListAll() =>
        _db.Users.OrderBy(u => u.Id).Select(u => Map(u)).ToList();

    public bool SetAdminStatus(int id, bool isAdmin)
    {
        var u = _db.Users.Find(id);
        if (u == null) return false;
        u.IsAdmin = isAdmin;
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var u = _db.Users.Find(id);
        if (u == null) return false;
        _db.Users.Remove(u);
        _db.SaveChanges();
        return true;
    }

    private static UserDto Map(User u) =>
        new(u.Id, u.Name, u.Email, u.PasswordHash, u.IsAdmin, u.CreatedAt);
}
