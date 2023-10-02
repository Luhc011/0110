using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo é obrigatório")]
    public string Slug { get; set; } = string.Empty;

}
