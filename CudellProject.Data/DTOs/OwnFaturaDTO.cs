using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CudellProject.Data.DTOs
{
    public class OwnFaturaDTO
    {
        public string Fornecedor;
        public string DataFatura;
        public string DataVencimento;
        public string Valor;
        public string Estado;

        public OwnFaturaDTO(string fornecedor, DateTime dataFatura, DateTime dataVencimento, Double valor, string estado)
        {
            Fornecedor = fornecedor ?? throw new ArgumentNullException(nameof(fornecedor));
            DataFatura = dataFatura.ToString("dd-MM-yyyy");
            DataVencimento = dataVencimento.ToString("dd-MM-yyyy");
            Valor = (valor.ToString() + "$");
            Estado = estado ?? throw new ArgumentNullException(nameof(estado));
        }
    }
}
