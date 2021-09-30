using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.IntegrationTest.Helper
{
	public class ApiRoutes
	{
        private static readonly string _baseUrl = "https://localhost:5000/api/";

        public static class Products
        {
            private static readonly string _productsControllerUrl = string.Concat(_baseUrl, "products");

            public static readonly string Get = _productsControllerUrl;

            //public static readonly string Get = string.Concat(_usersControllerUrl, "/{userId}");

            //public static readonly string Delete = string.Concat(_usersControllerUrl, "/{userId}");
        }
    }
}
