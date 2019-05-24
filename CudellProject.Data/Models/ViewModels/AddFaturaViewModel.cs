using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CudellProject.Data.Models.ViewModels
{
    public class AddFaturaViewModel
    {
        public List<SelectListItem> Fornecedores { get; } = new List<SelectListItem> { };

        [Required]
        [DataType(DataType.Date)]
        public string DataFatura { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string DataVencimento { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public string Fornecedor { get; set; }
    }
}
