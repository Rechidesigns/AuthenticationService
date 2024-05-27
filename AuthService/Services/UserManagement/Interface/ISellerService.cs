using AuthService.Data.UserDatas.DTOs;

namespace AuthService.Services.UserManagement.Interface
{
    public interface ISellerService
    {
        Task CreateSellerAccount(SellerDto model);
        Task UpdateSellerAccount(string userId, SellerDto model);
        Task<List<SellerDto>> GetSellerAccounts(string userId);
        Task<SellerDto> GetStoreById(string storeId);

    }
}
