namespace U_VoluntApp_Backend.Src.Domain.Utils.Factories;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;

public interface IActivityFactory
{
    (Activity Activity, ActivityRule Rule) CreateWorkshop(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateMentoring(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateBrigade(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateMultiDay(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateEvent(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CloneFromMultiDayTemplate(CreateActivitySimpleDto simpleDto);
}
