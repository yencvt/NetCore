using System;
using common.Constants;

namespace common.Models.Base
{
	public class ResponseBase<T>
	{
        public ResponseBase()
        {
        }

        public ResponseBase(T body)
        {
            this.body = body;
        }

        public ResponseBase(string code, string message, T body)
        {
			this.code = code;
			this.message = message;
			this.body = body;
		}

		public string code { get; set; } = Messages.Success.Code;
		public string message { get; set; } = Messages.Success.Message;
		public T body { get; set; }
	}
}

