using ColorsManagement.Common;
using ColorsManagement.Dtos;
using ColorsManagement.Interfaces.Repositories;
using ColorsManagement.Interfaces.Services;
using ColorsManagement.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace ColorsManagement.Implementations.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }
        public async Task<ServiceResponse<object>> AddColorAsync(OperationColorDto createColorDto)
        {
            try
            {
                var newColor = createColorDto.ToColorFromCreateDto();
                await _colorRepository.AddColorAsync(newColor);
                return new ServiceResponse<object>( StatusCodes.Status200OK, "Color was created successfully" );


            }
            catch (SqlException sqlEx)
            {
                return new ServiceResponse<object>(StatusCodes.Status500InternalServerError, sqlEx.Message);
              
            }
        }

        public async Task<ServiceResponse<object>> GetAllAsync()
        {
            try
            {
                
                var results = await _colorRepository.GetAllAsync();
                
                if (results.Count == 0) return new ServiceResponse<object>(StatusCodes.Status404NotFound, "There are no colors in the Db!");
                return new ServiceResponse<object>(StatusCodes.Status200OK, results);


            }
            catch (SqlException sqlEx)
            {
                return new ServiceResponse<object>(StatusCodes.Status500InternalServerError, sqlEx.Message);

            }
        }

        public async Task<ServiceResponse<object>> RemoveColorByIdAsync(Guid colorId)
        {
            try
            {
               
                var colorExists = await _colorRepository.IsColorExistByIdAsync(colorId);

                if (!colorExists) return new ServiceResponse<object>(StatusCodes.Status404NotFound, "Color with the given Id was not found.");


                await _colorRepository.RemoveColorByIdAsync(colorId);
                
                
                return new ServiceResponse<object>(StatusCodes.Status200OK, "Color was deleted successfully");


            }
            catch (SqlException sqlEx)
            {
                return new ServiceResponse<object>(StatusCodes.Status500InternalServerError, sqlEx.Message);

            }
        }

        public async Task<ServiceResponse<object>> UpdateColorAsync(Guid colorId, OperationColorDto updateColorDto)
        {
            try
            {

                var colorExists = await _colorRepository.IsColorExistByIdAsync(colorId);
                if (!colorExists) return new ServiceResponse<object>(StatusCodes.Status404NotFound, "Color with the given Id was not found.");
                var colorToUpdate = updateColorDto.ToColorFromUpdateDto(colorId);

                await _colorRepository.UpdateColorAsync(colorToUpdate);
               

                return new ServiceResponse<object>(StatusCodes.Status200OK, "Color was updated successfully");


            }
            catch (SqlException sqlEx)
            {
                return new ServiceResponse<object>(StatusCodes.Status500InternalServerError, sqlEx.Message);

            }
        }

        public async Task<ServiceResponse<object>> UpdateColorOrderAsync(UpdateDisplayOrderListDto updateDisplayOrderListDto)
        {
            try
            {
                var sortedItems = updateDisplayOrderListDto.Items.OrderBy(item => item.DisplayOrder).ToList();



                await _colorRepository.UpdateDisplayOrdersAsync(sortedItems);


                return new ServiceResponse<object>(StatusCodes.Status200OK, "Colors order was updated successfully");


            }
            catch (SqlException sqlEx)
            {
                return new ServiceResponse<object>(StatusCodes.Status500InternalServerError, sqlEx.Message);

            }
        }
    }
}
