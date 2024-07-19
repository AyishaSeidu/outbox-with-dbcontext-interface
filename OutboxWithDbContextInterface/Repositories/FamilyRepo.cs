using OutboxWithDbContextInterface.Data;
using OutboxWithDbContextInterface.Models;

namespace OutboxWithDbContextInterface.Repositories;

public class FamilyRepo(IMyDbContext context) : IFamilyRepo
{
    public async Task<Parent> CreateParent()
    {
        var parent = new Parent { ExternalId = Guid.NewGuid().ToString() };
        context.Add(parent);
        await context.SaveChangesAsync();
        return parent;
    }

    public async Task<Parent> UpdateParent(Parent parent, List<string> childrenIds)
    {
        parent.ChildrenIds = childrenIds;
        context.Update(parent);
        await context.SaveChangesAsync();
        return parent;
    }

    public async Task<Child> CreateChild(Parent parent, string name)
    {
        var child = new Child
            { ExternalId = Guid.NewGuid().ToString(), ParentId = parent.Id, Name = name };
        context.Add(child);
        await context.SaveChangesAsync();
        return child;
    }
}