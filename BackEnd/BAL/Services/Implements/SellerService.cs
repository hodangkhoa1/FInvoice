using AutoMapper;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Seller> _sellerRepository;

        public SellerService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Seller> sellerRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _sellerRepository = sellerRepository;
        }

        public async Task<bool> ActionSeller(Seller seller, string action)
        {
            Seller getSeller = new();
            bool check = false;

            switch (action)
            {
                case "AddSeller":
                    getSeller = await _sellerRepository.Get(seller, "GetByID");

                    if (getSeller == null)
                    {
                        check = await _sellerRepository.ActionEdit(seller, "AddSeller");
                    }

                    break;
            }

            return check;
        }

        public int Count(Seller? seller, string? action)
        {
            throw new NotImplementedException();
        }

        public Task<Seller> GetSeller(Seller seller, string action)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.CommitAsync();
        }
    }
}
