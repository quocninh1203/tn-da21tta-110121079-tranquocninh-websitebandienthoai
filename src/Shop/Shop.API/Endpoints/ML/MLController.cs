using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.ML;

namespace Shop.API.Endpoints.ML
{
    [ApiController]
    [Route("api/[controller]")]
    public class MLController : ControllerBase
    {
        private readonly RecommendationService _recommendationService;

        public MLController(RecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost("train-model-recommend")]
        public async Task<IActionResult> TrainModel()
        {
            try
            {
                // Gọi trực tiếp phương thức TrainAndSaveModel để huấn luyện lại mô hình
                await _recommendationService.TrainAndSaveModel();

                return Ok("Mô hình đã được huấn luyện lại và lưu thành công.");
            }
            catch (Exception ex)
            {
                // Xử lý và trả về lỗi nếu có vấn đề xảy ra trong quá trình huấn luyện
                return StatusCode(500, $"Lỗi nội bộ server khi huấn luyện mô hình: {ex.Message}");
            }
        }
    }
}
