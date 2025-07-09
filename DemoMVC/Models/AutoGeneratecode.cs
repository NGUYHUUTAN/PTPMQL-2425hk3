using DemoMVC.Data;

namespace DemoMVC.Models
{
    public class AutoGeneratecode
    {
        public static string Generate(ApplicationDbcontext context)
        {
            var lastPerson = context.Person
                .OrderByDescending(p => p.PersonId)
                .FirstOrDefault();

            if (lastPerson == null)
                return "St001";

            var lastNumber = int.Parse(lastPerson.PersonId.Substring(1));
            return "St" + (lastNumber + 1).ToString("D3"); // St002, St003...
        }
    }
}