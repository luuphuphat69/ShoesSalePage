using ShoesSalePage.Models;
using System.Collections.Generic;

namespace ShoesSalePage.Data
{
    public class ShoesInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ShoesDbContext>
    {
        protected override void Seed(ShoesDbContext context)
        {
            var listShoes = new List<ShoesModel>()
            {
                new ShoesModel{Id=1, Name="Giày da nam công sở NL83", Price = 490000, Brand = "Sunview", Size = 38, Color = "Black", Image="~/Content/Images/ProductImg/shoe1.jpg", IsAvailable=true},
                new ShoesModel{Id=2, Name="Giày da nam NL82", Price = 190000, Brand = "Raisins", Size = 38, Color = "Black", Image="~/Content/Images/ProductImg/shoe2.jpg", IsAvailable=true},
                new ShoesModel{Id=3, Name="Giày da nam EL81", Price = 290000, Brand = "Elly", Size = 38, Color = "Brown", Image="~/Content/Images/ProductImg/shoe3.jpg", IsAvailable=true},
                new ShoesModel{Id=4, Name="Giày da nam EL80", Price = 390000, Brand = "Elly", Size = 38, Color = "Black", Image="~/Content/Images/ProductImg/shoe4.jpg", IsAvailable = true},
                new ShoesModel{Id=5, Name="Giày da nam bóng NL85", Price = 490000, Brand = "Elly", Size = 38, Color = "Black", Image="~/Content/Images/ProductImg/shoe5.jpg", IsAvailable=true},
                new ShoesModel{Id=6, Name="Giày da nam bóng NL84", Price = 590000, Brand = "Elly", Size = 38, Color = "Black", Image="~/Content/Images/ProductImg/shoe6.jpg", IsAvailable=true},
                new ShoesModel{Id=7, Name="Giày da nam bóng NL89", Price = 430000, Brand = "Elly", Size = 38, Color = "Black", Image="~/Content/Images/ProductImg/shoe7.jpg", IsAvailable=true},
                new ShoesModel{Id=8, Name="Giày da nam LS69", Price = 420000, Brand = "Uniquilo", Size = 38, Color = "Brown", Image="~/Content/Images/ProductImg/shoe8.jpg", IsAvailable=true},
                new ShoesModel{Id=9, Name="Giày tây da nam đen bóng LW37", Price = 1200000, Brand = "Hobits", Size = 38, Color = "Brown", Image="~/Content/Images/ProductImg/shoe9.jpg", IsAvailable=true},
                new ShoesModel{Id=10, Name="Giày da nam buộc dây NL12", Price = 450000, Brand = "BingChillin", Size = 38, Color = "Brown", Image="~/Content/Images/ProductImg/shoe10.jpg", IsAvailable=true},
                new ShoesModel{Id=11, Name="Giày da nam buộc dây NL", Price = 450000, Brand = "BingChillin", Size = 38, Color = "Brown", Image="~/Content/Images/ProductImg/shoe10.jpg", IsAvailable=true},
            };
            listShoes.ForEach(s => context.Shoes.Add(s));
            context.SaveChanges();
        }
    }
}