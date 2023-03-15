using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repo
{
    public class InMemoryRepo : IInMemoryRepo
    {
        private readonly List<Person> people = new List<Person>();
        
        public InMemoryRepo() => people.AddRange
            (
                new Person[]
                {
                new Person(id: 1, name: "Johan", lastname: "Johansson", age: 22, adress: "Valgatan 2 Göteborg", email: "johan.jonhansson@test.com"),
                new Person(id: 2, name: "Gustav", lastname: "Eriksson", age: 31, adress: "Husvägen 4 Göteborg", email: "gustav.eriksson@test.com"),
                new Person(id: 3, name: "Tony", lastname: "Hansson", age: 74, adress: "Brunnsparken Göteborg", email: "tony.hansson@test.com")
                }
            );

        public IEnumerable<Person> GetAll() => people;
        public Person GetOne(int id) => people.First(p => p.Id == id) ?? new Person(id: 1, name: "DidntWork", lastname: "Fail", age: 404, adress: "DidntWork", email: "Fail@mail.com");
        public Person NewName(int id, string newName)
        {
            var person = people.FirstOrDefault(a => a.Id == id);
            person = new Person(name: newName);
            return person;
        }
    }
}
