CREATE OR REPLACE FUNCTION delete_region(id INTEGER, delete_children BOOLEAN)
RETURNS SETOF regions
AS
$body$
BEGIN

	IF(delete_children) THEN
		WITH deletes AS (
			SELECT
				rc.id
			FROM regions AS rp
			JOIN regions AS rc ON rc.left_bound BETWEEN rp.left_bound AND rp.right_bound
			WHERE rp.id = delete_region.id)
		DELETE FROM regions AS rd
		WHERE rd.id IN(SELECT ds.id FROM deletes ds);
		
		RETURN;
		
	ELSE
	
		RETURN QUERY
		WITH child_regions AS (
			SELECT * FROM get_region_children(delete_region.id)
		), deleted_regions AS (
			DELETE FROM regions AS r
			WHERE r.id = delete_region.id
		)
		SELECT * FROM child_regions; 
	
	END IF;
	
END;
$body$
LANGUAGE plpgsql