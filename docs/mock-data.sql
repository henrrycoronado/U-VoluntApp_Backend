-- states
INSERT INTO public.profile_state (uva_code, name) VALUES 
('stage-1', 'inactive'), ('stage-2', 'active'), ('stage-3', 'deleted');

INSERT INTO public.program_state (uva_code, name) VALUES 
('stage-1', 'inactive'), ('stage-2', 'active'), ('stage-3', 'deleted');

INSERT INTO public.activity_state (uva_code, name) VALUES 
('stage-1', 'inactive'), ('stage-2', 'active'), ('stage-3', 'deleted'), ('stage-4', 'canceled');

INSERT INTO public.enrollment_state (uva_code, name) VALUES 
('stage-1', 'pending'), ('stage-2', 'active'), ('stage-3', 'rejected'), ('stage-4', 'canceled');

INSERT INTO public.tracking_state (uva_code, name) VALUES 
('stage-1', 'pending'), ('stage-2', 'active'), ('stage-3', 'deleted');

INSERT INTO public.contract_state (uva_code, name) VALUES 
('stage-1', 'pending'), ('stage-2', 'active'), ('stage-3', 'rejected'), ('stage-4', 'canceled');

INSERT INTO public.role_request_state (uva_code, name) VALUES 
('stage-1', 'pending'), ('stage-2', 'active'), ('stage-3', 'rejected'), ('stage-4', 'canceled');

-- types
INSERT INTO public.activity_type (uva_code, name) VALUES
('type-1', 'taller'), ('type-2', 'mentoria'), ('type-3', 'brigada'), ('type-4', 'evento'), ('type-5', 'colecta'), ('type-6', 'customize');

INSERT INTO public.evidence_type (uva_code, name) VALUES 
('type-1', 'check_in'), ('type-2', 'check_out');

INSERT INTO public.tracking_type (uva_code, name) VALUES 
('type-1', 'scaning'), ('type-2', 'manual');

INSERT INTO public.career_type (uva_code, name) VALUES 
('type-1', 'none'), ('type-2', 'ingenieria de software'), ('type-3', 'ingenieria civil'), ('type-4', 'derecho'), 
('type-5', 'medicina'), ('type-6', 'administracion de empresas'), ('type-7', 'psicologia'), ('type-8', 'comunicacion social'),
('type-9', 'arquitectura'), ('type-10', 'bioquimica'), ('type-11', 'marketing');

INSERT INTO public.scholarship_type (uva_code, name) VALUES 
('type-1', 'ceil'), ('type-2', 'obispo'), ('type-3', 'cre'), ('type-4', 'bachiller');

-- program data
INSERT INTO public.vol_programs (
    uva_code,
    name,
    acronym,
    state_code
) VALUES
('6ca9df57-19d2-430b-9c4c-3ba868114f24', 'Ministerio de Monaguillos', NULL, 'stage-2'),
('711e5a59-9f79-43a9-a9a3-5c3b94b05a61', 'Ministerio de Musica', NULL, 'stage-2'),
('a8b27f4d-4c12-4217-91f1-ef0dbaf9e7a8', 'Catedra Cardenal Julio Terrazas', NULL, 'stage-2'),
('bf9f0951-b062-42df-b371-55bb40026e68', 'Alpha', NULL, 'stage-2'),
('d30f6a2d-b0a3-4886-9a25-ccceb4d7c0f1', 'Programa del Adulto Mayor - PAM', 'PAM', 'stage-2'),
('f1b9b1d9-81a1-432a-bc91-23d91eb69735', 'Formacion de Lideres', NULL, 'stage-2'),
('47c1a84f-e25c-4fdb-9e79-22a969f688e5', 'Mision Basilio', NULL, 'stage-2'),
('28d57579-251c-4395-9b24-9b0d3b6f27dc', 'Vina del Senor Plan 3000', NULL, 'stage-2'),
('9cfb3e2b-272e-4071-8bc6-eb528f89c748', 'Jovins', NULL, 'stage-2'),
('1842bc3c-f4df-41bb-98f5-4cf55097de69', 'Camino a/en la Universidad', NULL, 'stage-2'),
('ea667232-2775-47e9-a477-d6b38c234a5d', 'Alas de Esperanza', NULL, 'stage-2'),
('5180f98e-49b0-466d-a111-e7370788ab63', 'Recicla y Ayuda', NULL, 'stage-2'),
('349eb5be-968b-4b3c-b26a-9b1d1f05a9de', 'Promocion y difusion de la Pastoral', NULL, 'stage-2'),
('a2d21226-9f17-4d7a-8f35-6548ebc43f79', 'Atencion a emergencias', NULL, 'stage-2');

-- program content data
INSERT INTO public.program_content (
    uva_code,
    description,
    activities_description,
    schedule_info,
    leadership_info,
    contact_info,
    mission_statement
) VALUES
('c977f682-1d12-4b2a-899d-1f6b8b0e8c78', 'Fortalece la identidad catolica de universitarios desde los servicios religiosos.',
 'Servicio liturgico. Formacion espiritual y liturgica. Animacion en actividades universitarias. Participacion en actividades arquidiocesanas.',
 'Martes: formacion de monaguillos Grupo 1 12:20 a 13:20. Viernes: formacion de monaguillos Grupo 2 12:20 a 13:20.',
 'Encargado: Erick Marco Mercado.',
 'Celular: 79936590.',
 'Senor, hazme un siervo leal, testigo de tu presencia, llevando tu luz con alegria.'),

('f215ea78-d513-40e9-b5f7-6bc8dbf03b21', 'Servicio y evangelizacion a traves del canto y animacion liturgica.',
 'Ensayos de coro. Misas en Parroquia Universitaria. Clases de guitarra.',
 'Viernes: 13:00 a 15:00 Capilla UCB campus. Sabado: 19:00 Misa Parroquia Universitaria. Jueves: 13:00 a 15:00.',
 'Encargada: Natalia Vargas Coimbra.',
 'Celular: 71038935.',
 'El que canta, ora dos veces.'),

('672e87c0-7fb4-4bc7-bd96-90dc77e0f2f3', 'Dialogo abierto entre estudiantes y expertos para desarrollo personal y social.',
 'Los jovenes eligen los temas. Algunas veces cine especial. El refrigerio al final. Proximamente en UAGM.',
 'Las catedras son semanales. Dia y hora cambian cada semestre segun horario libre de los estudiantes.',
 'Encargada: Camila Claure Perez.',
 'Instagram: catedralibre_ucb.',
 'Bienaventurado el hombre que encuentra la sabiduria y la inteligencia.'),

('9b5d214a-71b3-4f91-ba4a-4e2b5e28a506', 'Espacio para explorar grandes preguntas de vida y fe en sesiones semanales. Detalle: Retiro Alpha en junio (fin de semestre) y convivencia en diciembre (primeras semanas).',
 'Preparacion y logistica de sesiones. Ambientacion y refrigerio. Facilitacion de grupos pequenos. Aporte creativo y difusion. Organizacion del Retiro Alpha.',
 'Todos los lunes 12:20 a 13:20. Campus Universitario.',
 'Lideres de programa: Ariane Padilla Masai, Alessandra Maturana Zelaya.',
 'Celular: 69043747 / 70964501.',
 'Grandes preguntas merecen espacios autenticos.'),

('d30f6a2d-b0a3-4886-9a25-ccceb4d7c0f1', 'Dialogo intergeneracional y apoyo en talleres semanales para adultos mayores. Detalle: marzo Redes Sociales, abril Kinesiologia, mayo Psicologia, junio Manos que crean, agosto Fonoaudiologia, septiembre Agronegocios, octubre Nutricion, noviembre Espiritualidad.',
 'Apoyo en talleres semanales. Participacion en salidas y actividades externas. Guia y asistencia en el traslado dentro del campus.',
 'Sabados en la manana 8:30 a 12:00. Campus Universitario.',
 'Lider de programa: Nataly Arias Villarroel.',
 'Celular: 77168163.',
 'Donde jovenes y adultos mayores aprenden juntos.'),

('f1b9b1d9-81a1-432a-bc91-23d91eb69735', 'Formacion integral de voluntarios en espiritualidad, liderazgo y servicio. Detalle: mes 1 Identidad del lider y proposito de servicio; mes 2 Comunicacion, escucha y manejo de conflictos; mes 3 Trabajo en equipo y liderazgo colaborativo; mes 4 Organizacion, planificacion y responsabilidad; mes 5 Servicio pastoral, comunidad y mision; mes 6 Cierre, sintesis, compromiso y proyeccion.',
 'Formacion espiritual y discernimiento. Talleres de liderazgo y trabajo en equipo. Dinamicas participativas y escucha activa. Servicio y accion pastoral. Planificacion de programas y roles. Acompanamiento y evaluacion del proceso.',
 'Viernes 14:00 hrs (encuentro semanal).',
 'Lider de programa: Dilkar Fabricio Anez Justiniano.',
 'Celular: 63606824.',
 'Llamados a servir y enviados a liderar.'),

('47c1a84f-e25c-4fdb-9e79-22a969f688e5', 'Servicio y mision con jornadas periodicas y apoyo social y pastoral. Detalle: en cada mes del ano se destina el viaje de visita a distintas brigadas (medicas o psicologicas), obras sociales y religiosas.',
 'Campanas medicas y apoyo psicologico. Encuentros y dinamicas de formacion espiritual. Apoyo en celebraciones liturgicas. Limpieza, mantenimiento y pintura de la capilla. Actividades recreativas y de integracion comunitaria.',
 'Uno a dos sabados por mes. Manana o tarde.',
 'Lider de programa: Leonardo A. Espinoza R.',
 'Celular: 60027233.',
 'El que no nace para servir, no sirve para vivir.'),

('28d57579-251c-4395-9b24-9b0d3b6f27dc', 'Acompanamiento espiritual y catequesis para adultos mayores. Detalle: se realiza el ultimo jueves de cada mes del ano.',
 'Celebracion de una liturgia sencilla y participativa. Catequesis dirigida a adultos mayores. Dinamicas de convivencia e integracion. Compartir fraterno mediante un refrigerio.',
 'Ultimo jueves de cada mes de 10:00 a 12:00.',
 'Lider de programa: Leonardo A. Espinoza R.',
 'Celular: 60027233.',
 'Acompanando con fe y carino a quienes sembraron antes que nosotros.'),

('9cfb3e2b-272e-4071-8bc6-eb528f89c748', 'Programa misionero juvenil universitario.',
 NULL, NULL, NULL, NULL, NULL),

('1842bc3c-f4df-41bb-98f5-4cf55097de69', 'Mentorias y orientacion vocacional para jovenes antes y durante la universidad. Detalle: inicio de mentorias 09/03/26, inicio 2do semestre de mentorias 10/03/26, misiones en Basilio agosto y septiembre.',
 'Mentorias academicas grupales y de pares. Visitas y apoyo a colegios/comunidades con orientacion vocacional y motivacion academica. Talleres practicos: habitos de estudio, gestion del tiempo, organizacion y metas.',
 'Horarios y dias se establecen con los mentores. Misiones en las comunidades se definen de acuerdo a la comunidad.',
 'Lider de programa: Erick Rodriguez, Raul Quiroga.',
 'Celular: 79037773 / 78197517.',
 'Te guiamos para llegar a la universidad y te acompanamos para avanzar dentro de ella.'),

('ea667232-2775-47e9-a477-d6b38c234a5d', 'Voluntariado de apoyo integral a pacientes oncologicos y familias.',
 'Acompanamiento emocional y espiritual a pacientes oncologicos y sus familias. Talleres de autoestima y valores, actividades recreativas. Un rato a la esperanza. Campanas solidarias en coordinacion con el Instituto Oncologico del Oriente Boliviano.',
 'Durante los viernes del periodo academico en vigencia.',
 'Lideres de programa: Katia Monica Urquidi, Jose Luis Franco.',
 'Celular Katia: 78452324. Celular Jose: 61545052. Correo: ligaceco@gmail.com. Correo: jose.franco.cc@ucb.edu.bo.',
 NULL),

('5180f98e-49b0-466d-a111-e7370788ab63', 'Iniciativa de reciclaje para apoyar al instituto oncologico. Detalle: 8 de abril lanzamiento oficial, 9 de abril a 21 de junio semanas de recoleccion, 22 de junio cierre y pesaje final, 25 de junio entrega de donaciones y premiacion.',
 'Talleres de sensibilizacion en base al Laudato Si en colegios y parroquias. Recoleccion de materiales reciclables. Stand en la expo cruz. Participacion en ferias.',
 'Sujetos a convocatoria.',
 'Lideres del programa: Ana Niurka Aroca, M. Fernanda Aguiar.',
 'Celular: 73687239 / 63420274.',
 'Nadie puede hacerlo todo, pero todos podemos hacer algo.'),

('349eb5be-968b-4b3c-b26a-9b1d1f05a9de', 'Comunicacion y difusion de actividades pastorales.',
 NULL, NULL, NULL, NULL, NULL),

('a2d21226-9f17-4d7a-8f35-6548ebc43f79', 'Brigadas medicas y ayuda humanitaria en emergencias. Detalle: primera brigada segunda semana de abril, brigada en corpus christi 04/06 y futuras misiones en mas comunidades.',
 'Brigadas medicas en comunidades rurales. Apoyo humanitario en desastres (incendios, inundaciones, emergencias). Cobertura sanitaria en actividades y festividades locales.',
 'Horarios definidos con previa planificacion de la mision o brigada.',
 'Lideres de programa: Raul Quiroga, Rudi Jimenez.',
 'Celular: 78197517 / 66051675.',
 'Salud y apoyo cuando mas se necesita. Siempre listos para ayudar.');