﻿using Acme.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Biz
{
    /// <summary>
    /// Manages the vendors from whom we purchase our inventory.
    /// </summary>
    public class Vendor 
    {
        public enum IncludeAddress { Yes, No };
        public enum SendCopy { Yes, No};

        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Sends an email to welcome a new vendor.
        /// </summary>
        /// <returns></returns>
        public string SendWelcomeEmail(string message)
        {
            var emailService = new EmailService();
            var subject = ("Hello " + this.CompanyName).Trim();
            var confirmation = emailService.SendMessage(subject,
                                                        message, 
                                                        this.Email);
            return confirmation;
        }

        
        /// <summary>
        /// Sends a product order to the vendor
        /// </summary>
        /// <param name="product">Product to order</param>
        /// <param name="quantity">Quantity of the product to order</param>
        /// <param name="deliverBy">Requested delivery date</param>
        /// <param name="instructions">Delivery instructions</param>
        /// <returns></returns>
        public OperationResult PlaceOrder(Product product, int quantity,
            DateTimeOffset? deliverBy = null, string instructions =  "standard delivery")
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            if (deliverBy <= DateTimeOffset.Now)
                throw new ArgumentOutOfRangeException(nameof(deliverBy));


            var success = false;

            var orderText = "Order from Acme, Inc" + System.Environment.NewLine +
                "Product: " + product.ProductCode +
                System.Environment.NewLine +
                "Quantity: " + quantity;

            if (deliverBy.HasValue)
            {
                orderText += System.Environment.NewLine +
                    "Deliver By: " + deliverBy.Value.ToString("d");
            }

            if (!string.IsNullOrWhiteSpace(instructions))
            {
                orderText += System.Environment.NewLine +
                    "Instructions: " + instructions;
            }

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Order", orderText, this.Email);

            if (confirmation.StartsWith("Message sent: "))
            {
                success = true;
            }

            var operationResult = new OperationResult(success, orderText);
            return operationResult;
        }
        /// <summary>
        /// Send a product order to the vendor
        /// </summary>
        /// <param name="product">Product to order</param>
        /// <param name="quantity">Quantity of the product to order</param>
        /// <param name="includeAddress">True to include shippeng address</param>
        /// <param name="sendCopy">True to send a copy of the email to the current user</param>
        /// <returns></returns>
        public OperationResult PlaceOrder(Product product, int quantity,
            IncludeAddress includeAddress, SendCopy sendCopy)
        {
            var orderText = "Test";
            if (includeAddress == IncludeAddress.Yes) orderText += " With Address";
            if (sendCopy == SendCopy.Yes) orderText += " With Copy";

            var operationResult = new OperationResult(true, orderText);
            return operationResult;
        }

        public override string ToString()
        {
            string vendorInfo = "Vendor: " + this.CompanyName;
            return vendorInfo;
        }

        public string PrepareDirections()
        {
            //verbatim string literal
            var directions = @"Insert \r\n to define a new line";
            return directions;
        }


    }
}
