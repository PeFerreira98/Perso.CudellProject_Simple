using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CudellProject.Data.Models.ViewModels
{
    public class EditFaturaViewModel
    {
        public List<SelectListItem> EstadosFatura { get; } = new List<SelectListItem> { };
        public List<SelectListItem> Fornecedores { get; } = new List<SelectListItem> { };

        [Required]
        public long FaturaID { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public string Fornecedor { get; set; }

        [Required]
        public string EstadoFatura { get; set; }
    }
}