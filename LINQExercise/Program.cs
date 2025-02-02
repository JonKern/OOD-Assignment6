﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;

namespace AggregateOperators
{
    class Program
    {
        static void Main(string[] args)
        {
        //Need Help?
        //see https://docs.microsoft.com/en-us/samples/dotnet/try-samples/101-linq-samples/
        //or        https://linqsamples.com/

            LinqSamples samples = new LinqSamples();
            
            List<Customer> customers = samples.GetCustomerList();
            List<Product> products = samples.GetProductList();

            //1. Return a list of customers, ordered by customer name ascending
            List<Customer> customersOrderedByNameAsc = customers.OrderBy(c => c.CompanyName).ToList();
            List<Customer> customersOrderedByNameAsc2 = (from c in customers orderby c.CompanyName select c).ToList();


            List<Order> orders = (from c in customers
                                  from o in c.Orders
                orderby o.OrderDate
                select o).ToList();

            samples.WriteOrders(orders);

            //2. Return a list of products, ordered by unit price descending
            List<Product> pr1 = (from p in products
                orderby p.UnitPrice descending
                select p).ToList();

            samples.WriteProducts(pr1);

            //3. Return the number of products and number of customers
            Console.WriteLine(products.Count);
            Console.WriteLine(customers.Count);

            //4. Return whether there are any products categorized as "Seafood"
            bool pr2 = (from p in products
                        where p.Category == "Seafood"
                        select p).Any();// (p => p.Category == "Seafood");

            Console.WriteLine("Are there are Seafood products?" + pr2);

            //5. Return whether all products are less than $100 per unit
            bool pr3 = (from p in products
                select p).All(p => (p.UnitPrice < 100));

            Console.WriteLine("Are all products less than $100?" + pr3);

            //6. Return the average price per unit of all products in the catalog
            //decimal avgProductPrice = (from p in products
            //    select p).Average(p => p.UnitPrice);

            decimal avgProductPrice = products.Average(p => p.UnitPrice);

            Console.WriteLine("What is the average product price?" + avgProductPrice);

            //7. Return all customers who are from the country "UK"
            //List<Customer> cstUK = (from c in customers
            //                        where c.Country == "UK"
            //                        select c).ToList();

            List<Customer> cstUK = customers.Where(c => c.Country == "UK").ToList();

            samples.WriteCustomers(cstUK);

            //8. Return the list of distinct product categories
            List<string> distinctCategories = (from p in products
                select p.Category).Distinct().ToList();

            //9. Return the product (by name) with the lowest and the highest prices per unit
            Product lowPrice = (from p in products
                orderby p.UnitPrice, p.ProductName
                select p).First();

            Product highestPrice = (from p in products
                orderby p.UnitPrice, p.ProductName
                select p).Last();

            Console.WriteLine("The lowest priced product is:" + 
                lowPrice.ProductName + " at " + lowPrice.UnitPrice);

            //10. Return the cost to buy one unit of each product in the catalog
            decimal totalCostForOneOfEach = (from p in products
                select p).Sum(p => p.UnitPrice);

            Console.WriteLine("What is the total product price to buy one of each product?" 
                + totalCostForOneOfEach);

            //11. Return the name of all customers who placed at least one order before 1997

            List<string> customerNames = (from c in customers
                                            from o in c.Orders
                                            where o.OrderDate.Date.Year < 1997
                                            select c.CompanyName).Distinct().ToList();

            //12. Return the IDs for all orders over $1000

            List<int> ordersOver1k = (from c in customers
                                        from o in c.Orders
                                        where o.Total > 1000
                                        select o.OrderID).ToList();

            //13. Return the average order cost for all orders placed

            decimal averageOrderCost = (from c in customers
                                        from o in c.Orders
                                        select o.Total).Average();

            //14. Return the product category with the most products in the catalog

            var productCategoryCounts = (from prod in products
                group prod by prod.Category
                into prodGroup
                select new {Category = prodGroup.Key, ProductCount = prodGroup.Count()}
            ).ToList();

            int highestProductCatalogCount = productCategoryCounts.Max(pcc => pcc.ProductCount);
            string highestProductCatalogCountCategory = (from p in productCategoryCounts
                where p.ProductCount == highestProductCatalogCount
                                                         select p.Category).First();
            Console.WriteLine("The product with the most items in the catalog is: "
                + highestProductCatalogCountCategory);
            //ObjectDumper.Write(productCategoryCounts);


            //15. Return the list of products, ordered by product name

            List<Product> productOrderedByNameAsc = products.OrderBy(p => p.ProductName).ToList();
            List<Product> productOrderedByNameAsc2 = (from p in products orderby p.ProductName select p).ToList();
            Console.WriteLine("\n15. List of all products ordered by name:");
            samples.WriteProducts(productOrderedByNameAsc);


            //16. Return the list of customers, ordered by country

            List<Customer> customerByCountry = customers.OrderBy(c => c.Country).ToList();
            Console.WriteLine("\n16. List of customers, ordered by country:");
            samples.WriteCustomers(customerByCountry);


            //17. Return if there are any customers from Argentina

            bool fromArgentina = customers.Any(c => c.Country.Contains("Argentina"));

            Console.WriteLine("\n17. Are there any customers from Argentina? ");
            if (fromArgentina == true)
            {
                Console.WriteLine("    Yes, there are customers from Argentina");
            }
            else
            {
                Console.WriteLine("    No, there are no customers from Argentina");
            }


            //18. Return which year had a higher total sales, 1997 or 1996

            decimal totalSales1997 = (from o in orders
                                      where o.OrderDate.Date.Year == 1997
                                      select o).Sum(o => o.Total);

            decimal totalSales1998 = (from o in orders
                                      where o.OrderDate.Date.Year == 1998
                                      select o).Sum(o => o.Total);

            Console.WriteLine("\n18. Did 1997 or 1998 have a higher sales total?");

            if (totalSales1997 > totalSales1998)
            {
                Console.WriteLine("    1997 did with the following total sales: $" + totalSales1997);
            }
            else
            {
                Console.WriteLine("    1998 did with the following total sales: $" + totalSales1998);
            }


            //19. Return which year had a higher average order total, 1997 or 1998

            decimal averageSales1997 = (from o in orders
                                      where o.OrderDate.Date.Year == 1997
                                      select o).Average(o => o.Total);

            decimal averageSales1998 = (from o in orders
                                      where o.OrderDate.Date.Year == 1998
                                      select o).Average(o => o.Total);

            Console.WriteLine("\n19. Did 1997 or 1998 have a average order total?");

            if (averageSales1997 > averageSales1998)
            {
                Console.WriteLine("    1997 did with an average order total: $" + decimal.Round(averageSales1997, 2));
            }
            else
            {
                Console.WriteLine("    1998 did with anverage order total: $" + decimal.Round(averageSales1998, 2));
            }


            //20. Return whether Tofu is a product in the product catalog

            bool containsTofu = products.Any(p => p.ProductName.Contains("Tofu"));

            Console.WriteLine("\n20. Does the product catalog contain Tofu?");
            Console.WriteLine("    " + containsTofu);


            //21. Return the number of orders altogether for all years in the databas

            int totalNumOfOrders = (from o in orders
                                    select o).Count();
            Console.WriteLine("\n21. Total number of orders in database:");
            Console.WriteLine("    " + totalNumOfOrders);


            //22. Return how many customers are located in Paris

            int fromParis = (from c in customers
                           where c.City == "Paris"
                           select c).Count();


            Console.WriteLine("\n22. How many customers are located in Paris:");
            Console.WriteLine("    " + fromParis);


            //23. Return whether there are products that are out of stock

            bool productsOutOfStock = products.Any(p => p.UnitsInStock == 0);
            Console.WriteLine("\n23. Are there products that are out of stock?");

            if (productsOutOfStock == true)
            {
                Console.WriteLine("    Yes, there are items out of stock");
            }
            else
            {
                Console.WriteLine("    No, there are no items out of stock");
            }

            //Console.WriteLine("    " + productsOutOfStock);


            //24. Return the product with the highest amount of current stock

            Product highestCurrentStock = (from p in products
                                           orderby p.UnitsInStock, p.ProductName
                                           select p).Last();

            Console.WriteLine("\n24. The product with the most items in stock is:");
            Console.WriteLine("    " + highestCurrentStock.ProductName + " at " + highestCurrentStock.UnitsInStock);


            //25. Pick your own query - explore a new LINQ method and write a query with the data provided here
            //    List of all beverages in reverse alphabetical order

            List<Product> listOfBeverages = (from p in products
                                                       where p.Category == "Beverages"
                                                       orderby p.ProductName descending
                                                       select p).ToList();

            Console.WriteLine("\n25. List of beverages in reverse order by name:");
            samples.WriteProducts(listOfBeverages);

            // Catch the program before it finishes
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }


        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public string Category { get; set; }
            public int UnitsInStock { get; set; }
        }

        public class Order
        {
            public int OrderID { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal Total { get; set; }
        }

        public class Customer
        {
            public string CustomerID { get; set; }
            public string CompanyName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public Order[] Orders { get; set; }
        }

        public class LinqSamples
        {
            private List<Product> productList;
            private List<Customer> customerList;

            public void WriteProducts(List<Product> products)
            {
                foreach (Product pr in products)
                {
                    Console.WriteLine(pr.ProductName);
                }
            }

            public void WriteCustomers(List<Customer> customers)
            {
                foreach (Customer cst in customers)
                {
                    Console.WriteLine(cst.CompanyName);
                }
            }

            public void WriteOrders(List<Order> orders)
            {
                foreach (Order ord in orders)
                {
                    Console.WriteLine(ord.OrderID);
                }
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Count to get the number of unique prime factors of 300.")]
            public void Linq73()
            {
                int[] primeFactorsOf300 = { 2, 2, 3, 5, 5 };

                int uniqueFactors = primeFactorsOf300.Distinct().Count();

                Console.WriteLine("There are {0} unique prime factors of 300.", uniqueFactors);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Count to get the number of odd ints in the array.")]
            public void Linq74()
            {
                int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

                int oddNumbers = numbers.Count(n => n % 2 == 1);

                Console.WriteLine("There are {0} odd numbers in the list.", oddNumbers);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Count to return a list of customers and how many orders " +
                         "each has.")]
            public void Linq76()
            {
                List<Customer> customers = GetCustomerList();

                var orderCounts =
                    from cust in customers
                    select new { cust.CustomerID, OrderCount = cust.Orders.Count() };

                ObjectDumper.Write(orderCounts);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Count to return a list of categories and how many products " +
                         "each has.")]
            public void Linq77()
            {
                List<Product> products = GetProductList();

                var categoryCounts =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    select new { Category = prodGroup.Key, ProductCount = prodGroup.Count() };

                ObjectDumper.Write(categoryCounts);
            }

            //DONE Changed "get the total of" to "add all"
            [Category("Aggregate Operators")]
            [Description("This sample uses Sum to add all the numbers in an array.")]
            public void Linq78()
            {
                int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

                double numSum = numbers.Sum();

                Console.WriteLine("The sum of the numbers is {0}.", numSum);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Sum to get the total number of characters of all words " +
                         "in the array.")]
            public void Linq79()
            {
                string[] words = { "cherry", "apple", "blueberry" };

                double totalChars = words.Sum(w => w.Length);

                Console.WriteLine("There are a total of {0} characters in these words.", totalChars);
            }



            [Category("Aggregate Operators")]
            [Description("This sample uses Sum to get the total units in stock for each product category.")]
            public void Linq80()
            {
                List<Product> products = GetProductList();

                var categories =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    select new { Category = prodGroup.Key, TotalUnitsInStock = prodGroup.Sum(p => p.UnitsInStock) };

                ObjectDumper.Write(categories);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Min to get the lowest number in an array.")]
            public void Linq81()
            {
                int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

                int minNum = numbers.Min();

                Console.WriteLine("The minimum number is {0}.", minNum);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Min to get the length of the shortest word in an array.")]
            public void Linq82()
            {
                string[] words = { "cherry", "apple", "blueberry" };

                int shortestWord = words.Min(w => w.Length);

                Console.WriteLine("The shortest word is {0} characters long.", shortestWord);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Min to get the cheapest price among each category's products.")]
            public void Linq83()
            {
                List<Product> products = GetProductList();

                var categories =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    select new { Category = prodGroup.Key, CheapestPrice = prodGroup.Min(p => p.UnitPrice) };

                ObjectDumper.Write(categories);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Min to get the products with the lowest price in each category.")]
            public void Linq84()
            {
                List<Product> products = GetProductList();

                var categories =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    let minPrice = prodGroup.Min(p => p.UnitPrice)
                    select new { Category = prodGroup.Key, CheapestProducts = prodGroup.Where(p => p.UnitPrice == minPrice) };

                ObjectDumper.Write(categories, 1);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Max to get the highest number in an array. Note that the method returns a single value.")]
            public void Linq85()
            {
                int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

                int maxNum = numbers.Max();

                Console.WriteLine("The maximum number is {0}.", maxNum);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Max to get the length of the longest word in an array.")]
            public void Linq86()
            {
                string[] words = { "cherry", "apple", "blueberry" };

                int longestLength = words.Max(w => w.Length);

                Console.WriteLine("The longest word is {0} characters long.", longestLength);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Max to get the most expensive price among each category's products.")]
            public void Linq87()
            {
                List<Product> products = GetProductList();

                var categories =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    select new { Category = prodGroup.Key, MostExpensivePrice = prodGroup.Max(p => p.UnitPrice) };

                ObjectDumper.Write(categories);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Max to get the products with the most expensive price in each category.")]
            public void Linq88()
            {
                List<Product> products = GetProductList();

                var categories =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    let maxPrice = prodGroup.Max(p => p.UnitPrice)
                    select new { Category = prodGroup.Key, MostExpensiveProducts = prodGroup.Where(p => p.UnitPrice == maxPrice) };

                ObjectDumper.Write(categories, 1);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Average to get the average of all numbers in an array.")]
            public void Linq89()
            {
                int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

                double averageNum = numbers.Average();

                Console.WriteLine("The average number is {0}.", averageNum);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Average to get the average length of the words in the array.")]
            public void Linq90()
            {
                string[] words = { "cherry", "apple", "blueberry" };

                double averageLength = words.Average(w => w.Length);

                Console.WriteLine("The average word length is {0} characters.", averageLength);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Average to get the average price of each category's products.")]
            public void Linq91()
            {
                List<Product> products = GetProductList();

                var categories =
                    from prod in products
                    group prod by prod.Category into prodGroup
                    select new { Category = prodGroup.Key, AveragePrice = prodGroup.Average(p => p.UnitPrice) };

                ObjectDumper.Write(categories);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Aggregate to create a running product on the array that " +
                         "calculates the total product of all elements.")]
            public void Linq92()
            {
                double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

                double product = doubles.Aggregate((runningProduct, nextFactor) => runningProduct * nextFactor);

                Console.WriteLine("Total product of all numbers: {0}", product);
            }

            [Category("Aggregate Operators")]
            [Description("This sample uses Aggregate to create a running account balance that " +
                         "subtracts each withdrawal from the initial balance of 100, as long as " +
                         "the balance never drops below 0.")]
            public void Linq93()
            {
                double startBalance = 100.0;

                int[] attemptedWithdrawals = { 20, 10, 40, 50, 10, 70, 30 };

                double endBalance =
                    attemptedWithdrawals.Aggregate(startBalance,
                        (balance, nextWithdrawal) =>
                            ((nextWithdrawal <= balance) ? (balance - nextWithdrawal) : balance));

                Console.WriteLine("Ending balance: {0}", endBalance);
            }

            public List<Product> GetProductList()
            {
                if (productList == null)
                    createLists();

                return productList;
            }

            public List<Customer> GetCustomerList()
            {
                if (customerList == null)
                    createLists();

                return customerList;
            }

            private void createLists()
            {
                // Product data created in-memory using collection initializer:
                productList =
                    new List<Product> {
                    new Product { ProductID = 1, ProductName = "Chai", Category = "Beverages", UnitPrice = 18.0000M, UnitsInStock = 39 },
                    new Product { ProductID = 2, ProductName = "Chang", Category = "Beverages", UnitPrice = 19.0000M, UnitsInStock = 17 },
                    new Product { ProductID = 3, ProductName = "Aniseed Syrup", Category = "Condiments", UnitPrice = 10.0000M, UnitsInStock = 13 },
                    new Product { ProductID = 4, ProductName = "Chef Anton's Cajun Seasoning", Category = "Condiments", UnitPrice = 22.0000M, UnitsInStock = 53 },
                    new Product { ProductID = 5, ProductName = "Chef Anton's Gumbo Mix", Category = "Condiments", UnitPrice = 21.3500M, UnitsInStock = 0 },
                    new Product { ProductID = 6, ProductName = "Grandma's Boysenberry Spread", Category = "Condiments", UnitPrice = 25.0000M, UnitsInStock = 120 },
                    new Product { ProductID = 7, ProductName = "Uncle Bob's Organic Dried Pears", Category = "Produce", UnitPrice = 30.0000M, UnitsInStock = 15 },
                    new Product { ProductID = 8, ProductName = "Northwoods Cranberry Sauce", Category = "Condiments", UnitPrice = 40.0000M, UnitsInStock = 6 },
                    new Product { ProductID = 9, ProductName = "Mishi Kobe Niku", Category = "Meat/Poultry", UnitPrice = 97.0000M, UnitsInStock = 29 },
                    new Product { ProductID = 10, ProductName = "Ikura", Category = "Seafood", UnitPrice = 31.0000M, UnitsInStock = 31 },
                    new Product { ProductID = 11, ProductName = "Queso Cabrales", Category = "Dairy Products", UnitPrice = 21.0000M, UnitsInStock = 22 },
                    new Product { ProductID = 12, ProductName = "Queso Manchego La Pastora", Category = "Dairy Products", UnitPrice = 38.0000M, UnitsInStock = 86 },
                    new Product { ProductID = 13, ProductName = "Konbu", Category = "Seafood", UnitPrice = 6.0000M, UnitsInStock = 24 },
                    new Product { ProductID = 14, ProductName = "Tofu", Category = "Produce", UnitPrice = 23.2500M, UnitsInStock = 35 },
                    new Product { ProductID = 15, ProductName = "Genen Shouyu", Category = "Condiments", UnitPrice = 15.5000M, UnitsInStock = 39 },
                    new Product { ProductID = 16, ProductName = "Pavlova", Category = "Confections", UnitPrice = 17.4500M, UnitsInStock = 29 },
                    new Product { ProductID = 17, ProductName = "Alice Mutton", Category = "Meat/Poultry", UnitPrice = 39.0000M, UnitsInStock = 0 },
                    new Product { ProductID = 18, ProductName = "Carnarvon Tigers", Category = "Seafood", UnitPrice = 62.5000M, UnitsInStock = 42 },
                    new Product { ProductID = 19, ProductName = "Teatime Chocolate Biscuits", Category = "Confections", UnitPrice = 9.2000M, UnitsInStock = 25 },
                    new Product { ProductID = 20, ProductName = "Sir Rodney's Marmalade", Category = "Confections", UnitPrice = 81.0000M, UnitsInStock = 40 },
                    new Product { ProductID = 21, ProductName = "Sir Rodney's Scones", Category = "Confections", UnitPrice = 10.0000M, UnitsInStock = 3 },
                    new Product { ProductID = 22, ProductName = "Gustaf's Knäckebröd", Category = "Grains/Cereals", UnitPrice = 21.0000M, UnitsInStock = 104 },
                    new Product { ProductID = 23, ProductName = "Tunnbröd", Category = "Grains/Cereals", UnitPrice = 9.0000M, UnitsInStock = 61 },
                    new Product { ProductID = 24, ProductName = "Guaraná Fantástica", Category = "Beverages", UnitPrice = 4.5000M, UnitsInStock = 20 },
                    new Product { ProductID = 25, ProductName = "NuNuCa Nuß-Nougat-Creme", Category = "Confections", UnitPrice = 14.0000M, UnitsInStock = 76 },
                    new Product { ProductID = 26, ProductName = "Gumbär Gummibärchen", Category = "Confections", UnitPrice = 31.2300M, UnitsInStock = 15 },
                    new Product { ProductID = 27, ProductName = "Schoggi Schokolade", Category = "Confections", UnitPrice = 43.9000M, UnitsInStock = 49 },
                    new Product { ProductID = 28, ProductName = "Rössle Sauerkraut", Category = "Produce", UnitPrice = 45.6000M, UnitsInStock = 26 },
                    new Product { ProductID = 29, ProductName = "Thüringer Rostbratwurst", Category = "Meat/Poultry", UnitPrice = 123.7900M, UnitsInStock = 0 },
                    new Product { ProductID = 30, ProductName = "Nord-Ost Matjeshering", Category = "Seafood", UnitPrice = 25.8900M, UnitsInStock = 10 },
                    new Product { ProductID = 31, ProductName = "Gorgonzola Telino", Category = "Dairy Products", UnitPrice = 12.5000M, UnitsInStock = 0 },
                    new Product { ProductID = 32, ProductName = "Mascarpone Fabioli", Category = "Dairy Products", UnitPrice = 32.0000M, UnitsInStock = 9 },
                    new Product { ProductID = 33, ProductName = "Geitost", Category = "Dairy Products", UnitPrice = 2.5000M, UnitsInStock = 112 },
                    new Product { ProductID = 34, ProductName = "Sasquatch Ale", Category = "Beverages", UnitPrice = 14.0000M, UnitsInStock = 111 },
                    new Product { ProductID = 35, ProductName = "Steeleye Stout", Category = "Beverages", UnitPrice = 18.0000M, UnitsInStock = 20 },
                    new Product { ProductID = 36, ProductName = "Inlagd Sill", Category = "Seafood", UnitPrice = 19.0000M, UnitsInStock = 112 },
                    new Product { ProductID = 37, ProductName = "Gravad lax", Category = "Seafood", UnitPrice = 26.0000M, UnitsInStock = 11 },
                    new Product { ProductID = 38, ProductName = "Côte de Blaye", Category = "Beverages", UnitPrice = 263.5000M, UnitsInStock = 17 },
                    new Product { ProductID = 39, ProductName = "Chartreuse verte", Category = "Beverages", UnitPrice = 18.0000M, UnitsInStock = 69 },
                    new Product { ProductID = 40, ProductName = "Boston Crab Meat", Category = "Seafood", UnitPrice = 18.4000M, UnitsInStock = 123 },
                    new Product { ProductID = 41, ProductName = "Jack's New England Clam Chowder", Category = "Seafood", UnitPrice = 9.6500M, UnitsInStock = 85 },
                    new Product { ProductID = 42, ProductName = "Singaporean Hokkien Fried Mee", Category = "Grains/Cereals", UnitPrice = 14.0000M, UnitsInStock = 26 },
                    new Product { ProductID = 43, ProductName = "Ipoh Coffee", Category = "Beverages", UnitPrice = 46.0000M, UnitsInStock = 17 },
                    new Product { ProductID = 44, ProductName = "Gula Malacca", Category = "Condiments", UnitPrice = 19.4500M, UnitsInStock = 27 },
                    new Product { ProductID = 45, ProductName = "Rogede sild", Category = "Seafood", UnitPrice = 9.5000M, UnitsInStock = 5 },
                    new Product { ProductID = 46, ProductName = "Spegesild", Category = "Seafood", UnitPrice = 12.0000M, UnitsInStock = 95 },
                    new Product { ProductID = 47, ProductName = "Zaanse koeken", Category = "Confections", UnitPrice = 9.5000M, UnitsInStock = 36 },
                    new Product { ProductID = 48, ProductName = "Chocolade", Category = "Confections", UnitPrice = 12.7500M, UnitsInStock = 15 },
                    new Product { ProductID = 49, ProductName = "Maxilaku", Category = "Confections", UnitPrice = 20.0000M, UnitsInStock = 10 },
                    new Product { ProductID = 50, ProductName = "Valkoinen suklaa", Category = "Confections", UnitPrice = 16.2500M, UnitsInStock = 65 },
                    new Product { ProductID = 51, ProductName = "Manjimup Dried Apples", Category = "Produce", UnitPrice = 53.0000M, UnitsInStock = 20 },
                    new Product { ProductID = 52, ProductName = "Filo Mix", Category = "Grains/Cereals", UnitPrice = 7.0000M, UnitsInStock = 38 },
                    new Product { ProductID = 53, ProductName = "Perth Pasties", Category = "Meat/Poultry", UnitPrice = 32.8000M, UnitsInStock = 0 },
                    new Product { ProductID = 54, ProductName = "Tourtière", Category = "Meat/Poultry", UnitPrice = 7.4500M, UnitsInStock = 21 },
                    new Product { ProductID = 55, ProductName = "Pâté chinois", Category = "Meat/Poultry", UnitPrice = 24.0000M, UnitsInStock = 115 },
                    new Product { ProductID = 56, ProductName = "Gnocchi di nonna Alice", Category = "Grains/Cereals", UnitPrice = 38.0000M, UnitsInStock = 21 },
                    new Product { ProductID = 57, ProductName = "Ravioli Angelo", Category = "Grains/Cereals", UnitPrice = 19.5000M, UnitsInStock = 36 },
                    new Product { ProductID = 58, ProductName = "Escargots de Bourgogne", Category = "Seafood", UnitPrice = 13.2500M, UnitsInStock = 62 },
                    new Product { ProductID = 59, ProductName = "Raclette Courdavault", Category = "Dairy Products", UnitPrice = 55.0000M, UnitsInStock = 79 },
                    new Product { ProductID = 60, ProductName = "Camembert Pierrot", Category = "Dairy Products", UnitPrice = 34.0000M, UnitsInStock = 19 },
                    new Product { ProductID = 61, ProductName = "Sirop d'érable", Category = "Condiments", UnitPrice = 28.5000M, UnitsInStock = 113 },
                    new Product { ProductID = 62, ProductName = "Tarte au sucre", Category = "Confections", UnitPrice = 49.3000M, UnitsInStock = 17 },
                    new Product { ProductID = 63, ProductName = "Vegie-spread", Category = "Condiments", UnitPrice = 43.9000M, UnitsInStock = 24 },
                    new Product { ProductID = 64, ProductName = "Wimmers gute Semmelknödel", Category = "Grains/Cereals", UnitPrice = 33.2500M, UnitsInStock = 22 },
                    new Product { ProductID = 65, ProductName = "Louisiana Fiery Hot Pepper Sauce", Category = "Condiments", UnitPrice = 21.0500M, UnitsInStock = 76 },
                    new Product { ProductID = 66, ProductName = "Louisiana Hot Spiced Okra", Category = "Condiments", UnitPrice = 17.0000M, UnitsInStock = 4 },
                    new Product { ProductID = 67, ProductName = "Laughing Lumberjack Lager", Category = "Beverages", UnitPrice = 14.0000M, UnitsInStock = 52 },
                    new Product { ProductID = 68, ProductName = "Scottish Longbreads", Category = "Confections", UnitPrice = 12.5000M, UnitsInStock = 6 },
                    new Product { ProductID = 69, ProductName = "Gudbrandsdalsost", Category = "Dairy Products", UnitPrice = 36.0000M, UnitsInStock = 26 },
                    new Product { ProductID = 70, ProductName = "Outback Lager", Category = "Beverages", UnitPrice = 15.0000M, UnitsInStock = 15 },
                    new Product { ProductID = 71, ProductName = "Flotemysost", Category = "Dairy Products", UnitPrice = 21.5000M, UnitsInStock = 26 },
                    new Product { ProductID = 72, ProductName = "Mozzarella di Giovanni", Category = "Dairy Products", UnitPrice = 34.8000M, UnitsInStock = 14 },
                    new Product { ProductID = 73, ProductName = "Röd Kaviar", Category = "Seafood", UnitPrice = 15.0000M, UnitsInStock = 101 },
                    new Product { ProductID = 74, ProductName = "Longlife Tofu", Category = "Produce", UnitPrice = 10.0000M, UnitsInStock = 4 },
                    new Product { ProductID = 75, ProductName = "Rhönbräu Klosterbier", Category = "Beverages", UnitPrice = 7.7500M, UnitsInStock = 125 },
                    new Product { ProductID = 76, ProductName = "Lakkalikööri", Category = "Beverages", UnitPrice = 18.0000M, UnitsInStock = 57 },
                    new Product { ProductID = 77, ProductName = "Original Frankfurter grüne Soße", Category = "Condiments", UnitPrice = 13.0000M, UnitsInStock = 32 }
                };

                // Customer/Order data read into memory from XML file using XLinq:
                customerList = (
                    from e in XDocument.Load("Customers.xml").
                              Root.Elements("customer")
                    select new Customer
                    {
                        CustomerID = (string)e.Element("id"),
                        CompanyName = (string)e.Element("name"),
                        Address = (string)e.Element("address"),
                        City = (string)e.Element("city"),
                        Region = (string)e.Element("region"),
                        PostalCode = (string)e.Element("postalcode"),
                        Country = (string)e.Element("country"),
                        Phone = (string)e.Element("phone"),
                        Fax = (string)e.Element("fax"),
                        Orders = (
                            from o in e.Elements("orders").Elements("order")
                            select new Order
                            {
                                OrderID = (int)o.Element("id"),
                                OrderDate = (DateTime)o.Element("orderdate"),
                                Total = (decimal)o.Element("total")
                            })
                            .ToArray()
                    })
                    .ToList();
            }
        }
    }
}
