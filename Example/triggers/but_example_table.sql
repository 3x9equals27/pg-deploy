DROP TRIGGER IF EXISTS but_example_table ON public.example_table;

CREATE TRIGGER but_example_table
BEFORE UPDATE ON example_table
FOR EACH ROW
EXECUTE PROCEDURE fn_trigger_set_timestamp();
