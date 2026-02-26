# LearnHub - Web-based Learning System

## Japanese

### 1. プロジェクト概要
LearnHub は、.NET（ASP.NET Core MVC）と SQLite を使った Web-based Learning System です。  
課題要件に沿って、会員登録・認証/認可・管理者CRUD・フォーム検証・HTML5/CSS・マルチメディアを実装しています。

### 2. 技術スタック
- ASP.NET Core MVC (.NET 10)
- ASP.NET Core Identity（会員/管理者認証）
- EF Core + SQLite
- Bootstrap + Razor Views

### 3. ロール別機能
- Guest
  - Home閲覧
  - Resource一覧/詳細閲覧
  - Register / Login
- Member（ログイン必須）
  - Member Dashboard
  - Bookmark追加/一覧/削除
- Admin（Adminロール必須）
  - AdminResources で Resource CRUD（Create/Read/Update/Delete）

### 4. 主要要件への対応
- DB接続と永続化: SQLite + EF Core
- CRUD（必須4操作）: Adminの Resource 管理画面から実行可能
- 生SQL実装: `FromSqlRaw`, `FromSqlInterpolated`, `ExecuteSqlInterpolatedAsync`
- 認証/認可: Identity + Roleベース制御
- フォーム検証: DataAnnotations + クライアント/サーバ検証
- HTML5/CSS: セマンティック要素 + 外部/内部/インラインCSS
- マルチメディア: Homeページの埋め込み動画

### 5. 実行方法（クイックスタート）
`LearnHub` ディレクトリで実行:

```bash
dotnet restore
dotnet build
dotnet run --urls http://localhost:5050
```

ブラウザ:
- `http://localhost:5050`

### 6. 初期管理者アカウント（SeedData）
- Email: `admin@learnhub.local`
- Password: `Admin123!`

定義場所:
- `Data/SeedData.cs`

### 7. データベース運用（今回の方針）
このリポジトリは **`app.db` を同梱**しています（Seedデータ込みでデモしやすくするため）。

再生成したい場合:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### 8. SQLクエリ説明
実装で使った SQL の説明は以下を参照:
- `SQL_QUERIES.md`

### 9. 検証証跡（Step 7）
- 受入検証レポート: `evidence/STEP7_ACCEPTANCE_REPORT.md`
- スクリーンショット: `evidence/*.png`

---

## English

### 1. Project Overview
LearnHub is a Web-based Learning System built with ASP.NET Core MVC and SQLite.  
It implements assignment-focused features: registration, authentication/authorization, admin CRUD, form validation, HTML5/CSS, and multimedia.

### 2. Tech Stack
- ASP.NET Core MVC (.NET 10)
- ASP.NET Core Identity (member/admin authentication)
- EF Core + SQLite
- Bootstrap + Razor Views

### 3. Role-based Features
- Guest
  - View Home
  - Browse Resource list/details
  - Register / Login
- Member (login required)
  - Member Dashboard
  - Add/list/remove bookmarks
- Admin (Admin role required)
  - Resource CRUD via AdminResources pages

### 4. Requirement Coverage
- Database connectivity/persistence: SQLite + EF Core
- Full CRUD: available via Admin Resource UI
- Raw SQL usage: `FromSqlRaw`, `FromSqlInterpolated`, `ExecuteSqlInterpolatedAsync`
- Auth/AuthZ: Identity with role-based access control
- Form validation: DataAnnotations + client/server validation
- HTML5/CSS: semantic tags + external/internal/inline CSS examples
- Multimedia: embedded video on Home page

### 5. Quick Start
Run from the `LearnHub` directory:

```bash
dotnet restore
dotnet build
dotnet run --urls http://localhost:5050
```

Open:
- `http://localhost:5050`

### 6. Default Admin Account (Seeded)
- Email: `admin@learnhub.local`
- Password: `Admin123!`

Defined in:
- `Data/SeedData.cs`

### 7. Database Policy for This Repository
This repository **includes `app.db`** (seeded) for easy demo/submission sharing.

If you want to regenerate DB:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### 8. SQL Query Documentation
See:
- `SQL_QUERIES.md`

### 9. Verification Evidence
- Acceptance report: `evidence/STEP7_ACCEPTANCE_REPORT.md`
- Screenshots: `evidence/*.png`
