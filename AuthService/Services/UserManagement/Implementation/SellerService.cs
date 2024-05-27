using AuthService.Data.UserDatas.DTOs;
using AuthService.Data.UserDatas.Model;
using AuthService.Data;
using AuthService.Services.UserManagement.Interface;
using Microsoft.EntityFrameworkCore;




namespace AuthService.Services.UserManagement.Implementation
{
    public class SellerService : ISellerService
    {
        private readonly AppDbContext _context;

        public SellerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateSellerAccount(SellerDto model)
        {
            // Logic to create a seller account
            var seller = new Seller
            {
                Business_name = model.Business_name,
                Business_address = model.Business_address,
                phonenumber = model.phonenumber,
                bank_acc_num = model.bank_acc_num,
                bank_name = model.bank_name,
                reg_num = model.reg_num,
                UserId = model.UserId
            };

            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSellerAccount(string userId, SellerDto model)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);
            if (seller == null)
            {
                throw new InvalidOperationException("Seller not found.");
            }

            // Update seller information
            seller.Business_name = model.Business_name;
            seller.Business_address = model.Business_address;
            seller.phonenumber = model.phonenumber;
            seller.bank_acc_num = model.bank_acc_num;
            seller.bank_name = model.bank_name;
            seller.reg_num = model.reg_num;

            _context.Sellers.Update(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SellerDto>> GetSellerAccounts(string userId)
        {
            var sellerAccounts = await _context.Sellers
                .Where(s => s.UserId == userId)
                .ToListAsync();

            if (sellerAccounts == null || !sellerAccounts.Any())
            {
                return null;
                throw new InvalidOperationException("Seller accounts not found.");

            }

            var sellerDtos = sellerAccounts.Select(sellerAccount => new SellerDto
            {
                Business_name = sellerAccount.Business_name,
                Business_address = sellerAccount.Business_address,
                phonenumber = sellerAccount.phonenumber,
                bank_acc_num = sellerAccount.bank_acc_num,
                bank_name = sellerAccount.bank_name,
                reg_num = sellerAccount.reg_num,
                UserId = sellerAccount.UserId
            }).ToList();

            return sellerDtos;
        }

        public async Task<SellerDto> GetStoreById(string storeId)
        {
            var store = await _context.Sellers.FindAsync(storeId);

            if (store == null)
            {
                return null;
                throw new InvalidOperationException("Seller Account not found.");

            }

            var storeDto = new SellerDto
            {
                Business_name = store.Business_name,
                Business_address = store.Business_address,
                phonenumber = store.phonenumber,
                bank_acc_num = store.bank_acc_num,
                bank_name = store.bank_name,
                reg_num = store.reg_num,
                UserId = store.UserId
            };

            return storeDto;
        }

    }
}
