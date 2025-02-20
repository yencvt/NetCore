using System;
using System.Net.NetworkInformation;
using common.Constants;

namespace common.Exceptions
{
    public class CommonException : Exception
    {
        public int Status { get; }
        public string Code { get; }

        public CommonException(Messages message)
            : base(message.Message)
        {
            Status = message.Status;
            Code = message.Code;
        }

        public CommonException(int status, string errorCode, string message)
            : base(message)
        {
            Status = status;
            Code = errorCode;
        }
    }
}


