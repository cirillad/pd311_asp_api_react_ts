using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.DAL.Entities
{
    public class RefreshToken : BaseEntity<string>
    {
        [Required]
        [MaxLength(450)]
        public required string Token { get; set; }
        [Required]
        [MaxLength(255)]
        public required string AccessId { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime ExpiredDate { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

    }
}
