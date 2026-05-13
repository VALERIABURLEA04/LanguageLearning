using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using sa.Models;

namespace sa.Stores;

public class EfPurchaseStore : IPurchaseStore
{
    private readonly ApplicationDbContext _db;
    public EfPurchaseStore(ApplicationDbContext db) => _db = db;

    public PurchaseDto? FindById(int id) =>
        _db.Purchases.Find(id) is { } p ? Map(p) : null;

    public IReadOnlyList<PurchaseDto> ListAll() =>
        _db.Purchases.OrderByDescending(p => p.PurchaseDate).Select(p => Map(p)).ToList();

    public IReadOnlyList<PurchaseDto> ListByStudent(string studentName) =>
        _db.Purchases
            .Where(p => p.StudentName == studentName)
            .OrderByDescending(p => p.PurchaseDate)
            .Select(p => Map(p))
            .ToList();

    public PurchaseDto Create(PurchaseDto purchase)
    {
        var row = new Purchase
        {
            StudentName   = purchase.StudentName,
            CourseTitle   = purchase.CourseTitle,
            Amount        = purchase.Amount,
            PurchaseDate  = purchase.PurchaseDate == default ? DateTime.UtcNow : purchase.PurchaseDate,
            Status        = purchase.Status,
            PaymentMethod = purchase.PaymentMethod
        };
        _db.Purchases.Add(row);
        _db.SaveChanges();
        return Map(row);
    }

    public bool Update(PurchaseDto purchase)
    {
        var row = _db.Purchases.Find(purchase.Id);
        if (row == null) return false;
        row.StudentName   = purchase.StudentName;
        row.CourseTitle   = purchase.CourseTitle;
        row.Amount        = purchase.Amount;
        row.PurchaseDate  = purchase.PurchaseDate;
        row.Status        = purchase.Status;
        row.PaymentMethod = purchase.PaymentMethod;
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var row = _db.Purchases.Find(id);
        if (row == null) return false;
        _db.Purchases.Remove(row);
        _db.SaveChanges();
        return true;
    }

    private static PurchaseDto Map(Purchase p) =>
        new(p.Id, p.StudentName, p.CourseTitle, p.Amount, p.PurchaseDate, p.Status, p.PaymentMethod);
}
