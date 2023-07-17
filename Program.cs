using System.Text;

namespace orderManagementApplication
{
    class Item
    {
        public int pid { get; init; }
        public string name { get; init; }
        public int qty { get; set; }
        public float uprice { get; init; }
        public float discount { get; init; }

    }

    class Customer {
        public int cid { get; set; }
        public string name { get; set; }
        public bool vip;
        public int oid { get; set; }

    }
    class Order
    {
        private int oid { get; set; }
        private readonly List<Item> items = new List<Item>();
        StringBuilder s = new StringBuilder();
        public void AddItem(Item item, int qty)
        {   
            item.qty = qty;
            items.Add(item);
            Console.WriteLine($"{item.qty} {item.name} has been added to the Order!");
        }

        public Item SearchItem(int pid, List<Item> InventoryList) { 
            foreach(Item item in InventoryList)
            {
                if (pid == item.pid)
                    return item;
            }
            return new Item { pid=0 };
        }

        public void Inventory_Shopping(List<Item> InventoryList)
        {
            Console.WriteLine("Welcome to Shoppers' Stop!");
            Console.WriteLine("--------------------------");
            Console.WriteLine("Here are the List of items we have!");
            Console.WriteLine("PID | Item Name | Unit Price | Discounts ");
            foreach (Item item in InventoryList) {
                Console.WriteLine($"{item.pid}   | {item.name}   | {item.uprice} | {item.discount}");
            }

            Console.WriteLine("Choose the items you need! - n for no items!");
            char c='y';
            int p, q;
            do
            {
                Console.WriteLine("Enter the Product Id of the item you want! ");
                p = Convert.ToInt32(Console.ReadLine());
                Item item = SearchItem(p, InventoryList);
                if (item.pid == 0) { Console.WriteLine("Product Not Found!"); continue; }
                Console.WriteLine($"Enter the Qty of the {item.name}");
                q = Convert.ToInt32(Console.ReadLine());
                AddItem(item, q);
                Console.WriteLine("y for more items, n for no items");
                c = Convert.ToChar(Console.ReadLine());
            } while (c != 'n');
        }

        public float totalBill(Customer c) {
            Console.WriteLine("Your Bill:");
            int i = 1;
            float sum = 0.0f;
            s.Append("Item No\t | Item Name\t | Unit Price\t | Qty\t | Total Price\t | After Discount\t\n");
            Console.WriteLine("Item No\t | Item Name\t | Unit Price\t | Qty\t | Total Price\t | After Discount\t");
            foreach (Item item in items) {
                float finalPrice = (item.uprice * item.qty) * ((100 - item.discount) / 100);
                sum += finalPrice;
                s.Append($"{i++}\t | {item.name}\t\t | {item.uprice}\t\t | {item.qty}\t | {item.uprice * item.qty}\t\t | {finalPrice}\t\n");
                Console.WriteLine($"{i++}\t | {item.name}\t\t | {item.uprice}\t\t | {item.qty}\t | {item.uprice * item.qty}\t\t | {finalPrice}\t");
            }
            Console.WriteLine($"You Owe {sum}$!");
            s.Append($"\nTotal Amount: {sum}\n");
            if (c.vip == true) { sum = sum * 0.9f; Console.WriteLine($"VIP Discount applicable: {sum}"); }
            return sum;
        }

        public void billPDF() {
            string path = @$"{Directory.GetCurrentDirectory()}\bill.txt";
            Console.WriteLine($"Generating & saving invoice as pdf at {path}!");
            File.WriteAllText(path, s.ToString());          
        }

        public void MakePayment(float finalAmt) {
            Console.WriteLine("Enter your banking details");
            string pseudoDetails = Convert.ToString(Console.ReadLine());
            Console.WriteLine($"Payment of {finalAmt} Successful!");
        }

        public void SendEmail()
        {
            Console.WriteLine("Enter your email Id:");
            string email = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Order Successful! A Copy of the bill has been sent to the email! Thank you for shopping with us!");


        }

        public Customer SearchCustomers(int cid, List<Customer> customers)
        {
            foreach(Customer customer in customers)
            {
                if (customer.cid == cid) { return customer; } 
            }
            return new Customer { cid = 0 };
        }

        public void PlaceOrder(Customer c, List<Item> inventoryList)
        {
            //Retrieve individual line item unit prices and discounts
            Inventory_Shopping(inventoryList);

            //Calculate total value of the order
            float totalAmt = totalBill(c);

            //Make payment 
            MakePayment(totalAmt);

            //Send email to customer
            SendEmail();

            //extended feature - bill PDF
            billPDF();
            
            //extended feature - VIP customers
            //searchCustomers.
        }

        public static void Main(string[] args)
        {
            Order or = new Order();
            or.oid = 1;
            List<Customer> customers = new List<Customer> { new Customer { cid = 101, name = "Rohan", vip = true, oid = or.oid } ,
                new Customer { cid = 102, name = "Ruth", vip = true, oid = or.oid } , new Customer { cid = 103, name = "Visakh", vip = false, oid = or.oid },
                new Customer { cid = 104, name = "Jerin", vip = true, oid = or.oid } 
            };
            Console.WriteLine("Enter your membership id!");
            int memid = Convert.ToInt32(Console.ReadLine());
            Customer c = or.SearchCustomers(memid, customers);
            Console.WriteLine($"Welcome {c.name}!");
            if (c.vip) Console.WriteLine("You have VIP Privileges");
            Item i1 = new Item { pid = 1, name="banana",qty = 1, uprice = 10, discount = 5 };
            Item i2 = new Item { pid = 2, name="milk",qty = 1, uprice = 40, discount = 6 };
            Item i3 = new Item { pid = 3, name="rice", qty = 1, uprice = 55, discount = 4 };
            Item i4 = new Item { pid = 4, name="eggs",qty = 1, uprice = 100, discount = 9 };
            Item i5 = new Item { pid = 5, name = "bread", qty = 1, uprice = 50, discount = 3 };
            List<Item> InventoryList = new List<Item> {i1,i2,i3,i4,i5 };
            or.PlaceOrder(c,InventoryList);

        }

    } //order end
}//namespace end
