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

-- 반찬 소비 기록
CREATE TABLE IF NOT EXISTS ConsumptionLog (
    Id TEXT PRIMARY KEY,
    BanchanInstanceId TEXT NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Portion REAL NOT NULL CHECK (Portion > 0 AND Portion <= 1),
    Memo TEXT,
    FOREIGN KEY (BanchanInstanceId) REFERENCES BanchanInstance(Id) ON DELETE CASCADE
);

-- 반찬 폐기 기록
CREATE TABLE IF NOT EXISTS DisposalLog (
    Id TEXT PRIMARY KEY,
    BanchanInstanceId TEXT NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Reason TEXT,
    FOREIGN KEY (BanchanInstanceId) REFERENCES BanchanInstance(Id) ON DELETE CASCADE,
    UNIQUE (BanchanInstanceId)
);