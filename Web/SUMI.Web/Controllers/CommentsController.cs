namespace SUMI.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Comments;

    [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
    public class CommentsController : BaseController
    {
        private readonly ICommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string text, string policyId, int? claimId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            var newPolicy = new Comment()
            {
                Text = text,
                AuthorId = currentUser.Id,
                PolicyId = policyId,
            };
            if (policyId != null)
            {
                newPolicy.PolicyId = policyId;
            }
            else if (claimId != null)
            {
                newPolicy.ClaimId = claimId;
            }

            await this.commentService.Create(newPolicy);
            return this.Ok();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.commentService.RemoveComment(id);
            return this.Ok();
        }
    }
}
