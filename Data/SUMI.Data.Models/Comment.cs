namespace SUMI.Data.Models
{
    using SUMI.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public string Text { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string PolicyId { get; set; }

        public virtual Policy Policy { get; set; }

        public int? ClaimId { get; set; }

        public virtual InsuranceClaim Claim { get; set; }
    }
}
