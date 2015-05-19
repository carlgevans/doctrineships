-- update [ebs_DATADUMP].[dbo].[invTypes] from https://developers.eveonline.com/resource/static-data-export

-- see new stuff
  select p.[ComponentID], u.[typeID], p.[Name], u.[typeName] as NewTypeName 
  from [DoctrineShipsProd].[dbo].[Components] p right outer join [ebs_DATADUMP].[dbo].[invTypes] u
  on p.[ComponentID] = u.[typeID] or p.ComponentID is null
  where p.[Name] <> u.[typeName] or p.Name is null
  and u.[typeName] not like '%SKIN%'
  and u.[typeName] not like '%Blueprint%';

  -- update changed names
select 'update [DoctrineShipsProd].[dbo].[Components] set name = ''' + u.[typeName] + ''' where [ComponentID] = ' + convert(varchar(6), u.[typeID]) as query
  from [DoctrineShipsProd].[dbo].[Components] p right outer join [ebs_DATADUMP].[dbo].[invTypes] u
  on p.[ComponentID] = u.[typeID]
  where p.[Name] <> u.[typeName];

  -- see missing items
  select u.[typeID] as ComponentID, u.[typeName] as Name, 
  'https://image.eveonline.com/InventoryType/' + convert(varchar(6), u.[typeID]) + '_32.png' as ImageUrl, 
  u.volume as Volume
  from [DoctrineShipsProd].[dbo].[Components] p right outer join [ebs_DATADUMP].[dbo].[invTypes] u
  on p.[ComponentID] = u.[typeID]
  where p.[ComponentID] is null
  and u.[typeName] not like '%SKIN%'
  and u.[typeName] not like '%Blueprint%'
  and u.volume > 0;

  -- insert missing items
  insert into [DoctrineShipsProd].[dbo].[Components]
  select u.[typeID] as ComponentID, u.[typeName] as Name, 
  'https://image.eveonline.com/InventoryType/' + convert(varchar(6), u.[typeID]) + '_32.png' as ImageUrl, 
  u.volume as Volume 
  from [DoctrineShipsProd].[dbo].[Components] p right outer join [ebs_DATADUMP].[dbo].[invTypes] u
  on p.[ComponentID] = u.[typeID]
  where p.[ComponentID] is null
  and u.[typeName] not like '%SKIN%'
  and u.[typeName] not like '%Blueprint%'
  and u.volume > 0;

  -- you may need to go through and modify some volumes to be the packaged volume