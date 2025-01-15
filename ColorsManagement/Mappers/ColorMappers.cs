using ColorsManagement.Dtos;
using ColorsManagement.Models;

namespace ColorsManagement.Mappers
{
    public static class ColorMappers
    {
        public static Color ToColorFromCreateDto(this OperationColorDto createColorDto)
        {
            return new Color
            {
                ColorName = createColorDto.ColorName,
                Price = createColorDto.Price,
                DisplayOrder = createColorDto.DisplayOrder,
                InStock = createColorDto.InStock,

            };
        }

        public static Color ToColorFromUpdateDto(this OperationColorDto updateColorDto,Guid colorId)
        {
            return new Color
            {
                Id = colorId,
                ColorName = updateColorDto.ColorName,
                Price = updateColorDto.Price,
                DisplayOrder = updateColorDto.DisplayOrder,
                InStock = updateColorDto.InStock,

            };
        }
    }
}

