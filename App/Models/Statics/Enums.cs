using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum FILE_TYPES
{
    other,
    csv,
    txt,
    xls,
    dbf,
    xml,
    mdb,
    xlsx,
    xlsb,
    xlsm,
    accdb,
    zip,
	jpg,
    apk
}

public enum TABLE_TYPE
{
    NONE = 0,
    PROCESS = 1,
    USER = 2,
    RESULTS = 3,
	SUBMISSION =4,
    SCHEMA = 5
}

public enum PROCESS_TYPE
{
    STATIC = 0,
    DYNAMIC = 1,
    HYBRID = 2,
    TRAINING = 3,
}


public enum INPUT
{
    //file
	SUB_ID,
    P_NAME,
    P_TYPE_ID,
    P_FILE_ID,
    FILE_LABEL
}


public enum OPTIONS
{
    BASE_PATH,
    MAP_SERVICE
}

public enum USER_LEVEL
{
    ADMIN = 0,
    POWER_USER = 1,
    USER = 2
}

public enum CHANGE_TYPE
{
    NONE = 0,
    GROWTHRATE = 1,
    EXCLUSION = 2,
    RETIREMENT = 3,
    REPLACEMENT = 4
}

public enum MDB_TYPE
{
    TABLES,
    VIEWS
}

public enum AGGREGATES
{
    SUM = 0,
    AVG = 1,
    MIN = 2,
    MAX = 3,
    COUNT = 4,
    STDEV = 5,
    VAR = 6,
}

[JsonConverter(typeof(StringEnumConverter))]
public enum FONT_WEIGHT
{
    bold = 1,
    normal = 2
}

[JsonConverter(typeof(StringEnumConverter))]
public enum CURSOR_TYPE
{
    normal = 1,
    pointer = 2
}

[JsonConverter(typeof(StringEnumConverter))]
public enum SERIE_TYPE
{
    scatter = 0,
    line = 1,
    boxplot = 2,
    bar = 3,
    errorbar = 4,
    pie = 5
}

[JsonConverter(typeof(StringEnumConverter))]
public enum MARKER_SYMBOL
{
    circle = 1,
    square = 2,
    diamond = 3,
    triangle = 4
}

[JsonConverter(typeof(StringEnumConverter))]
public enum HOR_ALIGN
{
    left = 1,
    center = 2,
    right = 3
}

[JsonConverter(typeof(StringEnumConverter))]
public enum VER_ALIGN
{
    top = 1,
    middle = 2,
    bottom = 3
}

[JsonConverter(typeof(StringEnumConverter))]
public enum LEGEND_LAYOUT
{
    horizontal = 1,
    vertical = 2,
    proximate = 3
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ZOOM_TYPE
{

    xy = 1,
    x = 2,
    y = 3
}

[JsonConverter(typeof(StringEnumConverter))]
public enum PAN_KEY
{
    shift = 1,
    alt = 2,
    ctrl = 3,
    meta = 4
}


public enum MENU_CAT
{
    admin = 1,
    user = 2
}