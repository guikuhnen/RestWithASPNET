﻿using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Constants;
using System.Text;

namespace RestWithASPNET.Hypermedia.Enricher
{
	public class PersonEnricher : ContentResponseEnricher<PersonVO>
	{
		private readonly object _lock = new();

		protected override Task EnrichModel(PersonVO content, IUrlHelper urlHelper)
		{
			var path = "api/person";
			string link = GetLink(content.Id, urlHelper, path);

			content.Links.Add(new HyperMediaLink()
			{
				Action = HttpActionVerb.GET,
				Href = link,
				Rel = RelationType.self,
				Type = ResponseTypeFormat.DefaultGet
			});
			content.Links.Add(new HyperMediaLink()
			{
				Action = HttpActionVerb.POST,
				Href = link,
				Rel = RelationType.self,
				Type = ResponseTypeFormat.DefaultPost
			});
			content.Links.Add(new HyperMediaLink()
			{
				Action = HttpActionVerb.PUT,
				Href = link,
				Rel = RelationType.self,
				Type = ResponseTypeFormat.DefaultPut
			}); 
			content.Links.Add(new HyperMediaLink()
			{
				Action = HttpActionVerb.DELETE,
				Href = link,
				Rel = RelationType.self,
				Type = "int"
			});
			content.Links.Add(new HyperMediaLink()
			{
				Action = HttpActionVerb.PATCH,
				Href = link,
				Rel = RelationType.self,
				Type = ResponseTypeFormat.DefaultPatch
			});

			return Task.CompletedTask;
		}

		private string GetLink(long id, IUrlHelper urlHelper, string path)
		{
			lock (_lock)
			{
				var url = new { controller = path, id };
				return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
			};
		}
	}
}
