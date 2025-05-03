using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StorageLocations.Dto
{
	public class StorageLocationInput : PagedAndSortedResultRequestDto, IShouldNormalize
	{

		public string Filter { get; set; }
		public void Normalize()
		{
			if (Sorting.IsNullOrWhiteSpace())
			{
				Sorting = "CreationTime DESC";
			}
		}
	}
}
