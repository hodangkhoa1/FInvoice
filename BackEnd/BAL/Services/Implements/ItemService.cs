using AutoMapper;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Item> _itemRepository;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Item> itemRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<bool> ActionItem(Item item, string action)
        {
            Item getItem = new();
            bool check = false;

            switch (action)
            {
                case "AddItem":
                    getItem = await _itemRepository.Get(item, "GetByID");

                    if (getItem == null)
                    {
                        check = await _itemRepository.ActionEdit(item, "AddItem");
                    }

                    break;
            }

            return check;
        }

        public int Count(Item? item, string? action)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetItem(Item item, string action)
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
