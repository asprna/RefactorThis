using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.IntegrationTest.Helper
{
    /// <summary>
    /// Maintains REST API URLs. 
    /// </summary>
	public class ApiRoutes
	{
        /// <summary>
        /// Base URL.
        /// </summary>
        private static readonly string _baseUrl = "http://localhost:5000/api/";

        public static class Products
        {
            /// <summary>
            /// Base URL for products.
            /// </summary>
            private static readonly string _productsControllerUrl = string.Concat(_baseUrl, "products");

            /// <summary>
            /// Base URL for product option.
            /// </summary>
            private static readonly string _productsOptionControllerUrl = string.Concat(_baseUrl, "products/{id}/options");

            public static readonly string Get = _productsControllerUrl;

            public static readonly string GetProductByName = string.Concat(_productsControllerUrl, "?name={name}");

            public static readonly string ProducIdUrl = string.Concat(_productsControllerUrl, "/{id}");

            public static readonly string Post = _productsControllerUrl;

            public static readonly string GetOption = _productsOptionControllerUrl;

            public static readonly string GetOptionId = string.Concat(_productsOptionControllerUrl, "/{optionId}");
        }
    }
}
