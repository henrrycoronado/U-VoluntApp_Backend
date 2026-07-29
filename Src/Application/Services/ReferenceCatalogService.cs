namespace U_VoluntApp_Core.Src.Application.Services;

using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Enums;

public class ReferenceCatalogService : IReferenceCatalogService
{
    public Task<List<ReferenceStateDto>> GetStatesAsync(string stateGroup)
    {
        stateGroup = NormalizeGroup(stateGroup);

        var result = stateGroup switch
        {
            "activity" => GetEnumValues<ActivityState>(),
            "program" => GetEnumValues<ProgramState>(),
            "profile" => GetEnumValues<ProfileState>(),
            "enrollment" => GetEnumValues<EnrollmentState>(),
            "tracking" => GetEnumValues<TrackingState>(),
            "contract" => GetEnumValues<ContractState>(),
            "role-request" => GetEnumValues<RoleRequestState>(),
            _ => throw new InvalidOperationException("Grupo de states no valido"),
        };

        return Task.FromResult(result);
    }

    public Task<ReferenceStateDto> UpdateStateNameAsync(string stateGroup, string stateCode, string newName)
    {
        throw new NotSupportedException("Los estados están definidos en código y no pueden ser modificados dinámicamente.");
    }

    public Task<List<ReferenceTypeDto>> GetTypesAsync(string typeGroup)
    {
        typeGroup = NormalizeGroup(typeGroup);

        var result = typeGroup switch
        {
            "activity" => GetTypeEnumValues<ActivityType>(),
            "evidence" => GetTypeEnumValues<EvidenceType>(),
            "tracking" => GetTypeEnumValues<TrackingType>(),
            "career" => GetTypeEnumValues<CareerType>(),
            "scholarship" => GetTypeEnumValues<ScholarshipType>(),
            _ => throw new InvalidOperationException("Grupo de types no valido"),
        };

        return Task.FromResult(result);
    }

    public Task<ReferenceTypeDto> CreateTypeAsync(string typeGroup, CreateReferenceTypeDto dto)
    {
        throw new NotSupportedException("Los tipos están definidos en código y no pueden ser creados dinámicamente.");
    }

    public Task<ReferenceTypeDto> UpdateTypeAsync(string typeGroup, string typeCode, UpdateReferenceTypeDto dto)
    {
        throw new NotSupportedException("Los tipos están definidos en código y no pueden ser modificados dinámicamente.");
    }

    private string NormalizeGroup(string group)
    {
        return group.Trim().ToLowerInvariant();
    }

    private List<ReferenceStateDto> GetEnumValues<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => new ReferenceStateDto
            {
                UvaCode = e.GetUvaCode(),
                Name = e.GetUvaName()
            })
            .OrderBy(x => x.Name)
            .ToList();
    }

    private List<ReferenceTypeDto> GetTypeEnumValues<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => new ReferenceTypeDto
            {
                UvaCode = e.GetUvaCode(),
                Name = e.GetUvaName(),
                IsActive = true
            })
            .OrderBy(x => x.Name)
            .ToList();
    }
}
