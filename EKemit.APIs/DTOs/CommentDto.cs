namespace EKemit.APIs.DTOs
{
    public class CommentDto
    {
        public string Text { get; set; }

        public string? BuyerEmail { get; set; }

        public int ProductId { get; set; }
    }
}
