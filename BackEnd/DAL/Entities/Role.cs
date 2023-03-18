namespace DAL.Entities
{
    public class Role
    {
        public string IdRole { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<Account> Accounts { get; set; }
    }
}
