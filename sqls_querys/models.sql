CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS clicker_users
(
    user_id    SERIAL PRIMARY KEY,
    name       VARCHAR(255) NOT NULL,
    created_at TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP    NOT NULL DEFAULT NOW()
);
CREATE TABLE IF NOT EXISTS clicker_api_key
(
    api_key_id SERIAL PRIMARY KEY,
    name       VARCHAR(255) NOT NULL,
    hash_value       VARCHAR(255) NOT NULL,
    created_at TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP    NOT NULL DEFAULT NOW()
);
CREATE TABLE IF NOT EXISTS clicker_apps
(
    app_id     SERIAL PRIMARY KEY,
    user_id    INTEGER      NOT NULL REFERENCES clicker_users (user_id),
    url        VARCHAR(255) NOT NULL,
    app_name   VARCHAR(255) NOT NULL,
    api_key_id INTEGER      NOT NULL REFERENCES clicker_api_key (api_key_id),
    created_at TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP    NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS clicker_events
(
    app_name     VARCHAR(255) NOT NULL,
    event_type   VARCHAR(255) NOT NULL,
    element_id   VARCHAR(255),
    element_type VARCHAR(255),
    element_text VARCHAR(255),
    page_url     VARCHAR(255),
    page_title   VARCHAR(255),
    path_name    VARCHAR(255),
    id           VARCHAR(255) NOT NULL,
    country      VARCHAR(255) NOT NULL,
    locale       VARCHAR(255) NOT NULL,
    platform     VARCHAR(255) NOT NULL,
    user_agent   VARCHAR(255) NOT NULL,
    timestamp    TIMESTAMP    NOT NULL DEFAULT NOW(),
    kafka_offset INTEGER      NOT NULL

);

CREATE TABLE IF NOT EXISTS clicker_events_simple
(
    element_id   VARCHAR(255),
    element_type VARCHAR(255),
    element_text VARCHAR(255),
    page_url     VARCHAR(255),
    page_title   VARCHAR(255),
    path_name    VARCHAR(255),
    id           VARCHAR(255) NOT NULL,
    country      VARCHAR(255) NOT NULL,
    locale       VARCHAR(255) NOT NULL,
    platform     VARCHAR(255) NOT NULL,
    user_agent   VARCHAR(255) NOT NULL,
    timestamp    TIMESTAMP    NOT NULL DEFAULT NOW(),
    kafka_offset INTEGER      NOT NULL
);



--- Selects



select *
from clicker_events_simple;