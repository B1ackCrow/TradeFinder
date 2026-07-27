create or replace view catalog.vw_companies as
select
    c.id,
    c.name,
    c.description,
    c.logo as image,

    coalesce(
            (
                select jsonb_agg(
                               jsonb_build_object(
                                       'Id', l.id,
                                       'Name', l.name,
                                       'TimeZoneCode', l.timezone_code,
                                       'ParentLocationId', l.parent_location_id
                               )
                               order by l.name
                       )
                from location.company_locations cl
                         join location.locations l
                              on l.id = cl.location_id
                where cl.company_id = c.id
            ),
            '[]'::jsonb
    ) as locations,

    coalesce(
            (
                select jsonb_agg(
                               jsonb_build_object(
                                       'Id', cc.id,
                                       'TypeName', cct.name_ru,
                                       'Value', cc.value,
                                       'IconName', cct.icon_name
                               )
                               order by cct.name_ru
                       )
                from catalog.company_contacts cc
                         join catalog.company_contact_types cct
                              on cct.id = cc.company_contact_type_id
                where cc.company_id = c.id
                  and cc.is_active
            ),
            '[]'::jsonb
    ) as contacts,

    coalesce(
            (
                select jsonb_agg(
                               jsonb_build_object(
                                       'Id', p.id,
                                       'Name', p.name,
                                       'Image', p.image
                               )
                               order by p.name
                       )
                from catalog.company_products cp
                         join catalog.products p
                              on p.id = cp.product_id
                where cp.company_id = c.id
            ),
            '[]'::jsonb
    ) as products

from catalog.companies c
where c.is_active;