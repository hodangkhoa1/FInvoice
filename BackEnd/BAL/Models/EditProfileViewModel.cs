using DAL.Entities;

namespace BAL.Models
{
    public class EditProfileViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public char Gender { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
