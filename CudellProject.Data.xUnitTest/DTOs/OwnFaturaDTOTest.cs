using CudellProject.Data.DTOs;
using System;
using Xunit;

namespace CudellProject.Data.xUnitTest.DTOs
{
    public class FaturaTest
    {
        [Fact]
        public void TestConstructorWithValidParameters()
        {
            var fornecedor = "FornecedorTest1";
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = 180.33;
            var estado = "Por Aprovar";

            OwnFaturaDTO faturaPendente = new OwnFaturaDTO(fornecedor, dataFatura, dataVencimento, valor, estado);

            Assert.Equal(faturaPendente.Fornecedor, fornecedor);
            Assert.Equal(faturaPendente.DataFatura, dataFatura.ToString("dd-MM-yyyy"));
            Assert.Equal(faturaPendente.DataVencimento, dataVencimento.ToString("dd-MM-yyyy"));
            Assert.Equal(faturaPendente.Valor, valor.ToString() + "$");
            Assert.Equal(faturaPendente.Fornecedor, fornecedor);
        }

        [Fact]
        public void TestConstructorWithNullFornecedorParameter()
        {
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = 180.33;
            var estado = "Por Aprovar";

            Action argumentNullException = () => new OwnFaturaDTO(null, dataFatura, dataVencimento, valor, estado);

            Assert.Throws<ArgumentNullException>(argumentNullException);
        }

        [Fact]
        public void TestConstructorWithNullEstadoParameter()
        {
            var fornecedor = "FornecedorTest1";
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = 180.33;

            Action argumentNullException = () => new OwnFaturaDTO(fornecedor, dataFatura, dataVencimento, valor, null);

            Assert.Throws<ArgumentNullException>(argumentNullException);
        }
    }
}
