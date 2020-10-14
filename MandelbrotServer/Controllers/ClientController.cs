using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MandelbrotLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MandelbrotServer.Controllers
{
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpPost]
        [Route("api/client")]
        public ActionResult<TripleResult> CalculationRequest([FromBody] CalculationRequest calculationRequest)
        {
            var calculator = new MandelbrotCalculator();

            var result = calculator.Calculate(calculationRequest.Height, calculationRequest.Width);

            var resultDto = result.Select(item => new TripleResult()
            {
                X = item.Item1,
                Y = item.Item2,
                Iteration = item.Item3
            }).ToList();

            //var resultDto = result.Select(item => new TripleResultNew()
            //{
            //    Result = item
            //}).ToList();

            return Ok(resultDto);
        }
    }
}
