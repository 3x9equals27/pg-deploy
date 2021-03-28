DO $$
-- Filename goes here
DECLARE v_script_name VARCHAR := 'YYYYMMDD_HHMM_change.sql';
BEGIN
--
IF NOT EXISTS (SELECT FROM _provision WHERE script_name = v_script_name) THEN
    
	-- Changes go here
    
    INSERT INTO _provision(script_name) VALUES(v_script_name);
END IF;
--    
END;
$$ LANGUAGE plpgsql;
