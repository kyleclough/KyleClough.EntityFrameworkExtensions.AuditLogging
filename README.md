# KyleClough.EntityFrameworkExtensions.AuditLogging

> Audit trails made simple for EF Core.

A lightweight, provider‑agnostic audit logging extension for Entity Framework Core.  
Tracks **Added**, **Modified**, **Deleted**, and **SoftDeleted** changes with before/after values, actor metadata, correlation IDs, and IP addresses.  
Includes plug‑and‑play `DbContext` extensions, fluent configuration, and multi‑provider support (SQL Server, PostgreSQL, MySQL).

---

## ✨ Features
- Automatic audit trail entries for all entity changes
- Records **before/after values** per property
- Supports **soft deletes** (`DeletedAt`, `DeletedBy`)
- Captures **actor metadata** (SubjectId, Actor, IP address)
- Groups changes with **CorrelationId**
- Provider‑agnostic migrations (SQL Server, PostgreSQL, MySQL)
- Clean architecture folder structure (`Entities`, `Configurations`, `DbContext`, `Extensions`)

---

## 📦 Installation

Add the package via NuGet:

```powershell
dotnet add package KyleClough.EntityFrameworkExtensions.AuditLogging
```

For PostgreSQL support:

```powershell
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

---

## 🚀 Usage

### 1. Configure `AuditDbContext`
```csharp
services.AddDbContext<AuditDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("AuditDb")));
```

### 2. Apply Configuration
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
}
```

### 3. Save Changes with Audit
```csharp
await context.SaveChangesWithAuditAsync(
    subjectId: "user-123",
    actor: "admin",
    correlationId: Guid.NewGuid().ToString(),
    ipAddress: "127.0.0.1"
);
```

---

## 🧪 Example AuditLog Entry

| Id | Action   | EntityName | EntityId | PropertyName | BeforeValue | AfterValue | Actor | SubjectId | CorrelationId | TimestampUtc |
|----|----------|------------|----------|--------------|-------------|------------|-------|-----------|---------------|--------------|
| 1  | Modified | User       | 42       | Name         | Kyle        | Kyle Updated | admin | user-123  | corr-001      | 2026‑01‑08   |

---

## 🧪 Testing

Use EF Core’s InMemory provider for unit tests:

```csharp
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseInMemoryDatabase("AuditTestDb")
    .Options;

using var context = new TestDbContext(options);

// Act
entity.Name = "Updated";
await context.SaveChangesWithAuditAsync("user-123", "admin", "corr-001");

// Assert
var audit = context.AuditLogs.First(a => a.PropertyName == "Name");
Assert.Equal("Updated", audit.AfterValue);
```

---

## 📂 Folder Structure

```
AuditLogging/
├── Entities/
│   └── AuditLog.cs
├── Configurations/
│   └── AuditLogConfiguration.cs
├── DbContext/
│   ├── AuditDbContext.cs
│   └── AuditDbContextFactory.cs
├── Extensions/
│   └── DbContextAuditExtensions.cs
├── Migrations/
└── README.md
```

---

## 📜 License
MIT License © Kyle Clough
```