#!/usr/bin/env bash

DB_NAME="natsinternal"

export PGPASSWORD="Huyy47b1"

psql -U postgres -h localhost -v ON_ERROR_STOP=1 postgres <<SQL
SELECT pg_terminate_backend(pid)
FROM pg_stat_activity
WHERE datname = '$DB_NAME';

DROP DATABASE IF EXISTS $DB_NAME;

CREATE DATABASE $DB_NAME
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = icu
    ICU_LOCALE = 'vi'
    TEMPLATE = template0;
SQL

unset PGPASSWORD