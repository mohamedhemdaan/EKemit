using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EKemit.Core.Entities;
using EKemit.Core.Entities.Order_Aggregate;

namespace EKemit.Repository.Data
{
    //public static class StoreContextSeed
    //{
    //    public static async Task SeedAsync(StoreContext dbContext)
    //    {

    //        if (!dbContext.ProductBrands.Any())
    //        {
    //            var BrandsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/brands.json");
    //            var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
    //            if (Brands?.Count > 0)
    //            {
    //                foreach (var brand in Brands)
    //                {
    //                    await dbContext.Set<ProductBrand>().AddAsync(brand);
    //                }
    //                await dbContext.SaveChangesAsync();
    //            }
    //        }

    //        if (!dbContext.ProductTypes.Any())
    //        {
    //            var TypesData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/types.json");
    //            var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
    //            if (Types?.Count > 0)
    //            {
    //                foreach (var type in Types)
    //                {
    //                    await dbContext.Set<ProductType>().AddAsync(type);
    //                }
    //                await dbContext.SaveChangesAsync();
    //            }
    //        }

    //        if (!dbContext.Governrates.Any())
    //        {
    //            var GovernratesData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/governrates.json");
    //            var Governrates = JsonSerializer.Deserialize<List<Governrate>>(GovernratesData);
    //            if (Governrates?.Count > 0)
    //            {
    //                foreach (var governrate in Governrates)
    //                {
    //                    await dbContext.Set<Governrate>().AddAsync(governrate);
    //                }
    //                await dbContext.SaveChangesAsync();
    //            }
    //        }

    //        if (!dbContext.Products.Any())
    //        {
    //            var ProductsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/products.json");
    //            var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
    //            if (Products?.Count > 0)
    //            {
    //                foreach (var product in Products)
    //                    await dbContext.Set<Product>().AddAsync(product);
    //                await dbContext.SaveChangesAsync();
    //            }
    //        }

    //        if (!dbContext.DeliveryMethods.Any())
    //        {
    //            var DeliveryMethodsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/delivery.json");
    //            var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
    //            if (DeliveryMethods?.Count > 0)
    //            {
    //                foreach (var deliveryMethod in DeliveryMethods)
    //                    await dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);
    //                await dbContext.SaveChangesAsync();
    //            }
    //        }


    //    }
    //}

    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Governrates.Any())
            {
                var GovernratesData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/governrates.json");
                var Governrates = JsonSerializer.Deserialize<List<Governrate>>(GovernratesData);
                if (Governrates?.Count > 0)
                {
                    foreach (var governrate in Governrates)
                    {
                        await dbContext.Set<Governrate>().AddAsync(governrate);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var product in Products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                        await dbContext.SaveChangesAsync();
                    }
                    //    await dbContext.Set<Product>().AddAsync(product);
                    //await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in DeliveryMethods)
                        await dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Comments.Any())
            {
                var CommentsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/comments.json");
                var Comments = JsonSerializer.Deserialize<List<Comment>>(CommentsData);
                if (Comments?.Count > 0)
                {
                    foreach (var comment in Comments)
                    {
                        await dbContext.Set<Comment>().AddAsync(comment);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Ratings.Any())
            {
                var RatingsData = File.ReadAllText("../EKemit.Repository/Data/DataSeed/ratings.json");
                var Ratings = JsonSerializer.Deserialize<List<Rating>>(RatingsData);
                if (Ratings?.Count > 0)
                {
                    foreach (var rating in Ratings)
                    {
                        await dbContext.Set<Rating>().AddAsync(rating);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }


        }
    }

}
