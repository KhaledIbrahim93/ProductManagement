using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductBLL.DTO;
using ProductBLL.Repositories;
using ProductDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ProductBLL.Services
{
    public class ProductSerivce : IDataRepository<ProductVM>
    {
        private readonly Context context;

        public ProductSerivce()
        {
            context = new Context();
        }
        public ProductSerivce(Context _context)
        { 
            this.context = _context;
        }

        public async void Add(ProductVM entity)
        {
            var product = new Product()
            {
                Name = entity.Name,
                CreateDate = DateTime.Now,
                Price = entity.Price,
                Photo = entity.URL,
                LastUpdate = null
            };
          await context.Products.AddAsync(product);
            context.SaveChanges();
        }

        public async void Delete(int id)
        {
            var entity =await context.Products.FindAsync(id);
            context.Products.Remove(entity);
            context.SaveChanges();
        }

        public async Task<ProductVM> Get(int id)
        {
            var entity =await context.Products.FindAsync(id);
            return new ProductVM()
            {
             Name=entity.Name,
             URL=entity.Photo,
             CreateDate=entity.CreateDate,
             Price=entity.Price,
             LastUpdate=entity.LastUpdate,
              Id=entity.Id
            };
         
        }

        public async Task<IEnumerable<ProductVM>> GetAll()
        {
            var enttity =await context.Products.ToListAsync();
            return enttity.Select(e => new ProductVM()
            {
                Name=e.Name,
                Price=e.Price,
                CreateDate=e.CreateDate,
                URL=e.Photo,
                Id=e.Id,
                LastUpdate=e.LastUpdate

            }).ToList();
        }

        public async void Update(ProductVM dbEntity, int id)
        {
            var entity = context.Products.Find(id);
            var createData = context.Products.Where(e => e.Id == id).Select(c => c.CreateDate).FirstOrDefault();
            entity.Name = dbEntity.Name;
            entity.Price = dbEntity.Price;
            entity.CreateDate = createData;
            entity.Photo = dbEntity.URL;
            entity.LastUpdate = DateTime.Now;
           await context.SaveChangesAsync();
        }
        
        public string GetOldePhotoPath(int id)
        {
          return context.Products.Where(e => e.Id == id).Select(e => e.Photo).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductVM>> Search(string value) {
            var enttity =await context.Products.Where(e => e.Name.Contains(value)).ToListAsync();
            return enttity.Select(e => new ProductVM()
            {
                Name = e.Name,
                Price = e.Price,
                CreateDate = e.CreateDate,
                URL = e.Photo,
                Id = e.Id,
                LastUpdate = e.LastUpdate

            }).ToList();
        }


    }
}
