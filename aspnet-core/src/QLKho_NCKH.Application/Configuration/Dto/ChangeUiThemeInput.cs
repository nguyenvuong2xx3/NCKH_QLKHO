using System.ComponentModel.DataAnnotations;

namespace QLKho_NCKH.Configuration.Dto
{
    public class ChangeUiThemeInput
    {
        [Required]
        [StringLength(32)]
        public string Theme { get; set; }
    }
}
