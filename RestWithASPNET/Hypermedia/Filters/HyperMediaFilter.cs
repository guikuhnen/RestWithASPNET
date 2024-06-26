using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNET.Hypermedia.Filters
{
	public class HyperMediaFilter(HyperMediaFilterOptions hyperMediaFilterOptions) : ResultFilterAttribute
	{
        private readonly HyperMediaFilterOptions _hyperMediaFilterOptions = hyperMediaFilterOptions;

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			TryEnrichResult(context);
			base.OnResultExecuting(context);
		}

		private void TryEnrichResult(ResultExecutingContext context)
		{
			if (context.Result is OkObjectResult okObjectResult)
			{
				var enricher = _hyperMediaFilterOptions.ContentResponseEnricherList.FirstOrDefault(e => e.CanEnrich(context));

				if (enricher != null)				
					Task.FromResult(enricher.Enrich(context));				
			}
		}
	}
}
