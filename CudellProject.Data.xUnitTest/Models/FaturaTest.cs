using CudellProject.Data.Models;
using System;
using Xunit;

namespace CudellProject.Data.xUnitTest.Models
{
    public class FaturaTest
    {
        [Fact]
        public void TestConstructorWithValidParameters()
        {
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = Convert.ToDecimal(180.33);
            var fornecedorID = (short)1;
            var insertUser = "TestUser";
            var responsavelFatura = "TestUser";

            Fatura fatura = new Fatura(dataFatura, dataVencimento, valor, fornecedorID, insertUser, responsavelFatura);

            Assert.Equal(fatura.DataFatura, dataFatura);
            Assert.Equal(fatura.DataVencimento, dataVencimento);
            Assert.Equal(fatura.Valor, valor);
            Assert.Equal(fatura.FornecedorID, fornecedorID);
            Assert.Equal(fatura.InsertUser, insertUser);
            Assert.Equal(fatura.InsertDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            Assert.Equal(fatura.AlterUser, insertUser);
            Assert.Equal(fatura.AlterDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            Assert.Equal(fatura.ResponsavelFatura, insertUser);
            Assert.Equal(1, fatura.EstadoFaturaID);
        }

        [Fact]
        public void TestConstructorWithNegativeIDParameters()
        {
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = Convert.ToDecimal(180.33);
            var fornecedorID = (short)-1;
            var insertUser = "TestUser";
            var responsavelFatura = "TestUser";

            Action argumentException = () => new Fatura(dataFatura, dataVencimento, valor, fornecedorID, insertUser, responsavelFatura);
            Assert.Throws<ArgumentException>(argumentException);
        }

        [Fact]
        public void TestConstructorWithNull1Parameters()
        {
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = Convert.ToDecimal(180.33);
            var fornecedorID = (short)1;
            var responsavelFatura = "TestUser";

            Action argumentException = () => new Fatura(dataFatura, dataVencimento, valor, fornecedorID, null, responsavelFatura);
            Assert.Throws<ArgumentNullException>(argumentException);
        }

        [Fact]
        public void TestConstructorWithNull2Parameters()
        {
            var dataFatura = new DateTime();
            var dataVencimento = new DateTime();
            var valor = Convert.ToDecimal(180.33);
            var fornecedorID = (short)1;
            var insertUser = "TestUser";

            Action argumentException = () => new Fatura(dataFatura, dataVencimento, valor, fornecedorID, insertUser, null);
            Assert.Throws<ArgumentNullException>(argumentException);
        }
    }
}
