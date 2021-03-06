CREATE TABLE B_SUBS
(
	SUB_ID			SERIAL PRIMARY KEY,
	SUB_FILE_ID		INTEGER,
	SUBS_SCHEMA_ID	INTEGER,
	SUBS_USER_ID	INTEGER,
	SUBS_LUD		TIMESTAMP(6),
	SUBS_LOG		VARCHAR(4000)
);

CREATE INDEX B_SUBS_SUB_FILE_ID_I ON B_SUBS (SUB_FILE_ID);
