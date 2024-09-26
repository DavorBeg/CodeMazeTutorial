using Entities.Responses.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses
{
	public class ApiOkResponse<TResult> : ApiBaseResponse
	{
		public TResult Result { get; set; }
		public ApiOkResponse(TResult result) : base(true)
		{
			Result = result;
		}
	}
}
