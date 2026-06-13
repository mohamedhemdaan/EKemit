namespace EKemit.APIs.DTOs.Dashbord_DTOs
{
    public class UserDashboardDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }  
    }
}
