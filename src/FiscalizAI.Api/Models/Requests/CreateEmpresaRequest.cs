using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Api.Models.Requests;

    public class CreateEmpresaRequest
    {
        [Required(ErrorMessage = "A razão social é obrigatória")]
        [StringLength(200, ErrorMessage = "A razão social deve ter no máximo 200 caracteres")]
        public string RazaoSocial { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        public string Cnpj { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "A UF é obrigatória")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter exatamente 2 caracteres")]
        public string Uf { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "A senha do certificado é obrigatória")]
        public string SenhaCertificado { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "O arquivo do certificado digital é obrigatório")]
        public IFormFile ArquivoCertificado { get; set; } = null!; 

    }
