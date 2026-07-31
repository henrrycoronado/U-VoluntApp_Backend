namespace U_VoluntApp_Core.Src.Domain.Utils.Factories;

using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Domain.Entities.Activity;

public interface IActivityFactory
{
    (Activity Activity, ActivityRule Rule) CreateWorkshop(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateMentoring(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateBrigade(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateMultiDay(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CreateEvent(CreateActivitySimpleDto dto);

    (Activity Activity, ActivityRule Rule) CloneFromMultiDayTemplate(CreateActivitySimpleDto simpleDto);
}
