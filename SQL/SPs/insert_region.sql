CREATE OR REPLACE FUNCTION insert_region(name varchar(250), parent_id integer = null)
RETURNS regions
AS
$body$
DECLARE
	_lb NUMERIC := NULL;
	_rb NUMERIC := NULL;
	_interval NUMERIC := NULL;
	_region regions;
BEGIN
	
	--raise notice 'name : %', name;
	--raise notice 'id : %', id;
	
	IF EXISTS(SELECT 1 FROM regions AS r WHERE r.id = parent_id) THEN
		--raise notice  'id [%] is null or Region is not present', id;
		
		-- Set the left boundary
		SELECT r.left_bound FROM regions AS r WHERE r.id = parent_id
		INTO _lb;
		
		-- Set the right boundary
		-- We will insert children from the left by default
		SELECT
			COALESCE((
				SELECT MIN(rc.left_bound) FROM regions AS rc 
				WHERE rc.left_bound BETWEEN rp.left_bound AND rp.right_bound
				AND rc.id <> rp.id), 
				rp.right_bound)
		FROM regions AS rp
		WHERE rp.id = parent_id
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
		
		SELECT COALESCE(MAX(r.right_bound), -1) + 1 FROM regions AS r
		INTO _lb;
		
		_rb = _lb + 1;
		
		--RAISE NOTICE '_lb = %', _lb;
		--RAISE NOTICE '_rb = %', _rb;
		
	END IF;
	
	INSERT INTO regions(name, left_bound, right_bound) 
	VALUES(name, _lb, _rb) RETURNING * INTO _region;
	RETURN _region;
	
END;
$body$
LANGUAGE plpgsql