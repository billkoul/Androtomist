CREATE TABLE A_FILE
(
	FILE_ID				SERIAL PRIMARY KEY, 
	FILE_NAME			VARCHAR(200),
	FILE_PATH			VARCHAR(200),
	FILE_RELATIVE_PATH	VARCHAR(200),
	FILEHASH			VARCHAR(200),
	FILETYPE			VARCHAR(5),
	FILE_LUD			TIMESTAMP(6),
	FILE_USER_ID		INTEGER,
	FILE_SIZE			INTEGER,
	FILE_LABEL			VARCHAR(200)
);

CREATE INDEX A_FILE_FILE_NAME_I ON A_FILE (FILE_NAME);
CREATE INDEX A_FILE_FILEHASH_I ON A_FILE (FILEHASH);
CREATE INDEX A_FILE_FILE_USER_ID_I ON A_FILE (FILE_USER_ID);