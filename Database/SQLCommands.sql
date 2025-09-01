# Create new database (MySQL)
CREATE DATABASE `Akashi`
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

# Create new database (PostgreSQL)
CREATE DATABASE "natsinternal"
  ENCODING 'UTF8'
  LC_COLLATE = 'vi_VN.UTF-8'
  LC_CTYPE = 'vi_VN.UTF-8'
  TEMPLATE template0;

# Drop all tables (PostgreSQL)
DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN
        SELECT tablename
        FROM pg_tables
        WHERE schemaname = 'public'
    LOOP
        EXECUTE 'DROP TABLE IF EXISTS public.' || quote_ident(r.tablename) || ' CASCADE';
    END LOOP;
END $$;