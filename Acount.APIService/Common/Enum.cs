using System.ComponentModel;

namespace Acount.APIService.Common
{

	public enum ResultCodes
	{
		R_SUCCESS = 0,
		R_DB_ERROR = 1,
		R_SERVICE_GET_DATA_ERROR = 2,
		R_SERVICE_POST_DATA_ERROR = 3,
		R_NO_DATA_FOUND = 4,
		R_AUTHENTICATION_FAILED = 5,
		R_UNAUTHORIZED = 6,
		R_UNKNOWN = 7,
		R_INVALID_LOGIN_ID = 8,
		R_INVALID_PASSWORD = 9,
		R_SERVICE_ERROR = 10,
		R_SERVICE_UNAVAILABLE = 11,
		R_INVALID_REQUEST = 12,
		R_INVALID_NUMBER = 13,
		R_NOT_FOUND = 14,
		R_NETWORK_ERROR_OR_SERVER_ERROR = 15,
		R_CREATED = 16,
		R_INVALID_SSO_PASSWORD = 17,
		R_NOT_AVAILABLE = 18,
		R_INTERNAL_SERVER_ERROR = 19,
		R_UNUSED = 20,
		R_MULTIPLE_RECORDS = 21,
		R_BAD_REQUEST = 22,
		R_USER_NOT_ACTIVE = 23,
		R_USER_VALIDITY_EXPIRED = 24,
		R_DUPLICATE = 25,
		R_FUNCTIONAL_ERROR = 26,
		R_MAIL_ERROR = 27,
		R_TENENT_NOT_FOUND = 28,
		R_OUT_OF_SCHEDULE = 30,
		R_DEVICE_IN_USE = 31,
		R_ALREADY_LOGGED_IN = 32,
		R_NOT_LOGGED_IN = 33,
		R_NO_ACCESS_ON_INSTALLATION = 34,
		R_ASSET_INFO_NOT_FOUND = 35,
		R_VALUE_BEYOND_LIMIT = 36,
		R_RECORD_NOT_EXIST = 37,
		R_CONFIGURATION_MISSING = 38,
		R_CONFLICT_OCCURRED = 39,
		R_USER_HAS_ACTIVE_INSTALLATION = 40,
		R_METHOD_NOT_ALLOWED = 41,
		R_FIELD_BLANK = 42,
		R_USER_NOT_FOUND = 43,
		R_REQUIRED = 44
	}

	public enum HTTPResponseCodes
	{
		R_UNAUTHORIZED = 401
	}
	public enum LoginMode
	{
		Password = 1,
		OTP = 2,
	}
	public enum RequestSource
	{
		Web = 1,
		Mobile = 2,
		Gateway = 3
	}
	public enum OTPStatus
	{
		Active = 1,
		Inactive = 2,
	}
	public enum UserType
	{
		[Description("SUPERADMIN")]
		SUPERADMIN = 1,
		[Description("ADMIN")]
		ADMIN = 2,
		[Description("AUDITOR")]
		AUDITOR = 3,
		[Description("SUPERVISOR")]
		SUPERVISOR = 4
	}
	public enum DataTypes
	{
		[Description("DATANUMERIC")]
		NUMERIC = 30,
		[Description("DATAFLOAT")]
		FLOAT = 31,
		[Description("DATADATE")]
		DATE = 32,
		[Description("DATASTRING")]
		STRING = 29,
		[Description("DATADATETIME")]
		DATETIME = 34,
		[Description("DATABOOLEAN")]
		BOOLEAN = 35,
		[Description("DATATIME")]
		TIME = 33
	}
	public enum TagTypes
	{
		[Description("TEXTAREA")]
		TEXTAREA = 55,
		[Description("LABEL")]
		LABEL = 36,
		[Description("DATEPICKER")]
		DATEPICKER = 24,
		[Description("TIMESELECTOR")]
		TIMESELECTOR = 25,
		[Description("INPUTFIELD")]
		INPUTFIELD = 22,
		[Description("RADIOBUTTON")]
		RADIOBUTTON = 27,
		[Description("CHECKBOX")]
		CHECKBOX = 28,
		[Description("DATETIMESELECTOR")]
		DATETIMESELECTOR = 26,
		[Description("DROPDOWN")]
		DROPDOWN = 23
	}
	public enum UserRole
	{
		SuperAdmin = 1,
		Admin = 5,
		Employee = 4,
		Default = 6
	}
	public enum Status
	{
		Draft = 1,
		PendingForApproval = 2,
		Finished = 3,
		Revert = 4
		//Submitted = 2,
		//Approved = 3,
	}
	public enum ReportStatus
	{
		[Description("DRAFT")]
		DRAFT = 58,
		[Description("SUBMITTED")]
		SUBMITTED = 61,
		[Description("APPROVED")]
		APPROVED = 67,
		[Description("REVIEWED")]
		REVIEWED = 64
	}
	public enum DocType
	{
		[Description(".jpeg")]
		JPEG = 1,
		[Description(".png")]
		PNG = 2,
		[Description(".pdf")]
		PDF = 3,
		[Description(".jpg")]
		JPG = 4,
		[Description(".doc")]
		DOC = 5,
		[Description(".docx")]
		DOCX = 6,
		[Description(".txt")]
		TXT = 7,
		[Description(".xlsx")]
		XLSX = 8,
		[Description(".xls")]
		XLS = 9,
	}
	public enum TempTagTypes
	{
		[Description("SECTION")]
		SECTION = 1,
		[Description("GROUP")]
		GROUP = 2,
		[Description("LABEL")]
		LABEL = 3,
		[Description("TAGS")]
		TAGS = 4,
		[Description("LABELTAGS")]
		LABELTAGS = 5,
	}
	public enum CunsumableType
	{
		[Description("SHIFT")]
		SHIFT = 1,
		[Description("CONSUMABLE")]
		CONSUMABLE = 2,
		[Description("PRODUCT")]
		PRODUCT = 3,
	}

	public enum FrequencyType
	{
		[Description("DAILY")]
		DAILY = 1,
		[Description("MONTHLY")]
		MONTHLY = 2,
		[Description("YEARLY")]
		YEARLY = 3,
		[Description("SHIFT")]
		SHIFT = 4,
		[Description("WEEKLY")]
		WEEKLY = 5,
	}
}