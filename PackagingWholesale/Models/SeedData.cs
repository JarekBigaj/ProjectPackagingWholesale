using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace PackagingWholesale.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if(!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Worek jutowy",
                        Description = "Wymiary: 650x1050",
                        Category = "Opakowania jutowe",
                        Price = 5.50m
                    },
                    new Product
                    {
                        Name = "Worek foliowy",
                        Description = "Wymiary: 300x400",
                        Category = "Opakowania foliowe",
                        Price = 15.50m
                    },
                    new Product
                    {
                        Name = "Worek raszlowy",
                        Description = "Wymiary: 300x500",
                        Category = "Opakowania raszlowe",
                        Price = 22.25m
                    },
                    new Product
                    {
                        Name = "Worek jutowy",
                        Description = "Wymiary: 500x850",
                        Category = "Opakowania jutowe",
                        Price = 4.20m
                    },
                    new Product
                    {
                        Name = "Domestos",
                        Description = "Do czyszczenia toalet",
                        Category = "Chemia",
                        Price = 7.80m
                    },
                    new Product
                    {
                        Name = "Folia ogrodnicza 4-sezonowa",
                        Description = "szerokość 6m",
                        Category = "Folie",
                        Price = 1.75m
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
