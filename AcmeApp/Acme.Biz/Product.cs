using Acme.Common;
using System;
using static Acme.Common.LoggingService;

namespace Acme.Biz
{

    /// <summary>
    /// Manage products carried in inventory
    /// </summary>
    public class Product
    {
        public const double InchesPerMeter = 39.37;
        public readonly decimal MinimumPrice;
        #region Constructors
        public Product()
        {
            //this.ProductVendor = new Vendor();
            Console.WriteLine("Product instance created");
            this.MinimumPrice = .96m;
            this.Category = "Tools";
        }

        public Product(int productId, string productName, string description) : this()
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.Description = description;
            if (ProductName.StartsWith("Bulk"))
            {
                this.MinimumPrice = 9.99m;
            }

            Console.WriteLine("Product instance has a name " + ProductName);
        }
        #endregion

        #region Properties

        private DateTime? availabilityDate;

	    public DateTime? AvailabilityDate	
        {
		    get { return availabilityDate;}
		    set { availabilityDate = value;}
	    }


        private string productName;

        public string ProductName
        {
            get {
                var formattedValue = productName?.Trim();
                return formattedValue;
            }
            set {

                if (value.Length < 3)
                {
                    ValidationMessage = "Product Name must be at least 3 characters";
                }
                else if (value.Length > 20)
                {
                    ValidationMessage = "Product Name cannot be more than 20 characters";
                }
                else
                    productName = value;
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int productId;

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        private Vendor productVendor;

        public Vendor ProductVendor
        {
            get {
                if (productVendor == null)
                {
                    productVendor = new Vendor();
                }
                return productVendor;
            }
            set { productVendor = value; }
        }

        public string ValidationMessage { get; private set; }
        internal string Category { get;  set; }
        public int SequenceNumber { get; set; } = 1;

        //string interpolation
        //public string ProductCode => $"{this.Category} - {this.SequenceNumber:0000}";

        public string ProductCode => this.Category + "-" + this.SequenceNumber;
        public decimal Cost { get; set; }

        #endregion
        /// <summary>
        /// Calculates the suggested retail price
        /// </summary>
        /// <param name="markupPercent">Percent used to mark up the cost</param>
        /// <returns></returns>
        public decimal CalculateSuggestedPrice (decimal markupPercent) =>
             this.Cost + (this.Cost * markupPercent / 100);
        

        public string SayHello()
        {
            //var vendor = new Vendor();
            //vendor.SendWelcomeEmail("Message from product");

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Product", this.productName, "check@gmail.com" );

            var result = LogAction("saying hello");

            return "Hello " + ProductName +
                " (" + ProductId + "): " +
                Description + 
                " Available on: " +
                //If not null, use datestring
                AvailabilityDate?.ToShortDateString();
        }

        public override string ToString() =>
             this.ProductName + " (" + this.ProductId + ")";
        


    }
}
