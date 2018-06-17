﻿using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Data.Abstract;
using Data.Products.Queries;
using Data.Products.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Products.QueryHandlers
{
    public class GetProductsHandlerEf : IPageHandler<GetProductListEf, ProductEf>
    {
        private readonly EfDataContext _context;

        public GetProductsHandlerEf(EfDataContext context)
        {
            _context = context;
        }

        public async Task<IPageResult<ProductEf>> Handle(GetProductListEf request, CancellationToken cancellationToken)
        {
            var count = await _context.Products
                .CountAsync(cancellationToken);

            var page = await _context.Products
                .OrderBy(x => x.Id)
                .Skip(request.Skip())
                .Take(request.Take())
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new PageResult<ProductEf>(count, page);
        }
    }
}
