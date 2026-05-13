using System.Collections.Generic;
using MyWebApplication.BusinessLogic.Core.Dtos;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface IPurchaseStore
    {
        PurchaseDto? FindById(int id);
        IReadOnlyList<PurchaseDto> ListAll();
        IReadOnlyList<PurchaseDto> ListByStudent(string studentName);
        PurchaseDto Create(PurchaseDto purchase);
        bool Update(PurchaseDto purchase);
        bool Delete(int id);
    }
}
