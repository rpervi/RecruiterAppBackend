IF DB_ID('recruitment_jobs_db') IS NULL
    CREATE DATABASE recruitment_jobs_db;
GO

IF DB_ID('recruitment_applications_db') IS NULL
    CREATE DATABASE recruitment_applications_db;
GO

IF DB_ID('recruitment_notifications_db') IS NULL
    CREATE DATABASE recruitment_notifications_db;
GO
