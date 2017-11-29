/*
*	get_region_children(parent_id)
*	
*	Returns the immediate children regions of the parent
*	region given by parameter parent_id.
*/
CREATE OR REPLACE FUNCTION get_region_children(parent_id integer)
RETURNS SETOF regions
AS
$body$
BEGIN
	RETURN QUERY
	SELECT
		rc.*
	FROM regions AS rp
	INNER JOIN regions AS rc ON (rc.left_bound BETWEEN rp.left_bound AND rp.right_bound)
	INNER JOIN region_depth AS rdp ON (rp.id = rdp.id)
	INNER JOIN region_depth AS rdc ON (rdc.id = rc.id)
	WHERE rp.id <> rc.id
	AND rdc.depth = (rdp.depth+1)
	AND rp.id = parent_id;
END;
$body$
LANGUAGE plpgsql