namespace Axia.Veiculos.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public int StatusCode { get; }
    public IEnumerable<string>? Errors { get; }

    protected Result(bool isSuccess, string message, int statusCode, IEnumerable<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
        Errors = errors;
    }

    public static Result Success(string message = "Operação realizada com sucesso")
        => new(true, message, 200);

    public static Result Created(string message = "Registro criado")
        => new(true, message, 201);

    public static Result BadRequest(string message = "Dados inválidos", IEnumerable<string>? errors = null)
        => new(false, message, 400, errors);

    public static Result Unauthorized(string message = "Credenciais inválidas")
        => new(false, message, 401);

    public static Result Forbidden(string message = "Sem permissão para este recurso")
        => new(false, message, 403);

    public static Result NotFound(string message = "Registro não encontrado")
        => new(false, message, 404);

    public static Result InternalError(string message = "Erro interno do servidor")
        => new(false, message, 500);
}

public class Result<T> : Result
{
    public T? Data { get; }

    private Result(bool isSuccess, string message, int statusCode, T? data = default, IEnumerable<string>? errors = null)
        : base(isSuccess, message, statusCode, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string message = "Sucesso")
        => new(true, message, 200, data);

    public static Result<T> Created(T data, string message = "Criado com sucesso")
        => new(true, message, 201, data);

    public static new Result<T> BadRequest(string message = "Requisição inválida", IEnumerable<string>? errors = null)
        => new(false, message, 400, default, errors);

    public static new Result<T> Unauthorized(string message = "Não autorizado")
        => new(false, message, 401);

    public static new Result<T> Forbidden(string message = "Acesso negado")
        => new(false, message, 403);

    public static new Result<T> NotFound(string message = "Não encontrado")
        => new(false, message, 404);

    public static new Result<T> InternalError(string message = "Erro no servidor")
        => new(false, message, 500);
}
