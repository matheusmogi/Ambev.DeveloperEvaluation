namespace Ambev.DeveloperEvaluation.Shared.Pagination;

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }
}
