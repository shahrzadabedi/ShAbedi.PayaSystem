namespace ShAbedi.PayaSystem.Application.Exceptions;

public class BusinessException : Exception
{
    public string ErrorCode { get; }

    public BusinessException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class AccountNotFoundException : BusinessException
{
    public AccountNotFoundException()
        : base("حساب کاربری یافت نشد", "ACCOUNT_NOT_FOUND")
    {
    }
}

public class ShebaRequestNotFoundException : BusinessException
{
    public ShebaRequestNotFoundException()
        : base(" درخواست انتقال یافت نشد", "ShebaRequest_NOT_FOUND")
    {
    }
}
