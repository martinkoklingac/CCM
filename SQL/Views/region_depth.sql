CREATE OR REPLACE VIEW region_depth
AS
SELECT
	c.id,
	COUNT(p.id)-1 AS depth
FROM
	regions p,
	regions c
WHERE c.left_bound BETWEEN p.left_bound AND p.right_bound
GROUP BY c.id;

ALTER VIEW region_depth OWNER TO "CCMAdmin";
