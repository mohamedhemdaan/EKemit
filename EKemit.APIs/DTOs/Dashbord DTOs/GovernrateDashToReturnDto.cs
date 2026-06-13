namespace EKemit.APIs.DTOs.Dashbord_DTOs
{
    public class GovernrateDashToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
