using System;
namespace common.Models.Base
{
	public class RequestBase<T>
	{
        public RequestBase(T body)
		{
			this.body = body;

        }

		public T body { get; set; }
	}
}

