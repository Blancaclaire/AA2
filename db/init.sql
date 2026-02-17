CREATE TABLE IF NOT EXISTS "Categories" (
    "Id"          SERIAL PRIMARY KEY,
    "Name"        VARCHAR(100) NOT NULL,
    "Description" VARCHAR(500) NOT NULL DEFAULT '',
    "Icon"        VARCHAR(20)  NOT NULL DEFAULT '',
    "IsActive"    BOOLEAN      NOT NULL DEFAULT TRUE,
    "CreatedAt"   TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "Users" (
    "Id"           SERIAL PRIMARY KEY,
    "Name"         VARCHAR(100) NOT NULL,
    "Email"        VARCHAR(256) NOT NULL UNIQUE,
    "PasswordHash" TEXT         NOT NULL,
    "Role"         VARCHAR(20)  NOT NULL DEFAULT 'student',
    "IsActive"     BOOLEAN      NOT NULL DEFAULT TRUE,
    "CreatedAt"    TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    "LastLoginAt"  TIMESTAMPTZ
);

CREATE TABLE IF NOT EXISTS "Courses" (
    "Id"              SERIAL PRIMARY KEY,
    "Title"           VARCHAR(200)     NOT NULL,
    "Description"     TEXT             NOT NULL DEFAULT '',
    "Instructor"      VARCHAR(100)     NOT NULL DEFAULT '',
    "Price"           NUMERIC(10,2)    NOT NULL DEFAULT 0,
    "DurationHours"   INT              NOT NULL DEFAULT 0,
    "Level"           VARCHAR(20)      NOT NULL DEFAULT 'Beginner',
    "ImageUrl"        TEXT             NOT NULL DEFAULT '',
    "IsPublished"     BOOLEAN          NOT NULL DEFAULT FALSE,
    "EnrollmentCount" INT              NOT NULL DEFAULT 0,
    "Rating"          DOUBLE PRECISION NOT NULL DEFAULT 0,
    "CreatedAt"       TIMESTAMPTZ      NOT NULL DEFAULT NOW(),
    "UpdatedAt"       TIMESTAMPTZ      NOT NULL DEFAULT NOW(),
    "CategoryId"      INT              NOT NULL REFERENCES "Categories"("Id")
);

CREATE TABLE IF NOT EXISTS "Enrollments" (
    "Id"              SERIAL PRIMARY KEY,
    "UserId"          INT         NOT NULL REFERENCES "Users"("Id")   ON DELETE CASCADE,
    "CourseId"        INT         NOT NULL REFERENCES "Courses"("Id") ON DELETE CASCADE,
    "EnrolledAt"      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "ProgressPercent" INT         NOT NULL DEFAULT 0,
    "IsCompleted"     BOOLEAN     NOT NULL DEFAULT FALSE,
    "CompletedAt"     TIMESTAMPTZ,
    UNIQUE("UserId", "CourseId")
);

INSERT INTO "Categories" ("Name","Description","Icon","CreatedAt") VALUES
    ('Programación',  'Cursos de desarrollo de software',         '💻','2024-01-01 00:00:00+00'),
    ('Diseño',        'Cursos de diseño gráfico y UX/UI',         '🎨','2024-01-01 00:00:00+00'),
    ('Marketing',     'Cursos de marketing digital',              '📈','2024-01-01 00:00:00+00'),
    ('Idiomas',       'Cursos de idiomas y comunicación',         '🌍','2024-01-01 00:00:00+00'),
    ('Negocios',      'Cursos de gestión empresarial y finanzas', '💼','2024-01-01 00:00:00+00');

INSERT INTO "Courses" ("Title","Description","Instructor","Price","DurationHours","Level","IsPublished","EnrollmentCount","Rating","CategoryId","CreatedAt","UpdatedAt") VALUES
    ('Vue 3 desde cero',           'Aprende Vue 3 con Composition API, Pinia y Vue Router',       'Ana García',      49.99,20,'Beginner',     TRUE,312,4.8,1,'2024-02-10 00:00:00+00','2024-02-10 00:00:00+00'),
    ('C# y .NET 8 Avanzado',       'Entity Framework, APIs REST y patrones de diseño',            'Carlos López',    69.99,35,'Advanced',     TRUE,189,4.9,1,'2024-03-05 00:00:00+00','2024-03-05 00:00:00+00'),
    ('Python para Data Science',   'NumPy, Pandas, Matplotlib y ML básico',                      'Laura Martínez',  59.99,28,'Intermediate', TRUE,445,4.7,1,'2024-04-15 00:00:00+00','2024-04-15 00:00:00+00'),
    ('Diseño UX/UI con Figma',     'Crea prototipos profesionales y sistemas de diseño',          'Miguel Ruiz',     44.99,16,'Beginner',     TRUE,278,4.6,2,'2024-05-20 00:00:00+00','2024-05-20 00:00:00+00'),
    ('Adobe Photoshop Completo',   'Retoque fotográfico y diseño gráfico profesional',            'Sofía Hernández', 39.99,22,'Intermediate', TRUE,521,4.5,2,'2024-06-01 00:00:00+00','2024-06-01 00:00:00+00'),
    ('SEO y Posicionamiento Web',  'Estrategias avanzadas para Google y Bing',                   'Javier Torres',   34.99,12,'Intermediate', TRUE,367,4.4,3,'2024-07-10 00:00:00+00','2024-07-10 00:00:00+00'),
    ('Marketing en Redes Sociales','Estrategia y contenido en Instagram, TikTok y LinkedIn',     'Patricia Gómez',  29.99,10,'Beginner',     TRUE,634,4.3,3,'2024-10-20 00:00:00+00','2024-10-20 00:00:00+00'),
    ('Inglés de Negocios B2',      'Comunicación profesional en inglés',                         'Emma Wilson',     54.99,30,'Intermediate', TRUE,198,4.7,4,'2024-08-05 00:00:00+00','2024-08-05 00:00:00+00'),
    ('Gestión de Proyectos Ágiles','Scrum, Kanban y metodologías ágiles en la práctica',         'Roberto Sánchez', 49.99,18,'Beginner',     TRUE,412,4.8,5,'2024-09-12 00:00:00+00','2024-09-12 00:00:00+00'),
    ('React y TypeScript',         'Desarrollo frontend moderno con hooks, context y testing',   'Ana García',      64.99,32,'Intermediate', FALSE,  0,0.0,1,'2024-10-01 00:00:00+00','2024-10-01 00:00:00+00');
