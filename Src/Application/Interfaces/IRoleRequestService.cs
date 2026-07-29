namespace U_VoluntApp_Core.Src.Application.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using U_VoluntApp_Core.Src.Application.DTOs;

public interface IRoleRequestService
{
    Task<RoleRequestResponseDto> RequestCoordinatorAsync(CreateRoleRequestDto dto, string requesterProfileCode);

    Task<RoleRequestResponseDto> RequestAdminAsync(CreateRoleRequestDto dto, string requesterProfileCode);

    Task<IEnumerable<RoleRequestResponseDto>> GetPendingCoordinatorRequestsAsync();

    Task<IEnumerable<RoleRequestResponseDto>> GetPendingAdminRequestsAsync();

    Task ApproveCoordinatorAsync(string uvaCode, string adminProfileCode);

    Task RejectCoordinatorAsync(string uvaCode, string adminProfileCode);

    Task ApproveAdminAsync(string uvaCode, string suProfileCode);

    Task RejectAdminAsync(string uvaCode, string suProfileCode);
}
