/*
*	get_region_primogenitors()
*	Returns the top level regions.
*/
CREATE OR REPLACE FUNCTION get_region_primogenitors()
RETURNS SETOF regions
AS
$body$
BEGIN
	RETURN QUERY
	SELECT
		r.*
	FROM regions AS r
	INNER JOIN region_depth AS rd ON (r.id = rd.id)
	WHERE rd.depth = 0;
END;
$body$
LANGUAGE plpgsql