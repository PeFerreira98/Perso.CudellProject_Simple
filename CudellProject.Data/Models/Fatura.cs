using System;

namespace CudellProject.Data.Models
{
    public class Fatura
    {
        public Int64 FaturaID { get; set; }
        public DateTime DataFatura { get; set; }
        public DateTime DataVencimento { get; set; }
        public Decimal Valor { get; set; }
        public Int16 FornecedorID { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public string AlterUser { get; set; }
        public DateTime AlterDate { get; set; }
        public string ResponsavelFatura { get; set; }
        public Int16 EstadoFaturaID { get; set; }
        public EstadoFatura EstadoFatura { get; set; }

        protected Fatura() { } //Context Use Only

        public Fatura(DateTime dataFatura, DateTime dataVencimento, decimal valor, short fornecedorID, string insertUser, string responsavelFatura)
        {
            if (fornecedorID <= 0)
            {
                throw new ArgumentException("fornecedorID is invalid");
            }

            DataFatura = dataFatura;
            DataVencimento = dataVencimento;
            Valor = valor;
            FornecedorID = fornecedorID;
            InsertUser = insertUser ?? throw new ArgumentNullException(nameof(insertUser));
            InsertDate = DateTime.Now;
            AlterUser = insertUser ?? throw new ArgumentNullException(nameof(insertUser));
            AlterDate = DateTime.Now;
            ResponsavelFatura = responsavelFatura ?? throw new ArgumentNullException(nameof(responsavelFatura));
            EstadoFaturaID = 1;
        }
    }
}
