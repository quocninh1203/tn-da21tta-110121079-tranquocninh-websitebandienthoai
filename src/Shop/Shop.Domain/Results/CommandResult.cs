namespace Shop.Domain.Results
{
    public class CommandResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public int Code { get; set; } = StatusCodes.StatusCode.Ok;
    }
}
