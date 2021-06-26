외부모듈:
	pip install pyinstaller
	pip install xlrd

규칙:
	ROWTYPE은 A셀에 입력. 행 순서나 여백은 상관 없음.
	ROWTYPE:NAME이 있는 필드만 출력.
	ROWTYPE:EXPORT에 포함되는 설정값의 필드만 출력.

버전:
	0.0.3
		CS에 Field(Tuple) List 추가
	0.0.2
		CS 작성 수정 (\n 누락 추가) + make_enum_fields 추가
	0.0.1
		기본기능 작성

사용법:
	tableexporter "<불러들일 XLSX 파일의 경로>" "<JSON 출력 경로>" "<CS 출력 경로>" "makeenumfield1;makeenumfield2;..."

기본설명:
	XLSX 내의 SHEET(탭) 마다 1개의 테이블로 출력.
	좌측 A열의 값을 종류로 구분하여 해석
	NAME, COMMENT, TYPE, DATA 로 구분됨

기능:
	# 기본 익스포트 기능 (XLSX ==> JSON+CS)
	# 로컬라이징매니저 연동 기능 (LocalizationID) ==> StringTable을 사용하는 전제하에 코드로만 연동됨. 엑셀문서의 구분은 기획 소관.
	# C#의 모든 기본 이뮤터블 타입을 다음의 4가지 타입으로 치환하여 지원 (number, text, bool, real)
	# 비어있는 셀을 기본값으로 처리 (0 or 0.0 or false or "")
	# c#에서 사용하는 키워드로 필드명 검사
	# 텍스트의 개행은 셀의 실제 개행값으로 처리
	# 배열 지원 (스트링 제외)
	# 열거체 클래스 생성 기능 : 지정한 필드의 모든 값을 enum 클래스 코드로 생성 (enum TableName_FileName) 단, 모든 값은 중복되어선 안됨. (중복될 경우 코드 에러남)
	# 열거체 자료형 지원 (EnumValue) ==> 문자열타입으로 제공.

확장자료형:
	LocalizationID
		해당 자료형을 입력하면 스트링테이블의 ID 로 코드를 자동 연결하여 ToString() 호출시 해당하는 로컬라이징 시스템의 문자열값을 반환.

처리순서:
	load .xlsx
	create t_sheet_info[]
	create t_table_info[]
	create t_csharpcode from t_table_info[]
	check name to keyword
	check diff type value
	modify default type value
	write .cs
	write .csv
	write .json