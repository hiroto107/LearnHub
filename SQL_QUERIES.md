# SQL Queries and Explanations

このプロジェクトでは、EF Coreを使いつつ、課題要件に合わせて生SQLも実装しています。  
実運用コードでは、`FromSqlInterpolated` / `ExecuteSqlInterpolatedAsync` を使い、SQLインジェクションを避けるためにパラメータ化しています。

## 1) CREATE TABLE（DDL）

SQLite上で `Resources` テーブルを作成するSQL（マイグレーションで適用）:

```sql
CREATE TABLE "Resources" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Resources" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Url" TEXT NOT NULL,
    "MediaType" TEXT NOT NULL,
    "Level" INTEGER NOT NULL,
    "CategoryId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Resources_Categories_CategoryId"
        FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);
```

- 目的: 学習リソースの永続化
- 主な制約: 主キー、外部キー、NOT NULL

## 2) SELECT（Read）

実装で使用している一覧取得SQL:

```sql
SELECT * FROM Resources;
```

単一レコード取得SQL:

```sql
SELECT * FROM Resources WHERE Id = @id;
```

- 目的: 一覧表示・詳細表示
- 使用箇所: `ResourcesController`, `AdminResourcesController`

## 3) INSERT（Create）

実装で使用している登録SQL:

```sql
INSERT INTO Resources
(Title, Description, Url, MediaType, Level, CategoryId, CreatedAt, UpdatedAt)
VALUES
(@title, @description, @url, @mediaType, @level, @categoryId, @createdAt, @updatedAt);
```

- 目的: 管理者による新規リソース作成
- 使用箇所: `AdminResourcesController.Create (POST)`

## 4) UPDATE（Update）

実装で使用している更新SQL:

```sql
UPDATE Resources
SET Title = @title,
    Description = @description,
    Url = @url,
    MediaType = @mediaType,
    Level = @level,
    CategoryId = @categoryId,
    UpdatedAt = @updatedAt
WHERE Id = @id;
```

- 目的: 管理者による既存リソース編集
- 使用箇所: `AdminResourcesController.Edit (POST)`

## 5) DELETE（Delete）

実装で使用している削除SQL:

```sql
DELETE FROM Resources WHERE Id = @id;
```

- 目的: 管理者によるリソース削除
- 使用箇所: `AdminResourcesController.DeleteConfirmed (POST)`

## 6) Member Module SQL（Bookmark）

ブックマーク追加:

```sql
INSERT INTO Bookmarks (UserId, ResourceId, CreatedAt)
VALUES (@userId, @resourceId, @createdAt);
```

ブックマーク削除:

```sql
DELETE FROM Bookmarks WHERE Id = @id AND UserId = @userId;
```

ブックマーク取得:

```sql
SELECT * FROM Bookmarks WHERE UserId = @userId;
```

- 目的: ログイン会員の活動管理（要件FR-40）
- 使用箇所: `MemberController`

## 7) 安全性（SQL Injection対策）

- 生SQLは `FromSqlInterpolated` / `ExecuteSqlInterpolatedAsync` で実行
- 文字列連結でSQLを作らず、パラメータ化して実行
