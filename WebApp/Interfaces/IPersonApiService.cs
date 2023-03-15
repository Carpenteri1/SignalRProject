using WebClientApp.Models;

namespace WebClientApp.ApiServices
{
    public interface IPersonApiService
    {
        Task<Person?> Add(Person product);
        Task<Person[]> GetAll();
        Task<Person> GetOne(int id);
        void GetMessage(string message);
    }
}
