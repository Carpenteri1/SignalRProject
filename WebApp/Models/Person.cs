using System.ComponentModel.DataAnnotations;

namespace WebClientApp.Models
{
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public string adress { get; set; }
        public string email { get; set; }

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
            id = id;
            name = name;
            lastName = lastname;
            age = age;
            adress = adress;
            email = email;
        }
        public Person(string name)
        {
            id = id;
            name = name;
            lastName = lastName;
            age = age;
            adress = adress;
            email = email;
        }
    }
}
