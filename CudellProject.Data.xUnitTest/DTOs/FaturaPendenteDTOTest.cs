using CudellProject.Data.DTOs;
using System;
using Xunit;

namespace CudellProject.Data.xUnitTest.DTOs
{
    public class FaturaPendenteDTOTest
    {
        [Fact]
        public void TestConstructorWithValidParameters()
        {
            long faturaID = 114141;
            var fornecedor = "FornecedorTest1";
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = 180.33;

            FaturaPendenteDTO faturaPendente = new FaturaPendenteDTO(faturaID, fornecedor, dataFatura, dataVencimento, valor);

            Assert.Equal(faturaPendente.Fornecedor, fornecedor);
            Assert.Equal(faturaPendente.DataFatura, dataFatura.ToString("dd-MM-yyyy"));
            Assert.Equal(faturaPendente.DataVencimento, dataVencimento.ToString("dd-MM-yyyy"));
            Assert.Equal(faturaPendente.Valor, valor.ToString() + "$");
        }

        [Fact]
        public void TestConstructorWithZeroFaturaIDParameter()
        {
            long faturaID = 0;
            var fornecedor = "FornecedorTest1";
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = 180.33;

            Action argumentException = () => new FaturaPendenteDTO(faturaID, fornecedor, dataFatura, dataVencimento, valor);

            Assert.Throws<ArgumentException>(argumentException);
        }

        [Fact]
        public void TestConstructorWithNullFornecedorParameter()
        {
            long faturaID = 114141;
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = 180.33;

            Action argumentNullException = () => new FaturaPendenteDTO(faturaID, null, dataFatura, dataVencimento, valor);

            Assert.Throws<ArgumentNullException>(argumentNullException);
        }
    }
}
