using OutboxWithDbContextInterface.Models;

namespace OutboxWithDbContextInterface.Repositories;

public interface IFamilyRepo
{
    Task<Parent> CreateParent();
    Task<Parent> UpdateParent(Parent parent, List<string> childrenIds);
    Task<Child> CreateChild(Parent parent, string name);
}