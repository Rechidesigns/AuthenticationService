using AuthService.Data.UserDatas.DTOs;
using AuthService.Services.UserManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers.SellerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateSellerAccount(SellerDto model)
        {
            try
            {
                await _sellerService.CreateSellerAccount(model);
                return Ok("Seller account created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPut("update/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateSellerAccount(string userId, SellerDto model)
        {
            try
            {
                await _sellerService.UpdateSellerAccount(userId, model);
                return Ok("Seller account updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetSellerAccounts(string userId)
        {
            try
            {
                var sellerAccounts = await _sellerService.GetSellerAccounts(userId);
                if (sellerAccounts == null || !sellerAccounts.Any())
                {
                    return NotFound("Seller accounts not found.");
                }
                return Ok(sellerAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("store/{storeId}")]
        [Authorize]
        public async Task<IActionResult> GetStoreById(string storeId)
        {
            try
            {
                var store = await _sellerService.GetStoreById(storeId);
                if (store == null)
                {
                    return NotFound("Store not found.");
                }
                return Ok(store);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

    }
}
