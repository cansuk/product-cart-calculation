/*
    VIP müşteriler: Eğer müşterinin rolü "VIP" ise %20 indirim uygulanır.
    Diğer indirimler: Ürün kategorisine ve koleksiyon özelliklerine göre ayakkabı için %10, yaz koleksiyonu için %5 ek indirim vardır.
    Kurallar: En faydalı indirim uygulanacak, yani VIP indirimi mi yoksa ürün indirimi mi daha avantajlıysa o kullanılacak.
	
	Mantık Hatasını Bulunuz, İndirim uygulanmış alt toplam: 720.00 USD olması lazım
	Bu tür karşılaştırmalı indirim sistemleri için performans nasıl optimize edilir?
	Eğer ürünler sepete çok fazla ekleniyorsa ve her biri farklı indirimlere tabiyse, bu yapıyı daha sürdürülebilir hale nasıl getirirdiniz?
	Üçüncü bir indirimi uygulasak, Sepette tüm indirimlere ek %10 uygulanmış kodu yazınız.
	Toplam İndirimi Bulunuz
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public List<string> Features { get; set; } = new List<string>();
}

public class Customer
{
    public string Role { get; set; } // Role can be "VIP" or "Regular"
}

public class Cart
{
    public List<Product> Products { get; set; } = new List<Product>();
    public Customer Customer { get; set; }

    // Initial question : Calculate the total discounted price 
    public decimal CalculateDiscountedPrice()
    {
        decimal totalPriceInitial = 0;
        decimal totalPrice = 0;

        foreach (var product in Products)
        {
            totalPriceInitial += product.Price;
            decimal discountedPrice = product.Price;

            // Discounts should be applied from the most beneficial to the least beneficial and just 1 discount can be applied:

            if (Customer.Role == "VIP")
            {
                decimal vipDiscountPrice = product.Price * 0.80m; // 20% discount for VIP
                discountedPrice = Math.Min(discountedPrice, vipDiscountPrice); // Choose the lower price (more beneficial discount)
                totalPrice += discountedPrice;
                continue;
            }

            if (product.Category == "Shoes")
            {
                discountedPrice = discountedPrice * 0.10m; // 10% discount for Shoes
                totalPrice += discountedPrice;
                continue;
            }

            if (product.Features.Contains("Collection: Summer"))
            {
                discountedPrice = discountedPrice * 0.95m; // Additional 5% discount for Summer Collection
                totalPrice += discountedPrice;
                continue;
            }
        }

        // == ANSWERS ==
        // Corrected discounted price: 720.00 USD (totalPrice)
        // Total Discounted Price after additional 10% : 648.00 USD (totalPrice * 90 / 100)
        // Total Discount: 252.00 USD (totalPriceInitial - totalPrice)
        // For Performance Optimization:
        // 1- Caching the discount values for each product and customer role can be used to prevent recalculating the same discount values for each product and customer role. 
        // 2- Also parallel processing can be used for calculating the discounts of each product in parallel.

        totalPrice = totalPrice * 90 / 100; // Additional 10% discount

        return totalPriceInitial - totalPrice;
    }
}

public class Program
{
    public static void Main(string[] args)
    {

        var customer = new Customer
        {
            Role = "VIP"
        };

        // Add products to the cart
        var cart = new Cart { Customer = customer };

        var product1 = new Product
        {
            Name = "Running Shoes",
            Category = "Shoes",
            Price = 300,
            Features = new List<string> { "Collection: Summer", "Color: White" }
        };

        var product2 = new Product
        {
            Name = "T-Shirt",
            Category = "Clothing",
            Price = 100,
            Features = new List<string> { "Collection: Summer", "Color: Black" }
        };

        var product3 = new Product
        {
            Name = "Formal Shoes",
            Category = "Shoes",
            Price = 500,
            Features = new List<string> { "Collection: Winter", "Color: Brown" }
        };

        // Generate 97 more products with different features and categories(Clothing, Shoes, etc.) collections (Summer, Winter, etc.) and prices :
        for (int i = 0; i < 97; i++)
        {
            var product = new Product
            {
                Name = $"Product {i}",
                Category = i % 2 == 0 ? "Shoes" : "Clothing",
                Price = i * 10,
                Features = new List<string> { i % 2 == 0 ? "Collection: Summer" : "Collection: Winter", i % 2 == 0 ? "Color: White" : "Color: Black" }
            };
            cart.Products.Add(product);
        }

        cart.Products.Add(product1);
        cart.Products.Add(product2);
        cart.Products.Add(product3);

        // Calculate the total discounted price
        decimal totalDiscountedPrice = cart.CalculateDiscountedPrice();
        Console.WriteLine($"Total Discounted Price: {totalDiscountedPrice} USD");
    }
}
