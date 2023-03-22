using AutoMapper;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class BuyerService : IBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Buyer> _buyerRepository;

        public BuyerService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Buyer> buyerRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _buyerRepository = buyerRepository;
        }

        public async Task<bool> ActionBuyer(Buyer buyer, string action)
        {
            Buyer getBuyer = new();
            bool check = false;

            switch (action)
            {
                case "AddBuyer":
                    getBuyer = await _buyerRepository.Get(buyer, "GetByID");

                    if (getBuyer == null)
                    {
                        check = await _buyerRepository.ActionEdit(buyer, "AddBuyer");
                    }

                    break;
            }

            return check;
        }

        public int Count(Buyer? buyer, string? action)
        {
            throw new NotImplementedException();
        }

        public Task<Buyer> GetBuyer(Buyer buyer, string action)
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
