using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IInMemoryRepo
    {
        public IEnumerable<Person> GetAll();
        public Person GetOne(int id);
        public Person NewName(int id, string newName);
    }
}
