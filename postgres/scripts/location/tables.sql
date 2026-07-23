create table location.locations (
    id uuid DEFAULT gen_random_uuid() not null,
    name text not null,
    parent_location_id uuid null,
    
    constraint locations_pkey primary key (id),
    constraint 
);

create table catalog.companies (
                                   id uuid DEFAULT gen_random_uuid() NOT NULL,
                                   name text NOT NULL,
                                   description text NULL,
                                   inn varchar(12),
                                   is_active bool DEFAULT true NOT NULL,
                                   created_at timestamptz DEFAULT now() NOT NULL,
                                   updated_at timestamptz DEFAULT now() NOT NULL,

                                   CONSTRAINT companies_pkey PRIMARY KEY (id)
);
CREATE UNIQUE INDEX uq_companies_name ON catalog.companies USING btree (lower(name));
