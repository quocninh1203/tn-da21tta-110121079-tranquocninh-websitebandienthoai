using Microsoft.ML.Trainers;
using Microsoft.ML;
using Shop.Application.DTOs.ML;
using Shop.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Shop.Application.Services.ML
{
    public class RecommendationService
    {
        private readonly IUserProductInteractionRepository _userProductInteractionRepository;
        private readonly MLContext _mlContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ITransformer _trainedModel;

        public RecommendationService(IUserProductInteractionRepository userProductInteractionRepository, IWebHostEnvironment webHostEnvironment)
        {
            _userProductInteractionRepository = userProductInteractionRepository;
            _webHostEnvironment = webHostEnvironment;
            _mlContext = new MLContext();
            LoadModel();
        }

        public void LoadModel()
        {
            string modelPath = Path.Combine(_webHostEnvironment.WebRootPath, "ML", "MLModel.zip");

            if (File.Exists(modelPath))
            {
                _trainedModel = _mlContext.Model.Load(modelPath, out var modelSchema);
            }
            else
            {
                TrainAndSaveModel().Wait();
            }
        }

        public async Task TrainAndSaveModel()
        {
            var interactions = await _userProductInteractionRepository.GetAsync();
            var data = interactions.Where(i => i.Label == true).Select(i => new ProductInteraction
            {
                UserId = i.UserId,
                ProductId = i.ProductId,
                Label = i.Label ? 1.0f : 0.0f
            }).ToList();

            var trainingDataView = _mlContext.Data.LoadFromEnumerable(data);

            // Thay đổi lớn ở đây: Tạo một pipeline kết hợp cả tiền xử lý và huấn luyện
            var trainingPipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                outputColumnName: "userIdEncoded",
                inputColumnName: nameof(ProductInteraction.UserId))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                outputColumnName: "productIdEncoded",
                inputColumnName: nameof(ProductInteraction.ProductId)))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "userIdEncoded",
                    MatrixRowIndexColumnName = "productIdEncoded",
                    LabelColumnName = "Label",
                    LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass
                }));

            // Huấn luyện toàn bộ pipeline
            _trainedModel = trainingPipeline.Fit(trainingDataView);

            string modelPath = Path.Combine(_webHostEnvironment.WebRootPath, "ML", "MLModel.zip");
            // Lưu toàn bộ pipeline, bao gồm cả các bước chuyển đổi
            _mlContext.Model.Save(_trainedModel, trainingDataView.Schema, modelPath);
        }

        public ITransformer GetTrainedModel() => _trainedModel;
        public MLContext GetMLContext()
        {
            return _mlContext;
        }
    }
}