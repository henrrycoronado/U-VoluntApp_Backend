namespace U_VoluntApp_Core.Src.Application.Interfaces;

using U_VoluntApp_Core.Src.Application.DTOs;

public interface IReferenceCatalogService
{
    Task<List<ReferenceStateDto>> GetStatesAsync(string stateGroup);

    Task<ReferenceStateDto> UpdateStateNameAsync(string stateGroup, string stateCode, string newName);

    Task<List<ReferenceTypeDto>> GetTypesAsync(string typeGroup);

    Task<ReferenceTypeDto> CreateTypeAsync(string typeGroup, CreateReferenceTypeDto dto);

    Task<ReferenceTypeDto> UpdateTypeAsync(string typeGroup, string typeCode, UpdateReferenceTypeDto dto);
}
