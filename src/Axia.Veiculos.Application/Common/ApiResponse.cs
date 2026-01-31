namespace Axia.Veiculos.Application.Common;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}
