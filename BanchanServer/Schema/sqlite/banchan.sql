-- 반찬 마스터 테이블: 이름 템플릿
CREATE TABLE IF NOT EXISTS Banchan (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL UNIQUE CHECK (LENGTH(Name) > 0),
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 반찬 인스턴스 테이블: 실제 만든 반찬
CREATE TABLE IF NOT EXISTS BanchanInstance (
    Id TEXT PRIMARY KEY,
    BanchanId TEXT NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FinishedAt TEXT,
    RemainingPortion REAL NOT NULL DEFAULT 1 CHECK (RemainingPortion >= 0 AND RemainingPortion <= 1),
    Memo TEXT,
    FOREIGN KEY (BanchanId) REFERENCES Banchan(Id) ON DELETE CASCADE
);

-- 반찬 인스턴스 로그 테이블
CREATE TABLE IF NOT EXISTS BanchanInstanceLog (
    Id TEXT PRIMARY KEY,
    BanchanInstanceId TEXT NOT NULL,
    Type TEXT NOT NULL CHECK (Type IN ('Consumption', 'Disposal')),
    DetailJson TEXT NOT NULL,
    LoggedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (BanchanInstanceId) REFERENCES BanchanInstance(Id) ON DELETE CASCADE
);

-- 반찬 인스턴스 로그 테이블 트리거
CREATE TRIGGER IF NOT EXISTS prevent_multiple_discarded_logs
BEFORE INSERT ON BanchanInstanceLog
WHEN NEW.Type = 'Disposal'
BEGIN
  SELECT
    CASE
      WHEN EXISTS (
        SELECT 1 FROM BanchanInstanceLog
        WHERE BanchanInstanceId = NEW.BanchanInstanceId
          AND Type = 'Disposal'
      )
      THEN RAISE(ABORT, 'Disposal log already exists for this BanchanInstance')
    END;
END;
