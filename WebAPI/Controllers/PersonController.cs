using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    public class PersonController : Controller
    {
        private IInMemoryRepo? inMemory;
        private IHubContext<PersonHub>? actionHub;

        public PersonController(
            IInMemoryRepo inMemory,
            IHubContext<PersonHub> actionHub)
        {
            this.inMemory = inMemory;
            this.actionHub = actionHub;
        }

        [Route("/Person/{id:int}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            if (inMemory == null ||  
                actionHub == null)
                return NotFound();

            var thePerson = inMemory.GetOne(id);
            await actionHub.Clients.All.SendAsync("PersonFound", thePerson);
            return Ok(thePerson);
        }


        [Route("/People")]
        public async Task<IActionResult> GetPeople(int id)
        {
            if (inMemory == null ||
                actionHub == null)
                return NotFound();

            var people = inMemory.GetAll();
            await actionHub.Clients.All.SendAsync("PersonFound", people);
            return Ok(people);
        }
    }
}
