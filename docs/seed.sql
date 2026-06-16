-- U-VoluntApp Mock Data Seeder
-- Includes Roles, Users, Profiles, Programs, Activities, and more.

-- 1. AspNetRoles
INSERT INTO public."AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp") VALUES
('role-volunteer', 'Volunteer', 'VOLUNTEER', '865239a5-7f5d-4f11-827c-3f9f36f68c34'),
('role-coordinator', 'Coordinator', 'COORDINATOR', '3d726b21-4f1e-4b7f-9b2a-8c8d7e6f5a4b'),
('role-admin', 'Admin', 'ADMIN', 'a1b2c3d4-e5f6-4a5b-bcde-f1e2d3c4b5a6'),
('role-superuser', 'SuperUser', 'SUPERUSER', '9f8e7d6c-5b4a-3a21-1098-76543210fedc')
ON CONFLICT ("Id") DO NOTHING;

-- 2. AspNetUsers (Passwords are dummy)
INSERT INTO public."AspNetUsers" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount") VALUES
('user-su', 'superuser@ucb.edu.bo', 'SUPERUSER@UCB.EDU.BO', 'superuser@ucb.edu.bo', 'SUPERUSER@UCB.EDU.BO', true, 'AQAAAAIAAYagAAAAEP...', 'SSTAMP1', 'CSTAMP1', false, false, true, 0),
('user-admin', 'admin@ucb.edu.bo', 'ADMIN@UCB.EDU.BO', 'admin@ucb.edu.bo', 'ADMIN@UCB.EDU.BO', true, 'AQAAAAIAAYagAAAAEP...', 'SSTAMP2', 'CSTAMP2', false, false, true, 0),
('user-coord-1', 'erick.mercado@ucb.edu.bo', 'ERICK.MERCADO@UCB.EDU.BO', 'erick.mercado@ucb.edu.bo', 'ERICK.MERCADO@UCB.EDU.BO', true, 'AQAAAAIAAYagAAAAEP...', 'SSTAMP3', 'CSTAMP3', false, false, true, 0),
('user-coord-2', 'natalia.vargas@ucb.edu.bo', 'NATALIA.VARGAS@UCB.EDU.BO', 'natalia.vargas@ucb.edu.bo', 'NATALIA.VARGAS@UCB.EDU.BO', true, 'AQAAAAIAAYagAAAAEP...', 'SSTAMP4', 'CSTAMP4', false, false, true, 0),
('user-vol-1', 'estudiante.uno@ucb.edu.bo', 'ESTUDIANTE.UNO@UCB.EDU.BO', 'estudiante.uno@ucb.edu.bo', 'ESTUDIANTE.UNO@UCB.EDU.BO', true, 'AQAAAAIAAYagAAAAEP...', 'SSTAMP5', 'CSTAMP5', false, false, true, 0),
('user-vol-2', 'estudiante.dos@ucb.edu.bo', 'ESTUDIANTE.DOS@UCB.EDU.BO', 'estudiante.dos@ucb.edu.bo', 'ESTUDIANTE.DOS@UCB.EDU.BO', true, 'AQAAAAIAAYagAAAAEP...', 'SSTAMP6', 'CSTAMP6', false, false, true, 0)
ON CONFLICT ("Id") DO NOTHING;

-- 3. AspNetUserRoles
INSERT INTO public."AspNetUserRoles" ("UserId", "RoleId") VALUES
('user-su', 'role-superuser'),
('user-admin', 'role-admin'),
('user-coord-1', 'role-coordinator'),
('user-coord-2', 'role-coordinator'),
('user-vol-1', 'role-volunteer'),
('user-vol-2', 'role-volunteer')
ON CONFLICT ("UserId", "RoleId") DO NOTHING;

-- 4. Profiles
INSERT INTO public.profiles (identity_user_id, uva_code, first_name, last_name, email, career_code, state_code, personal_goal_hours) VALUES
('user-su', 'profile-su', 'Super', 'User', 'superuser@ucb.edu.bo', 'type-2', 'stage-2', 0.00),
('user-admin', 'profile-admin', 'Admin', 'General', 'admin@ucb.edu.bo', 'type-2', 'stage-2', 0.00),
('user-coord-1', 'profile-coord-1', 'Erick Marco', 'Mercado', 'erick.mercado@ucb.edu.bo', 'type-2', 'stage-2', 0.00),
('user-coord-2', 'profile-coord-2', 'Natalia', 'Vargas Coimbra', 'natalia.vargas@ucb.edu.bo', 'type-11', 'stage-2', 0.00),
('user-vol-1', 'profile-vol-1', 'Juan', 'Perez', 'estudiante.uno@ucb.edu.bo', 'type-2', 'stage-2', 100.00),
('user-vol-2', 'profile-vol-2', 'Maria', 'Garcia', 'estudiante.dos@ucb.edu.bo', 'type-7', 'stage-2', 80.00);

-- 5. Vol Programs
INSERT INTO public.vol_programs (uva_code, name, acronym, manager_profile_code, state_code) VALUES
('6ca9df57-19d2-430b-9c4c-3ba868114f24', 'Ministerio de Monaguillos', NULL, 'profile-coord-1', 'stage-2'),
('711e5a59-9f79-43a9-a9a3-5c3b94b05a61', 'Ministerio de Musica', NULL, 'profile-coord-2', 'stage-2'),
('a8b27f4d-4c12-4217-91f1-ef0dbaf9e7a8', 'Catedra Cardenal Julio Terrazas', NULL, 'profile-admin', 'stage-2'),
('bf9f0951-b062-42df-b371-55bb40026e68', 'Alpha', NULL, 'profile-admin', 'stage-2'),
('d30f6a2d-b0a3-4886-9a25-ccceb4d7c0f1', 'Programa del Adulto Mayor - PAM', 'PAM', 'profile-admin', 'stage-2'),
('f1b9b1d9-81a1-432a-bc91-23d91eb69735', 'Formacion de Lideres', NULL, 'profile-admin', 'stage-2'),
('47c1a84f-e25c-4fdb-9e79-22a969f688e5', 'Mision Basilio', NULL, 'profile-admin', 'stage-2'),
('28d57579-251c-4395-9b24-9b0d3b6f27dc', 'Vina del Senor Plan 3000', NULL, 'profile-admin', 'stage-2'),
('9cfb3e2b-272e-4071-8bc6-eb528f89c748', 'Jovins', NULL, 'profile-admin', 'stage-2'),
('1842bc3c-f4df-41bb-98f5-4cf55097de69', 'Camino a/en la Universidad', NULL, 'profile-admin', 'stage-2'),
('ea667232-2775-47e9-a477-d6b38c234a5d', 'Alas de Esperanza', NULL, 'profile-admin', 'stage-2'),
('5180f98e-49b0-466d-a111-e7370788ab63', 'Recicla y Ayuda', NULL, 'profile-admin', 'stage-2'),
('349eb5be-968b-4b3c-b26a-9b1d1f05a9de', 'Promocion y difusion de la Pastoral', NULL, 'profile-admin', 'stage-2'),
('a2d21226-9f17-4d7a-8f35-6548ebc43f79', 'Atencion a emergencias', NULL, 'profile-admin', 'stage-2');

-- 6. Program Content
INSERT INTO public.program_content (uva_code, program_code, description, activities_description, mission_statement) VALUES
('pc-1', '6ca9df57-19d2-430b-9c4c-3ba868114f24', 'Fortalece la identidad catolica de universitarios desde los servicios religiosos.', 'Servicio liturgico. Formacion espiritual.', 'Senor, hazme un siervo leal.'),
('pc-2', '711e5a59-9f79-43a9-a9a3-5c3b94b05a61', 'Servicio y evangelizacion a traves del canto.', 'Ensayos de coro. Misas.', 'El que canta, ora dos veces.');

-- 7. Activities
INSERT INTO public.activities (uva_code, program_code, responsible_profile_code, activity_type_code, name, description, start_date, end_date, location_latitude, location_longitude, registration_radius_meters, state_code) VALUES
('act-1', '6ca9df57-19d2-430b-9c4c-3ba868114f24', 'profile-coord-1', 'type-4', 'Misa de Apertura', 'Misa comunitaria de inicio de semestre', now() + interval '1 day', now() + interval '1 day 2 hours', -17.7833, -63.1821, 100, 'stage-2'),
('act-2', '711e5a59-9f79-43a9-a9a3-5c3b94b05a61', 'profile-coord-2', 'type-1', 'Ensayo Semanal', 'Ensayo del ministerio de musica', now() + interval '2 days', now() + interval '2 days 3 hours', -17.7833, -63.1821, 50, 'stage-2');

-- 8. Activity Rules
INSERT INTO public.activity_rules (uva_code, activity_code, total_capacity, counts_volunteer_hours) VALUES
('rule-1', 'act-1', 50, true),
('rule-2', 'act-2', 20, true);

-- 9. Enrollments
INSERT INTO public.enrollments (uva_code, activity_code, enrolled_profile_code, state_code) VALUES
('enr-1', 'act-1', 'profile-vol-1', 'stage-2'),
('enr-2', 'act-1', 'profile-vol-2', 'stage-1'),
('enr-3', 'act-2', 'profile-vol-1', 'stage-2');

-- 10. Tracking Logs (Simulating past activities for analytics)
INSERT INTO public.tracking_logs (uva_code, enrollment_code, entry_time, exit_time, calculated_hours, state_code, check_in_registered_by_code) VALUES
('track-1', 'enr-1', now() - interval '1 day', now() - interval '1 day - 2 hours', 2.00, 'stage-2', 'profile-coord-1');

-- 11. User Scholarships
INSERT INTO public.user_scholarships (uva_code, assigned_profile_code, evaluator_profile_code, scholarship_type_code, reason, required_hours, state_code) VALUES
('sch-1', 'profile-vol-1', 'profile-admin', 'type-2', 'Beca Obispo por excelencia y servicio', 100.00, 'stage-2');

-- 12. Role Requests
INSERT INTO public.role_requests (uva_code, requester_profile_code, requested_role_id, reason, state_code) VALUES
('req-1', 'profile-vol-2', 'role-coordinator', 'Deseo apoyar en la coordinacion del programa Alpha', 'stage-1');
