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
	tableexporter "<절대경로로 된 XLSX 위치>" "<json 출력 경로>" "<CS 출력 경로>" "makeenumfield1;makeenumfield2;..."

기본설명:
	XLSX 내의 SHEET(탭) 마다 1개의 테이블로 출력.
	좌측 A열의 값을 종류로 구분하여 해석
	NAME, COMMENT, TYPE, DATA 로 구분됨

기능:
	# 기본 익스포트 기능 (XLSX ==> JSON+CS)
	# 로컬라이징매니저 연동 기능 (locale_id, locale_key) ==> StringTable을 사용하는 전제하에 코드로만 연동됨. 엑셀문서의 구분은 기획 소관.
	# C#의 모든 기본 이뮤터블 타입을 다음의 4가지 타입으로 치환하여 지원 (number, text, bool, real)
	# 비어있는 셀을 기본값으로 처리 (0 or 0.0 or false or "")
	# c#에서 사용하는 키워드로 필드명 검사
	# 텍스트의 개행은 셀의 실제 개행값으로 처리
	# 배열 지원 (스트링 제외)
	# 열거체 클래스 생성 기능 : 지정한 필드의 모든 값을 enum 클래스 코드로 생성 (enum TableName_FileName) 단, 모든 값은 중복되어선 안됨. (중복될 경우 코드 에러남)
	# 열거체 자료형 지원 (LocalizationID) ==> 문자열타입으로 제공.

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


기능:
	일반테이블 (#) 1차원 구조체 형식으로 TableName.cs, TableName.json 으로 뽑혀 나온다.
	열거테이블 (@) NAME, VALUE 를 통해 구분되며 export 시 eTableName.cs 로 뽑혀나온다.
	병합테이블 (&) 동일한 자료구조를 지닌 모든 시트가 하나의 테이블로 병합되어 뽑혀나온다.

테이블 규칙:
	ROWTYPE:
		해당 시트의 첫번째 컬럼(A)를 의미한다.
		해당 행이 테이블 구조에 유의미한 대상임을 정의하고 해당 이름에 따라 처리한다.
		대소문자는 구분하지 않으며 정해진 이름만을 사용한다.
		공백이면 유의미한 행으로 인식되지 않는다.
		NAME, TYPE, EXPORT은 반드시 존재해야한다.
		KEY, COMMENT, LOCALIZE, REF는 없어도 무방하다.

	NAME:
		실제 코드상에서 정의되는 필드의 이름.
		대소문자 구분하고 숫자가 문자보다 먼저 와서는 안되며 언더바 제외 띄어쓰기, 탭, 개행, 특수문자 안됨.
		NAME이 존재하는 컬럼을 필드로 규정한다. (즉, NAME이 공백이면 필드로 인식되지 않는다.)

	TYPE:
		실제 코드상에서 정의되는 필드의 타입.
		익스포트되는 코드에서 제공되는 이름과 동일해야 한다.
		배열, 열거체, 구조체, 클래스 등 immutable 하지 않거나, 자료구조인 것은 인정되지 않는다.
		
		논리형 : b8
		문자형 : text
		정수형 : s8, s16, s32, s64, u8, u16, u32, u64
		실수형 : f32, f64
		
		개선안)
			immutable:text 로 구분짓는다.
				enum:eGroupType : 이넘임을 구분 (--exportecs 을 통해 해당 테이블에서 사용하는 모든 enum을 cs로 저장)
				object:Vector2 : 쉼표와 괄호로 내부 멤버를 구분
				array:s32 : 쉼표로 구분



	EXPORT:
		상황별로 셋팅하는데 주로 C (클라이언트) 와 S (서버), CS(클라이언트/서버) 로 구분한다.
		실제 xtable2 을 사용할 때 지정한 export_target이 바로 C, S, CS의 문자열이 된다.
		만일 시트에 EXPORT 자체가 존재하지 않으면 아무것도 저장하지 않는다.

	COMMENT:
		해당 필드에 대한 설명.
		이는 C# 코드에도 인텔리센스로 동작하기 위해 작성된다.
		없어도 무방하다.

	KEY:
		입력값예시) true

		입력값예시) SampleTable, SampleTable:Value, 공백(사용하지않음)
		해당 시트를 로드할때 키값으로 쓰일 필드로 명명한다.
		키는 어떠한 타입이어도 상관없지만 immutable 해야 하며, 고유식별자이므로 중복되면 안된다.
		없어도 무방하다.

	LOCALIZE:
		입력값예시) true, false, 공백(=false)

		REF 기능을 활용함에 있어 로컬라이징을 위함을 표현한다.
		REF 셋팅이 없다면(공백이거나 없음) 코드에서 정의한 LocalizeTable의 ID와 연결된다. TableManager.DefualtLocalizeTable = TargetTable
		즉, DefaultLocalizeTable.ID 가 된다.
		적용된 테이블은 public string Localize_{FieldName}() { ... } 식의 별도 함수가 정의 된다.
