using ColorsManagement.Common;
using ColorsManagement.Dtos;

namespace ColorsManagement.Interfaces.Services
{
    public interface IColorService
    {
        public Task<ServiceResponse<object>> AddColorAsync(OperationColorDto createColorDto);
        public Task<ServiceResponse<object>> RemoveColorByIdAsync(Guid colorId);
        public Task<ServiceResponse<object>> GetAllAsync();
        public Task<ServiceResponse<object>> UpdateColorAsync(Guid colorId,OperationColorDto updateColorDto);
        public Task<ServiceResponse<object>> UpdateColorOrderAsync(UpdateDisplayOrderListDto updateDisplayOrderListDto);




    }
}
