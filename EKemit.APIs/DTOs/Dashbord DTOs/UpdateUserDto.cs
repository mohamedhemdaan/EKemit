namespace EKemit.APIs.DTOs.Dashbord_DTOs
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
}
