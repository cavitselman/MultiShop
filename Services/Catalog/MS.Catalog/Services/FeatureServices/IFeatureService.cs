﻿using MS.Catalog.Dtos.FeatureDtos;

namespace MS.Catalog.Services.FeatureServices
{
    public interface IFeatureService
    {
        Task<List<ResultFeatureDto>> GetAllFeatureAsync();
        Task CreateFeatureAsync(CreateFeatureDto createFeatureDto);
        Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto);
        Task DeleteFeatureAsync(string id);
        Task<GetByIdFeatureDto> GetByIdFeatureAsync(string id);
    }
}
