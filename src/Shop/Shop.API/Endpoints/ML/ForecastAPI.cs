using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.ML;

namespace Shop.API.Endpoints.ML
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastAPI : ControllerBase
    {
        private readonly IForecastService _forecastService;

        public ForecastAPI(IForecastService forecastService)
        {
            _forecastService = forecastService;
        }

        // GET: /api/forecast/revenue
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueForecast()
        {
            var result = await _forecastService.GetRevenueForecastAsync();
            return Ok(result);
        }

        // GET: /api/forecast/top-products
        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            var result = await _forecastService.GetTopSellingProductsAsync();
            return Ok(result);
        }

        // GET: /api/forecast/recommend-products
        //[HttpGet("recommend-products")]
        //public async Task<IActionResult> RecommendProducts(int userId)
        //{
        //    var result = await _forecastService.RecommendProductsAsync(userId);
        //    return Ok(result);
        //}
    }
}
