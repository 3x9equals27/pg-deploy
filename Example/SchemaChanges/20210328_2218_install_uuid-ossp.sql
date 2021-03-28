DO $$
DECLARE v_script_name VARCHAR := '20210328_2218_install_uuid-ossp.sql';
BEGIN
--
IF NOT EXISTS (SELECT FROM _provision WHERE script_name = v_script_name) THEN

	CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
    
    INSERT INTO _provision(script_name) VALUES(v_script_name);
END IF;
--    
END;
$$ LANGUAGE plpgsql;
