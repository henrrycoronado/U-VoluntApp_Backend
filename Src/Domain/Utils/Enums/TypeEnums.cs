namespace U_VoluntApp_Core.Src.Domain.Utils.Enums;

public enum ActivityType
{
    [Uva("type-1", "taller")]
    Workshop,

    [Uva("type-2", "mentoria")]
    Mentoring,

    [Uva("type-3", "brigada")]
    Brigade,

    [Uva("type-4", "evento")]
    Event,

    [Uva("type-5", "colecta")]
    Collection,

    [Uva("type-6", "customize")]
    Customize
}

public enum EvidenceType
{
    [Uva("type-1", "check_in")]
    CheckIn,

    [Uva("type-2", "check_out")]
    CheckOut
}

public enum TrackingType
{
    [Uva("type-1", "scaning")]
    Scanning,

    [Uva("type-2", "manual")]
    Manual
}

public enum CareerType
{
    [Uva("type-1", "none")]
    None,

    [Uva("type-2", "ingenieria de software")]
    SoftwareEngineering,

    [Uva("type-3", "ingenieria civil")]
    CivilEngineering,

    [Uva("type-4", "derecho")]
    Law,

    [Uva("type-5", "medicina")]
    Medicine,

    [Uva("type-6", "administracion de empresas")]
    BusinessAdministration,

    [Uva("type-7", "psicologia")]
    Psychology,

    [Uva("type-8", "comunicacion social")]
    SocialCommunication,

    [Uva("type-9", "arquitectura")]
    Architecture,

    [Uva("type-10", "bioquimica")]
    Biochemistry,

    [Uva("type-11", "marketing")]
    Marketing
}

public enum ScholarshipType
{
    [Uva("type-1", "ceil")]
    Ceil,

    [Uva("type-2", "obispo")]
    Bishop,

    [Uva("type-3", "cre")]
    Cre,

    [Uva("type-4", "bachiller")]
    HighSchoolGraduate
}
