using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorDesktop.Components
{
	public class BaseComponent  : ComponentBase, IBaseComponent, IDisposable
	{
        [Parameter]
        public ForwardRef RefBack { get; set; }

		[Inject]
		protected IJSRuntime Js { get; set; }
		protected bool Disposed { get; private set; }

		public virtual void Dispose()
		{
			Disposed = true;
		}

		protected async Task<T> JsInvokeAsync<T>(string code, params object[] args)
		{
			try
			{
				return await Js.InvokeAsync<T>(code, args);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
