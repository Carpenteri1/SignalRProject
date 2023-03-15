namespace WebAPI.Models
{
    public class Person
    {
        public int Id { get; }
        public string? Name { get;}
        public string? LastName { get;}
        public int? Age { get; }
        public string? Adress { get;}
        public string? Email { get; }

        public Person()
        {

        }

        public Person(
            int id,
            string name, 
            string lastname, 
            int age, 
            string adress, 
            string email)
        {
            Id = id;
            Name = name;
            LastName = lastname;
            Age = age;
            Adress = adress;
            Email = email;
        }
        public Person(string name)
        {
            Id = Id;
            Name = name;
            LastName = LastName;
            Age = Age;
            Adress = Adress;
            Email = Email;
        }
    }
}
