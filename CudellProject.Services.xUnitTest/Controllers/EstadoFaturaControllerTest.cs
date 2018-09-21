using CudellProject.Data.Contexts;
using CudellProject.Data.Models;
using CudellProject.Services.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CudellProject.Services.xUnitTest.Controllers
{
    public class EstadoFaturaControllerTest
    {
        private DemoDbContext _context;
        private ILogger<EstadoFaturaController> _logger;
        private EstadoFaturaController _controller;

        public EstadoFaturaControllerTest()
        {
            DbContextOptions<DemoDbContext> _options = new DbContextOptionsBuilder<DemoDbContext>().UseInMemoryDatabase(databaseName: "DemoDbContext_TestDatabase").Options;
            var context = new DemoDbContext(_options);

            var estadoFaturas = Enumerable.Range(1, 5).Select(i => new EstadoFatura { EstadoFaturaID = (short)i, DescritivoEstadoFatura = ("TesteDescritivo" + i) });

            if (context.EstadoFatura.ToList().Count() == 0)
            {
                context.EstadoFatura.AddRange(estadoFaturas);
                int changed = context.SaveChanges();
            }

            _context = context;
            _logger = Mock.Of<ILogger<EstadoFaturaController>>();
            _controller = new EstadoFaturaController(_context, _logger);
        }

        [Fact]
        public void GetEstadoFatura()
        {
            var result = _controller.GetEstadoFatura();
            Assert.NotNull(result);

            Assert.Equal(5, result.Count());
            Assert.Equal(1, result.First().EstadoFaturaID);
            Assert.Equal("TesteDescritivo1", result.First().DescritivoEstadoFatura);
            Assert.Equal(5, result.Last().EstadoFaturaID);
            Assert.Equal("TesteDescritivo5", result.Last().DescritivoEstadoFatura);
        }

        [Fact]
        public void GetEstadoFaturaById()
        {
            var resultTask = _controller.GetEstadoFatura(1);
            Assert.NotNull(resultTask);
            IActionResult resultIAction = resultTask.Result;

            var okObjectResult = resultIAction as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as EstadoFatura;
            Assert.NotNull(model);
            
            Assert.Equal(1, model.EstadoFaturaID);
            Assert.Equal("TesteDescritivo1", model.DescritivoEstadoFatura);
        }
    }
}

/*
 //Act 
            var response = await _client.GetAsync(_urlBase);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Farmacia>>(content);

            //Assert 
            Assert.NotNull(result);
            Assert.IsType<List<Farmacia>>(result);
            Assert.IsType<int>(result.Count);

            return result.Count;
            */
//[{"estadoFaturaID":1,"descritivoEstadoFatura":"Por Aprovar"},
//{"estadoFaturaID":2,"descritivoEstadoFatura":"Aprovada"},
//{"estadoFaturaID":3,"descritivoEstadoFatura":"Rejeitada"},
//{"estadoFaturaID":4,"descritivoEstadoFatura":"Paga"}]

/*
var controller = new EstadoFaturaController(_context, _logger);

IActionResult actionResult = await controller.GetEstadoFatura(2);
Assert.NotNull(actionResult);

OkObjectResult result = actionResult as OkObjectResult;
Assert.NotNull(result);

Console.Write(result.ToString());

List<string> messages = result.Value as List<string>;
Assert.NotNull(messages);

Console.WriteLine(messages);

Assert.Equal(4, messages.Count);
Assert.Equal("value1", messages[0]);
Assert.Equal("value2", messages[1]);
*/

/*
[TestClass]
public class MvcMoviesControllerTests
{
    [TestMethod]
    public async Task MoviesControllerIndex()
    {
        var mockContext = new Mock<ApplicationDbContext>();            
        var controller = new MoviesController(mockContext.Object);

        // Act
        var result = await controller.Index();

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }
*/

/*
var mockSet = new Mock<DbSet<EstadoFatura>>();
var context = new Mock<DemoDbContext>(_options);
context.Setup(m => m.EstadoFatura).Returns(mockSet.Object);
*/

/*
var builder = new DbContextOptionsBuilder<DemoDbContext>();
builder.UseInMemoryDatabase();
var context = new DemoDbContext(builder.Options);
*/
