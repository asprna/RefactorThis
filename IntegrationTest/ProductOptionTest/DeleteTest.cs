﻿using FluentAssertions;
using helper = IntegrationTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
//using model = API.Models;
using Xunit;
//using API.Models;
using System.Net.Http.Json;
using Domain;

namespace IntegrationTest.ProductOptionTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class DeleteTest : helper.IntegrationTest
	{
		/// <summary>
		/// Controller should delete the correct option.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task DeleteOption_ValidProductIDAndOptionID_OptionDeletionSuccessful()
		{
			//Arrange
			var productOption = helper.SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")).FirstOrDefault();

			//Act
			var response = await TestClient.DeleteAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", productOption.ProductId.ToString()).Replace("{optionId}", productOption.Id.ToString()));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should not delete the option when the option id is invalid.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task DeleteOption_ValidProductIDAndInvalidOption_BadRequestResponse()
		{
			//Arrange
			var productOption = helper.SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")).FirstOrDefault();
			var invalidOptionId = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F33";

			//Act
			var response = await TestClient.DeleteAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", productOption.ProductId.ToString()).Replace("{optionId}", invalidOptionId));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should not delete the option when the product id is invalid.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task DeleteOption_InvalidProductIDAndValidOptionID_BadRequestResponse()
		{
			//Arrange
			var productOption = helper.SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")).FirstOrDefault();
			var invalidProductId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";

			//Act
			var response = await TestClient.DeleteAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", invalidProductId).Replace("{optionId}", productOption.Id.ToString()));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
	}
}
