using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShAbedi.PayaSystem.Domain.Exceptions;

public class DomainException : Exception
{
    public string ErrorCode { get; }

    public DomainException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class InsufficientBalanceException : DomainException
{
    public InsufficientBalanceException()
        : base("موجودی کافی نیسیت", "INSUFFICIENT_BALANCE")
    {
    }
}

public class ValidationException : DomainException
{
    public ValidationException(string message, string errorCode)
        : base(message, errorCode)
    {
    }
}