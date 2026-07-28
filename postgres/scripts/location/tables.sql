create table location.location_types (
    id uuid default gen_random_uuid() not null,
    name text not null,
    code text not null,
    
    constraint pkey_location_types primary key (id)
);

create unique index uq_location_types on location.location_types (code);

create table location.locations (
    id uuid DEFAULT gen_random_uuid() not null,
    name text not null,
    parent_location_id uuid null,
    timezone_code text not null,
    location_type_id uuid not null,
    
    constraint pkey_locations primary key (id),
    constraint fkey_locations_parent_location_id foreign key (parent_location_id) references location.locations (id) on delete restrict,
    constraint fkey_locations_location_type_id foreign key (location_type_id) references location.location_types (id) on delete restrict   
);

create index idx_locations_locations_parent_location_id on location.locations using btree (parent_location_id);

create table location.company_locations (
  id uuid DEFAULT gen_random_uuid() not null,
  company_id uuid not null,
  location_id uuid not null,
  
  constraint pkey_company_locations primary key (id),
    constraint fkey_company_locations_company_id foreign key (company_id) references catalog.companies (id) on delete cascade,
    constraint fkey_company_locations_location_id foreign key (location_id) references location.locations (id) on delete restrict 
);

create unique index uq_company_locations on location.company_locations (company_id, location_id);
create index idx_company_locations_company_id on location.company_locations using btree (company_id);
create index idx_company_locations_location_id on location.company_locations using btree (location_id);