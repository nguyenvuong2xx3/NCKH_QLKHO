using System.ComponentModel.DataAnnotations;

namespace QLKho_NCKH.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}