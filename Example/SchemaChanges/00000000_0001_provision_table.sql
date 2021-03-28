DO $$
DECLARE v_script_name VARCHAR := '00000000_0001_provision_table.sql';
BEGIN
--
IF to_regclass('public._provision') IS NULL THEN
	-- DROP TABLE _provision;
	CREATE TABLE _provision(
	  script_name VARCHAR PRIMARY KEY
	, provisioned TIMESTAMPTZ NOT NULL DEFAULT NOW()
	);
	
	INSERT INTO _provision(script_name) VALUES(v_script_name);
END IF;
--    
END;
$$ LANGUAGE plpgsql;
