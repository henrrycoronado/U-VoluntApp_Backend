namespace U_VoluntApp_Core.Src.Domain.Utils.Enums;

public enum ProfileState
{
    [Uva("stage-1", "inactive")]
    Inactive,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "deleted")]
    Deleted
}

public enum ProgramState
{
    [Uva("stage-1", "inactive")]
    Inactive,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "deleted")]
    Deleted
}

public enum ActivityState
{
    [Uva("stage-1", "inactive")]
    Inactive,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "deleted")]
    Deleted,

    [Uva("stage-4", "canceled")]
    Canceled
}

public enum EnrollmentState
{
    [Uva("stage-1", "pending")]
    Pending,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "rejected")]
    Rejected,

    [Uva("stage-4", "canceled")]
    Canceled
}

public enum TrackingState
{
    [Uva("stage-1", "pending")]
    Pending,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "deleted")]
    Deleted
}

public enum ContractState
{
    [Uva("stage-1", "pending")]
    Pending,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "rejected")]
    Rejected,

    [Uva("stage-4", "canceled")]
    Canceled
}

public enum RoleRequestState
{
    [Uva("stage-1", "pending")]
    Pending,

    [Uva("stage-2", "active")]
    Active,

    [Uva("stage-3", "rejected")]
    Rejected,

    [Uva("stage-4", "canceled")]
    Canceled
}
