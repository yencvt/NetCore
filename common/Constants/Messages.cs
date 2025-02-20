using System;
using common.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace common.Constants
{
	public class Messages
	{
        public int Status { get; }
        public string Code { get; }
        public string Message { get; }

        private Messages(int status, string code, string message)
        {
            Status = status;
            Code = code;
            Message = message;
        }

        public static readonly Messages Success = new Messages(StatusCodes.Status200OK, "SUCCESS_00", "Successful.");
        public static readonly Messages Created = new Messages(StatusCodes.Status201Created, "SUCCESS_00", "Created.");
        public static readonly Messages Accepted = new Messages(StatusCodes.Status202Accepted, "SUCCESS_00", "Accepted.");
        public static readonly Messages NonAuthoritativeInformation = new Messages(StatusCodes.Status203NonAuthoritative, "SUCCESS_00", "Non authoritative information.");
        public static readonly Messages NoContent = new Messages(StatusCodes.Status204NoContent, "SUCCESS_00", "No content.");
        public static readonly Messages ResetContent = new Messages(StatusCodes.Status205ResetContent, "SUCCESS_00", "Reset content.");
        public static readonly Messages PartialContent = new Messages(StatusCodes.Status206PartialContent, "SUCCESS_00", "Partial content.");

        public static readonly Messages InvalidDataInput = new Messages(StatusCodes.Status400BadRequest, "BAD_REQ_001", "Invalid data input.");
        public static readonly Messages NotFound = new Messages(StatusCodes.Status404NotFound, "BAD_REQ_002", "Not found.");
        public static readonly Messages BadRequest = new Messages(StatusCodes.Status400BadRequest, "BAD_REQ_003", "Bad request.");
        public static readonly Messages Unauthorized = new Messages(StatusCodes.Status401Unauthorized, "BAD_REQ_004", "Unauthorized.");
        public static readonly Messages InternalServerError = new Messages(StatusCodes.Status500InternalServerError, "SER_ERR_01", "Internal server error.");
        public static readonly Messages CustomError = new Messages(1000, "CUS_ERR_01", "Lỗi tùy chỉnh.");
    }
}
