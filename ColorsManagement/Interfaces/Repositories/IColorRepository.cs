using ColorsManagement.Dtos;
using ColorsManagement.Models;

namespace ColorsManagement.Interfaces.Repositories
{
    public interface IColorRepository
    {
        public Task<List<Color>> GetAllAsync();
        public Task AddColorAsync(Color color);
        public Task RemoveColorByIdAsync(Guid colorId);
        public Task UpdateColorAsync(Color color);
        public Task<bool> IsColorExistByIdAsync(Guid colorId);
        public Task UpdateDisplayOrdersAsync(List<UpdateDisplayOrderDto> listDto);

    }
}
