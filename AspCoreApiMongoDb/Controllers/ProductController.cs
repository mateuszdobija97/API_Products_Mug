﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspCoreApiMongoDb.DataLayer.Abstracts;
using AspCoreApiMongoDb.Models;

namespace AspCoreApiMongoDb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route("api/product")]
        public Task<IEnumerable<Product>> Get()
        {
            return _productService.GetAllProducts();
        }

        [HttpGet]
        [Route("api/product/getByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var product = await _productService.GetProduct(name);
                if (product == null)
                {
                    return Json("No product found!");
                }
                return Json(product);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());

            }

        }

        [HttpPost]
        [Route("api/product")]
        public async Task<IActionResult> Post(Product model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return BadRequest("Please enter product name");
                else if (model.Price <= 0)
                    return BadRequest("Please enter price");

                
                await _productService.AddProduct(model);
                return Ok("Your product has been added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        [Route("api/product/updatePrice")]
        public async Task<IActionResult> UpdatePrice(Product model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest("Product name missing");
            
            var result = await _productService.UpdatePrice(model);
            if (result)
            {
                return Ok("Your product's price has been updated successfully");
            }
            return BadRequest("No product found to update");

        }

        [HttpDelete]
        [Route("api/product")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Product name missing");
                await _productService.RemoveProduct(name);
                return Ok("Your product has been deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpDelete]
        [Route("api/product/deleteAll")]
        public IActionResult DeleteAll()
        {
            try
            {
                _productService.RemoveAllProducts();
                return Ok("Your all products has been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}