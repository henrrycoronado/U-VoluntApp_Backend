-- ==============================================================================
-- U-VOLUNTAPP DATABASE SCHEMA
-- This script contains the complete structure for the U-VoluntApp database,
-- including Identity, Catalogs, States, Types, and Business Logic tables.
-- ==============================================================================

-- 0. EF MIGRATIONS HISTORY
CREATE TABLE IF NOT EXISTS public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- 1. IDENTITY TABLES
CREATE TABLE public."AspNetRoles" (
    "Id" VARCHAR(450) PRIMARY KEY,
    "Name" VARCHAR(256) UNIQUE,
    "NormalizedName" VARCHAR(256) UNIQUE,
    "ConcurrencyStamp" TEXT
);

CREATE TABLE public."AspNetUsers" (
    "Id" VARCHAR(450) PRIMARY KEY,
    "UserName" VARCHAR(256) UNIQUE NOT NULL,
    "NormalizedUserName" VARCHAR(256) UNIQUE,
    "Email" VARCHAR(256) UNIQUE,
    "NormalizedEmail" VARCHAR(256),
    "EmailConfirmed" BOOLEAN DEFAULT false NOT NULL,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN DEFAULT false NOT NULL,
    "TwoFactorEnabled" BOOLEAN DEFAULT false NOT NULL,
    "LockoutEnd" TIMESTAMPTZ,
    "LockoutEnabled" BOOLEAN DEFAULT true NOT NULL,
    "AccessFailedCount" INT DEFAULT 0 NOT NULL
);

CREATE TABLE public."AspNetUserRoles" (
    "UserId" VARCHAR(450) NOT NULL REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE,
    "RoleId" VARCHAR(450) NOT NULL REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE,
    PRIMARY KEY ("UserId", "RoleId")
);

CREATE TABLE public."AspNetUserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" VARCHAR(450) NOT NULL REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE,
    "ClaimType" TEXT,
    "ClaimValue" TEXT
);

CREATE TABLE public."AspNetUserLogins" (
    "LoginProvider" VARCHAR(128) NOT NULL,
    "ProviderKey" VARCHAR(128) NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" VARCHAR(450) NOT NULL REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE,
    PRIMARY KEY ("LoginProvider", "ProviderKey")
);

CREATE TABLE public."AspNetRoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" VARCHAR(450) NOT NULL REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE,
    "ClaimType" TEXT,
    "ClaimValue" TEXT
);

CREATE TABLE public."AspNetUserTokens" (
    "UserId" VARCHAR(450) NOT NULL REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE,
    "LoginProvider" VARCHAR(128) NOT NULL,
    "Name" VARCHAR(128) NOT NULL,
    "Value" TEXT,
    PRIMARY KEY ("UserId", "LoginProvider", "Name")
);

-- 2. CATALOGS (TYPES)
CREATE TABLE public.career_type ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE,
    is_active BOOLEAN DEFAULT true NOT NULL
);

CREATE TABLE public.scholarship_type ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE,
    is_active BOOLEAN DEFAULT true NOT NULL
);

CREATE TABLE public.activity_type ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE,
    is_active BOOLEAN DEFAULT true NOT NULL
);

CREATE TABLE public.evidence_type ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE,
    is_active BOOLEAN DEFAULT true NOT NULL
);

CREATE TABLE public.tracking_type ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE,
    is_active BOOLEAN DEFAULT true NOT NULL
);

-- 3. STATES
CREATE TABLE public.profile_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE public.program_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE public.activity_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE public.enrollment_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE public.tracking_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE public.contract_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE public.role_request_state ( 
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- 4. BUSINESS CORE TABLES

-- Profiles
CREATE TABLE public.profiles (
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    identity_user_id VARCHAR(450) NOT NULL REFERENCES public."AspNetUsers"("Id"),
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE,
    photo_url TEXT,
    career_code VARCHAR(100) REFERENCES public.career_type(uva_code),
    address_location TEXT,
    phone TEXT,
    personal_goal_hours NUMERIC(5,2) DEFAULT 0.00 NOT NULL,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.profile_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Volunteering Programs
CREATE TABLE public.vol_programs (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    name TEXT NOT NULL,
    acronym TEXT,
    manager_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code),
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.program_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Program Content (Detailed info)
CREATE TABLE public.program_content (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    program_code VARCHAR(100) UNIQUE NOT NULL REFERENCES public.vol_programs(uva_code),
    description TEXT,
    activities_description TEXT,
    schedule_info TEXT,
    leadership_info TEXT,
    contact_info TEXT,
    mission_statement TEXT,
    profile_photo_url TEXT,
    cover_photo_url TEXT,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ
);

-- Program Collaborators
CREATE TABLE public.program_collaborators (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    program_code VARCHAR(100) REFERENCES public.vol_programs(uva_code) NOT NULL,
    profile_code VARCHAR(100) REFERENCES public.profiles(uva_code) NOT NULL,
    assigned_by_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code),
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.contract_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Activity Recurrence Patterns
CREATE TABLE public.activity_recurrence_patterns (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    program_code VARCHAR(100) REFERENCES public.vol_programs(uva_code) NOT NULL,
    name TEXT NOT NULL,
    recurrence_type VARCHAR(20) NOT NULL,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.activity_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Activity Recurrence Details
CREATE TABLE public.activity_recurrence_detail (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    activity_recurrence_pattern_code VARCHAR(100) REFERENCES public.activity_recurrence_patterns(uva_code) NOT NULL,
    day_of_week SMALLINT,
    day_of_month SMALLINT,
    week_of_month SMALLINT,
    start_hour TIME,
    end_hour TIME,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.activity_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Activities
CREATE TABLE public.activities (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    program_code VARCHAR(100) REFERENCES public.vol_programs(uva_code) NOT NULL,
    responsible_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code),
    activity_type_code VARCHAR(100) REFERENCES public.activity_type(uva_code) NOT NULL,
    activity_recurrence_pattern_code VARCHAR(100) REFERENCES public.activity_recurrence_patterns(uva_code),
    name TEXT NOT NULL,
    description TEXT,
    start_date TIMESTAMPTZ NOT NULL,
    end_date TIMESTAMPTZ NOT NULL,
    location_latitude DOUBLE PRECISION NOT NULL,
    location_longitude DOUBLE PRECISION NOT NULL,
    registration_radius_meters INT DEFAULT 50 NOT NULL,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.activity_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Activity Rules
CREATE TABLE public.activity_rules (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    activity_code VARCHAR(100) REFERENCES public.activities(uva_code) UNIQUE NOT NULL,
    requires_enrollment BOOLEAN DEFAULT true NOT NULL,
    enrollment_deadline TIMESTAMPTZ,
    requires_approval BOOLEAN DEFAULT false NOT NULL,
    total_capacity INT DEFAULT 0,
    cost_amount NUMERIC(10,2) DEFAULT 0.0 NOT NULL,
    counts_volunteer_hours BOOLEAN DEFAULT true NOT NULL,
    photo_url TEXT,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ
);

-- Enrollments
CREATE TABLE public.enrollments (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    activity_code VARCHAR(100) REFERENCES public.activities(uva_code) NOT NULL,
    enrolled_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code) NOT NULL,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.enrollment_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Activity Groups
CREATE TABLE public.activity_group (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    activity_code VARCHAR(100) REFERENCES public.activities(uva_code) NOT NULL,
    name TEXT NOT NULL,
    details TEXT,
    total_capacity INT DEFAULT 0,
    start_date TIMESTAMPTZ,
    end_date TIMESTAMPTZ,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.activity_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Group Enrollments
CREATE TABLE public.group_enrollment (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    activity_group_code VARCHAR(100) REFERENCES public.activity_group(uva_code) NOT NULL,
    enrollment_code VARCHAR(100) REFERENCES public.enrollments(uva_code) NOT NULL,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.activity_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Tracking Logs
CREATE TABLE public.tracking_logs (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    enrollment_code VARCHAR(100) REFERENCES public.enrollments(uva_code) NOT NULL,
    group_enrollment_code VARCHAR(100) REFERENCES public.group_enrollment(uva_code),
    entry_time TIMESTAMPTZ,
    exit_time TIMESTAMPTZ,
    calculated_hours NUMERIC(5,2) DEFAULT 0.00 NOT NULL,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.tracking_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ,
    check_in_registered_by_code VARCHAR(100) REFERENCES public.profiles(uva_code),
    check_out_registered_by_code VARCHAR(100) REFERENCES public.profiles(uva_code)
);

-- Evidences
CREATE TABLE public.evidences (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    tracking_log_code VARCHAR(100) REFERENCES public.tracking_logs(uva_code) NOT NULL,
    photo_url TEXT NOT NULL,
    evidence_type_code VARCHAR(100) REFERENCES public.evidence_type(uva_code) NOT NULL,
    type_code VARCHAR(100) REFERENCES public.tracking_type(uva_code) NOT NULL,
    observations TEXT,
    location_latitude DOUBLE PRECISION NOT NULL,
    location_longitude DOUBLE PRECISION NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- User Scholarships
CREATE TABLE public.user_scholarships (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    assigned_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code) NOT NULL,
    evaluator_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code),
    scholarship_type_code VARCHAR(100) REFERENCES public.scholarship_type(uva_code) NOT NULL,
    reason TEXT NOT NULL,
    required_hours NUMERIC(5,2) DEFAULT 100.00 NOT NULL,
    start_date TIMESTAMPTZ DEFAULT now() NOT NULL,
    end_date TIMESTAMPTZ,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.contract_state(uva_code) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ
);

-- Role Requests
CREATE TABLE public.role_requests (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    requester_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code) NOT NULL,
    requested_role_id VARCHAR(450) NOT NULL REFERENCES public."AspNetRoles"("Id"),
    reason TEXT NOT NULL,
    duration_in_months INT,
    state_code VARCHAR(100) DEFAULT 'stage-1' REFERENCES public.role_request_state(uva_code) NOT NULL,
    resolved_by_profile_code VARCHAR(100) REFERENCES public.profiles(uva_code),
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    updated_at TIMESTAMPTZ,
    deleted_at TIMESTAMPTZ,
    resolved_at TIMESTAMPTZ
);

-- Refresh Tokens (JWT)
CREATE TABLE public.refresh_tokens (
    id SERIAL PRIMARY KEY,
    uva_code VARCHAR(100) NOT NULL UNIQUE,
    identity_user_id VARCHAR(450) NOT NULL REFERENCES public."AspNetUsers"("Id"),
    profile_code VARCHAR(100) NOT NULL REFERENCES public.profiles(uva_code),
    token_hash VARCHAR(128) NOT NULL UNIQUE,
    expires_at TIMESTAMPTZ NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now() NOT NULL,
    revoked_at TIMESTAMPTZ,
    replaced_by_token_hash VARCHAR(128),
    created_by_ip VARCHAR(100),
    revoked_by_ip VARCHAR(100),
    user_agent VARCHAR(512),
    reason_revoked VARCHAR(200)
);
