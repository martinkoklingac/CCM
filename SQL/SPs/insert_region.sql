CREATE OR REPLACE FUNCTION insert_region(name varchar(250), id integer = null)
RETURNS void
AS
$body$
DECLARE
	_lb numeric := null;
	_rb numeric := null;
BEGIN
	
	raise notice 'name : %', name;
	raise notice 'id : %', id;
	
	IF EXISTS(SELECT 1 FROM public."Regions" AS r WHERE r."Id" = id) THEN
		raise notice  'id [%] is null or Region is not present', id;
		
		
		
		
	ELSE
		
		SELECT COALESCE(MAX(r."RightBoundary"), -1) + 1 FROM "Regions" AS r
		INTO _lb;
		
		_rb = _lb + 1;
		
		RAISE NOTICE '_lb = %', _lb;
		RAISE NOTICE '_rb = %', _rb;
		
		
		INSERT INTO "Regions"("Name", "LeftBoundary", "RightBoundary") VALUES(name, _lb, _rb);
	END IF;
	
END;
$body$
LANGUAGE plpgsql