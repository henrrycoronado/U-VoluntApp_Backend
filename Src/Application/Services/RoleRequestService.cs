namespace U_VoluntApp_Backend.Src.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Contract;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;

public class RoleRequestService : IRoleRequestService
{
    private readonly IRoleRequestRepository _roleRequestRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public RoleRequestService(
        IRoleRequestRepository roleRequestRepository,
        IProfileRepository profileRepository,
        UserManager<IdentityUser> userManager)
    {
        _roleRequestRepository = roleRequestRepository;
        _profileRepository = profileRepository;
        _userManager = userManager;
    }

    public async Task<RoleRequestResponseDto> RequestCoordinatorAsync(CreateRoleRequestDto dto, string requesterProfileCode)
    {
        var profile = await _profileRepository.GetByCodeAsync(requesterProfileCode);
        if (profile == null || profile.Email != dto.Email)
        {
            throw new InvalidOperationException("El correo enviado no coincide con el solicitante logueado.");
        }

        if (!dto.DurationInMonths.HasValue || dto.DurationInMonths.Value <= 0)
        {
            throw new InvalidOperationException("Para ser Coordinador debes especificar una duración en meses válida.");
        }

        var roleRequest = RoleRequest.Create(
            requesterProfileCode,
            "Coordinator",
            dto.Reason,
            dto.DurationInMonths.Value,
            RoleRequestState.Pending.GetUvaCode(),
            DateTime.UtcNow);

        await _roleRequestRepository.AddAsync(roleRequest);

        return MapToDto(roleRequest);
    }

    public async Task<RoleRequestResponseDto> RequestAdminAsync(CreateRoleRequestDto dto, string requesterProfileCode)
    {
        var profile = await _profileRepository.GetByCodeAsync(requesterProfileCode);
        if (profile == null || profile.Email != dto.Email)
        {
            throw new InvalidOperationException("El correo enviado no coincide con el solicitante logueado.");
        }

        var identityUser = await _userManager.FindByIdAsync(profile.IdentityUserId);
        if (identityUser == null)
        {
            throw new KeyNotFoundException($"Usuario {profile.IdentityUserId} no encontrado");
        }

        if (!await _userManager.IsInRoleAsync(identityUser, "Coordinator"))
        {
            throw new UnauthorizedAccessException("Solo los Coordinadores pueden enviar solicitud para ser Administrador.");
        }

        var roleRequest = RoleRequest.Create(
            requesterProfileCode,
            "Admin",
            dto.Reason,
            null,
            RoleRequestState.Pending.GetUvaCode(),
            DateTime.UtcNow);

        await _roleRequestRepository.AddAsync(roleRequest);

        return MapToDto(roleRequest);
    }

    public async Task<IEnumerable<RoleRequestResponseDto>> GetPendingCoordinatorRequestsAsync()
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100, StateName = RoleRequestState.Pending.GetUvaCode() };
        var requests = await _roleRequestRepository.GetAllAsync(filter);
        var coordinatorRequests = requests.Where(r => r.RequestedRoleCode == "Coordinator");
        return coordinatorRequests.Select(MapToDto);
    }

    public async Task<IEnumerable<RoleRequestResponseDto>> GetPendingAdminRequestsAsync()
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100, StateName = RoleRequestState.Pending.GetUvaCode() };
        var requests = await _roleRequestRepository.GetAllAsync(filter);
        var adminRequests = requests.Where(r => r.RequestedRoleCode == "Admin");
        return adminRequests.Select(MapToDto);
    }

    public async Task ApproveCoordinatorAsync(string uvaCode, string adminProfileCode)
    {
        var request = await _roleRequestRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException($"Solicitud de rol {uvaCode} no encontrada");

        if (request.RequestedRoleCode != "Coordinator")
        {
            throw new InvalidOperationException("Esta solicitud no es para Coordinador.");
        }

        request.Approve(adminProfileCode, RoleRequestState.Active.GetUvaCode(), DateTime.UtcNow);
        await _roleRequestRepository.UpdateAsync(request);

        var profile = await _profileRepository.GetByCodeAsync(request.RequesterProfileCode);
        if (profile != null)
        {
            var user = await _userManager.FindByIdAsync(profile.IdentityUserId);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Coordinator"))
            {
                await _userManager.AddToRoleAsync(user, "Coordinator");
            }
        }
    }

    public async Task RejectCoordinatorAsync(string uvaCode, string adminProfileCode)
    {
        var request = await _roleRequestRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException($"Solicitud de rol {uvaCode} no encontrada");

        if (request.RequestedRoleCode != "Coordinator")
        {
            throw new InvalidOperationException("Esta solicitud no es para Coordinador.");
        }

        request.Reject(adminProfileCode, RoleRequestState.Rejected.GetUvaCode(), DateTime.UtcNow);
        await _roleRequestRepository.UpdateAsync(request);
    }

    public async Task ApproveAdminAsync(string uvaCode, string suProfileCode)
    {
        var request = await _roleRequestRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException($"Solicitud de rol {uvaCode} no encontrada");

        if (request.RequestedRoleCode != "Admin")
        {
            throw new InvalidOperationException("Esta solicitud no es para Admin.");
        }

        request.Approve(suProfileCode, RoleRequestState.Active.GetUvaCode(), DateTime.UtcNow);
        await _roleRequestRepository.UpdateAsync(request);

        var profile = await _profileRepository.GetByCodeAsync(request.RequesterProfileCode);
        if (profile != null)
        {
            var user = await _userManager.FindByIdAsync(profile.IdentityUserId);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }

    public async Task RejectAdminAsync(string uvaCode, string suProfileCode)
    {
        var request = await _roleRequestRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException($"Solicitud de rol {uvaCode} no encontrada");

        if (request.RequestedRoleCode != "Admin")
        {
            throw new InvalidOperationException("Esta solicitud no es para Admin.");
        }

        request.Reject(suProfileCode, RoleRequestState.Rejected.GetUvaCode(), DateTime.UtcNow);
        await _roleRequestRepository.UpdateAsync(request);
    }

    private static RoleRequestResponseDto MapToDto(RoleRequest entity)
    {
        return new RoleRequestResponseDto
        {
            UvaCode = entity.UvaCode,
            RequesterProfileCode = entity.RequesterProfileCode,
            RequestedRole = entity.RequestedRoleCode,
            Reason = entity.Reason,
            DurationInMonths = entity.DurationInMonths,
            StateCode = entity.StateCode,
            ResolvedByProfileCode = entity.ResolvedByProfileCode,
            CreatedAt = entity.CreatedAt,
            ResolvedAt = entity.ResolvedAt
        };
    }
}
