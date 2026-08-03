using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace U_VoluntApp_Core.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterializedViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE MATERIALIZED VIEW public.mv_scholarship_performance AS
SELECT
    us.uva_code AS scholarship_code,
    p.uva_code AS profile_code,
    p.first_name,
    p.last_name,
    us.scholarship_type_code AS scholarship_type,
    us.required_hours,
    COALESCE(SUM(tl.calculated_hours), 0) AS completed_hours,
    GREATEST(us.required_hours - COALESCE(SUM(tl.calculated_hours), 0), 0) AS remaining_hours,
    CASE
        WHEN us.required_hours > 0 THEN ROUND((COALESCE(SUM(tl.calculated_hours), 0) / us.required_hours) * 100, 2)
        ELSE 0
    END AS completion_percentage,
    us.state_code AS contract_state,
    us.end_date
FROM public.user_scholarships us
JOIN public.profiles p ON us.assigned_profile_code = p.uva_code
LEFT JOIN public.enrollments e ON p.uva_code = e.enrolled_profile_code
LEFT JOIN public.tracking_logs tl ON e.uva_code = tl.enrollment_code
    AND tl.state_code = 'stage-2'
    AND tl.created_at >= us.start_date
    AND tl.deleted_at IS NULL
GROUP BY
    p.uva_code, p.first_name, p.last_name, us.scholarship_type_code, us.required_hours, us.state_code, us.end_date, us.uva_code;

CREATE UNIQUE INDEX idx_mv_scholarship_perf_code ON public.mv_scholarship_performance(scholarship_code);

CREATE MATERIALIZED VIEW public.mv_program_analytics AS
WITH activity_hours AS (
    SELECT
        a.program_code,
        SUM(tl.calculated_hours) AS total_hours
    FROM public.activities a
    LEFT JOIN public.enrollments e ON a.uva_code = e.activity_code
        AND e.state_code = 'stage-2'
        AND e.deleted_at IS NULL
    LEFT JOIN public.tracking_logs tl ON e.uva_code = tl.enrollment_code
        AND tl.state_code = 'stage-2'
        AND tl.deleted_at IS NULL
    WHERE a.deleted_at IS NULL
    GROUP BY a.program_code
),
volunteer_counts AS (
    SELECT
        a.program_code,
        COUNT(DISTINCT e.enrolled_profile_code) AS total_unique_volunteers
    FROM public.activities a
    LEFT JOIN public.enrollments e ON a.uva_code = e.activity_code
        AND e.state_code = 'stage-2'
        AND e.deleted_at IS NULL
    WHERE a.deleted_at IS NULL
    GROUP BY a.program_code
)
SELECT
    p.uva_code AS program_code,
    p.name AS program_name,
    COUNT(DISTINCT a.uva_code) AS total_activities,
    COALESCE(vc.total_unique_volunteers, 0) AS total_unique_volunteers,
    COALESCE(ah.total_hours, 0) AS total_generated_hours
FROM public.vol_programs p
LEFT JOIN public.activities a ON p.uva_code = a.program_code AND a.deleted_at IS NULL
LEFT JOIN activity_hours ah ON p.uva_code = ah.program_code
LEFT JOIN volunteer_counts vc ON p.uva_code = vc.program_code
WHERE p.deleted_at IS NULL
GROUP BY
    p.uva_code, p.name, ah.total_hours, vc.total_unique_volunteers;

CREATE UNIQUE INDEX idx_mv_program_analytics_code ON public.mv_program_analytics(program_code);

CREATE MATERIALIZED VIEW public.mv_activity_analytics AS
WITH enrollment_hours AS (
    SELECT
        e.activity_code,
        COUNT(DISTINCT e.uva_code) AS total_enrolled,
        COUNT(DISTINCT CASE WHEN tl.uva_code IS NOT NULL THEN e.uva_code END) AS total_attended,
        SUM(tl.calculated_hours) AS total_activity_hours
    FROM public.enrollments e
    LEFT JOIN public.tracking_logs tl ON e.uva_code = tl.enrollment_code
        AND tl.state_code = 'stage-2'
        AND tl.deleted_at IS NULL
    WHERE e.state_code = 'stage-2'
        AND e.deleted_at IS NULL
    GROUP BY e.activity_code
)
SELECT
    a.uva_code AS activity_code,
    p.uva_code AS program_code,
    p.name AS program_name,
    a.name AS activity_name,
    a.start_date,
    a.end_date,
    COALESCE(ar.total_capacity, 0) AS total_capacity,
    COALESCE(eh.total_enrolled, 0) AS total_enrolled,
    COALESCE(eh.total_attended, 0) AS total_attended,
    COALESCE(eh.total_activity_hours, 0) AS total_activity_hours
FROM public.activities a
JOIN public.vol_programs p ON a.program_code = p.uva_code AND p.deleted_at IS NULL
LEFT JOIN public.activity_rules ar ON a.uva_code = ar.activity_code
LEFT JOIN enrollment_hours eh ON a.uva_code = eh.activity_code
WHERE a.deleted_at IS NULL;

CREATE UNIQUE INDEX idx_mv_activity_analytics_code ON public.mv_activity_analytics(activity_code);

CREATE MATERIALIZED VIEW public.mv_volunteer_history AS
SELECT
    p.uva_code AS profile_code,
    p.first_name,
    p.last_name,
    p.career_code AS career_name,
    p.personal_goal_hours,
    COUNT(DISTINCT e.activity_code) AS total_activities_participated,
    COALESCE(SUM(CASE WHEN tl.state_code = 'stage-2' THEN tl.calculated_hours ELSE 0 END), 0) AS validated_hours,
    COALESCE(SUM(CASE WHEN tl.state_code IN ('stage-2', 'stage-1') THEN tl.calculated_hours ELSE 0 END), 0) AS total_logged_hours,
    MAX(tl.created_at) AS last_activity_date
FROM public.profiles p
LEFT JOIN public.enrollments e ON p.uva_code = e.enrolled_profile_code
    AND e.state_code = 'stage-2'
    AND e.deleted_at IS NULL
LEFT JOIN public.tracking_logs tl ON e.uva_code = tl.enrollment_code AND tl.deleted_at IS NULL
WHERE p.deleted_at IS NULL
GROUP BY
    p.uva_code, p.first_name, p.last_name, p.career_code, p.personal_goal_hours;

CREATE UNIQUE INDEX idx_mv_volunteer_history_profile_code ON public.mv_volunteer_history(profile_code);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP MATERIALIZED VIEW IF EXISTS public.mv_scholarship_performance;
DROP MATERIALIZED VIEW IF EXISTS public.mv_program_analytics;
DROP MATERIALIZED VIEW IF EXISTS public.mv_activity_analytics;
DROP MATERIALIZED VIEW IF EXISTS public.mv_volunteer_history;
            ");
        }
    }
}
