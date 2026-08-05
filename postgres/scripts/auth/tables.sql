create table auth.users
(
  id uuid default gen_random_uuid() not null,
    login text not null,
    email text not null,
    password_hash text not null,
    is_active bool default true not null,
    created_at timestamptz default now() not null,
    updated_at timestamptz default now() not null,
    
    constraint pkey_users primary key(id)
);

create unique index uq_users_email on auth.users(lower(email));
create unique index uq_users_login on auth.users(lower(login));
