CREATE OR REPLACE FUNCTION insert_region(name varchar(250), id integer = null)
RETURNS "Regions"
AS
$body$
DECLARE
	_lb NUMERIC := NULL;
	_rb NUMERIC := NULL;
	_interval NUMERIC := NULL;
	_region "Regions";
BEGIN
	
	--raise notice 'name : %', name;
	--raise notice 'id : %', id;
	
	IF EXISTS(SELECT 1 FROM public."Regions" AS r WHERE r."Id" = id) THEN
		--raise notice  'id [%] is null or Region is not present', id;
		
		-- Set the left boundary
		SELECT r."LeftBoundary" FROM public."Regions" AS r WHERE r."Id" = id
		INTO _lb;
		
		-- Set the right boundary
		-- We will insert children from the left by default
		SELECT
			COALESCE((
				SELECT MIN(rc."LeftBoundary") FROM "Regions" AS rc 
				WHERE rc."LeftBoundary" BETWEEN rp."LeftBoundary" AND rp."RightBoundary"
				AND rc."Id" <> rp."Id"), 
				rp."RightBoundary")
		FROM "Regions" AS rp
		WHERE rp."Id" = id
		INTO _rb;
		
		
		--RAISE NOTICE '_lb = %', _lb;
		--RAISE NOTICE '_rb = %', _rb;
		
		SELECT ((_rb - _lb) / 3)
		INTO _interval;
		
		SELECT _lb + _interval
		INTO _lb;
		
		SELECT _rb - _interval
		INTO _rb;
		
		--RAISE NOTICE '- _lb = %', _lb;
		--RAISE NOTICE '- _rb = %', _rb;
		
	ELSE
		
		SELECT COALESCE(MAX(r."RightBoundary"), -1) + 1 FROM "Regions" AS r
		INTO _lb;
		
		_rb = _lb + 1;
		
		--RAISE NOTICE '_lb = %', _lb;
		--RAISE NOTICE '_rb = %', _rb;
		
	END IF;
	
	INSERT INTO "Regions"("Name", "LeftBoundary", "RightBoundary") 
	VALUES(name, _lb, _rb) RETURNING * INTO _region;
	RETURN _region;
END;
$body$
LANGUAGE plpgsql