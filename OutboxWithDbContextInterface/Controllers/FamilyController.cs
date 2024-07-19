using System.Transactions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OutboxWithDbContextInterface.Data;
using OutboxWithDbContextInterface.Models;
using OutboxWithDbContextInterface.Repositories;

namespace OutboxWithDbContextInterface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyController(IFamilyRepo familyRepo, ISendEndpointProvider sendEndpointProvider) : ControllerBase
    {

        [HttpPost(Name = "AddChildren")]
        public async Task<ActionResult<string>> Post([FromBody] List<string> childrenNames)
        {
            var childrenIds = new List<string>();
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:myQueue"));

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                       TransactionScopeAsyncFlowOption.Enabled))
            {
                var parent = await familyRepo.CreateParent();
                foreach (var name in childrenNames)
                {
                    var child = await familyRepo.CreateChild(parent, name);
                    childrenIds.Add(child.ExternalId);
                    await endpoint.Send(new NewChildMessage { Id = child.ExternalId, Name = name });
                }

                await familyRepo.UpdateParent(parent, childrenIds);
                transactionScope.Complete();
                return Ok(parent.ExternalId);

            }

        }
    }
}