namespace SUMI.Models
{
    public class Comment
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }
        public virtual InsuranceUser Author { get; set; }

        public string PolicyId { get; set; }
        public virtual Policy Policy { get; set; }

        public int? ClaimId { get; set; }
        public virtual Claim Claim { get; set; }
    }
}