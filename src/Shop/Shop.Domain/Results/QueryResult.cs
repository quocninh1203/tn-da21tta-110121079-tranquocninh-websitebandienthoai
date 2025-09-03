namespace Shop.Domain.Results
{
    public class QueryResult<TModel>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public int Code { get; set; } = StatusCodes.StatusCode.Ok;
        public TModel? Model { get; set; }
    }
}
