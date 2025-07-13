using AutoMapper;
using InventoryTracker.Dtos;
using InventoryTracker.Models;
using InventoryTracker.Repositories;

namespace InventoryTracker.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IComputerRepository _repository;
        private readonly IMapper _mapper;

        public ComputerService(IComputerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ComputerReadDto>> GetAllAsync()
        {
            var computers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ComputerReadDto>>(computers);
        }

        public async Task<ComputerReadDto?> GetByIdAsync(int id)
        {
            var computer = await _repository.GetByIdAsync(id);
            return computer is null ? null : _mapper.Map<ComputerReadDto>(computer);
        }

        public async Task<ComputerReadDto> CreateAsync(ComputerCreateDto dto)
        {
            var computer = _mapper.Map<Computer>(dto);
            await _repository.AddAsync(computer);
            await _repository.SaveChangesAsync();
            return _mapper.Map<ComputerReadDto>(computer);
        }

        public async Task<bool> UpdateAsync(int id, ComputerUpdateDto dto)
        {
            var computer = await _repository.GetByIdAsync(id);
            if (computer is null)
                return false;

            _mapper.Map(dto, computer);
            await _repository.UpdateAsync(computer);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var computer = await _repository.GetByIdAsync(id);
            if (computer is null)
                return false;

            await _repository.DeleteAsync(computer);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
