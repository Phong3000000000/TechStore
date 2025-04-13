﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Repository.Components
{
	public class BrandsViewComponent:ViewComponent
	{
		private readonly DataContext _Datacontext;
		public BrandsViewComponent(DataContext context)
		{
			_Datacontext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() =>View(await _Datacontext.Brands.ToListAsync());
	}

}
