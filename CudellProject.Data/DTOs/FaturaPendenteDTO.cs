using System;

namespace CudellProject.Data.DTOs
{
    public class FaturaPendenteDTO
    {
        public long FaturaID;
        public string Fornecedor;
        public string DataFatura;
        public string DataVencimento;
        public string Valor;

        public FaturaPendenteDTO(long faturaID, string fornecedor, DateTime dataFatura, DateTime dataVencimento, Double valor)
        {
            if (faturaID <= 0)
            {
                throw new ArgumentException("FaturaID is " + faturaID);
            }

            FaturaID = faturaID;
            Fornecedor = fornecedor ?? throw new ArgumentNullException(nameof(fornecedor));
            DataFatura = dataFatura.ToString("dd-MM-yyyy");
            DataVencimento = dataVencimento.ToString("dd-MM-yyyy");
            Valor = (valor.ToString() + "$");
        }
    }
}
