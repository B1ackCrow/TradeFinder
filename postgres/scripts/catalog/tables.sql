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

create table catalog.company_contact_types (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    name text NOT NULL,
    
    CONSTRAINT  company_contact_types_pkey PRIMARY KEY (id)
);

create table catalog.company_contacts (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    company_id uuid not null,
    company_contact_type_id uuid not null,
    is_active bool DEFAULT true NOT NULL,
    created_at timestamptz DEFAULT now() NOT NULL,
    updated_at timestamptz DEFAULT now() NOT NULL,
    
    constraint pkey_company_contacts_id primary key (id),
    constraint fkey_company_contacts_company_id foreign key (company_id) references catalog.companies(id) on delete cascade,
    constraint fkey_company_contacts_company_contact_type_id foreign key (company_contact_type_id) references catalog.company_contact_types(id) on delete RESTRICT
);
create index idx_company_contacts_company_id on catalog.company_contacts using btree(company_id);
create index idx_company_contacts_company_contact_type_id on catalog.company_contacts using btree(company_contact_type_id);

create table catalog.products (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    name text not null,
    description text not null,
    
    constraint pkey_products_id primary key (id)
);

create table catalog.company_products (
    company_id uuid not null,
    product_id uuid not null,
    
    constraint pkey_company_products primary key (company_id, product_id)
);