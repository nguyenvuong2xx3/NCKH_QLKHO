using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Products.Dtos
{
    public class UpdateProductDto : EntityDto, IHasCreationTime
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public IFormFile ImageFile { get; set; }

        //public ProductState State { get; set; }

        public DateTime CreationTime { get; set; }

		    public int? CategoryId { get; set; } 
	}
}
