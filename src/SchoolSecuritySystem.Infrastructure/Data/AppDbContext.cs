using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<department> departments { get; set; }

    public virtual DbSet<dispatch_log> dispatch_logs { get; set; }

    public virtual DbSet<dispatch_select> dispatch_selects { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<submission> submissions { get; set; }

    public virtual DbSet<submission_dispatch> submission_dispatches { get; set; }

    public virtual DbSet<submission_sequence> submission_sequences { get; set; }

    public virtual DbSet<submission_version> submission_versions { get; set; }

    public virtual DbSet<submission_workflow> submission_workflows { get; set; }

    public virtual DbSet<user_role> user_roles { get; set; }

    public virtual DbSet<pdf_password_log> pdf_password_logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<department>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("department")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.code, "UX_department_code").IsUnique();

            entity.Property(e => e.code).HasMaxLength(32);
            entity.Property(e => e.contact_email).HasMaxLength(64);
            entity.Property(e => e.name).HasMaxLength(64);
        });

        modelBuilder.Entity<dispatch_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("dispatch_log")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.dispatch_id, "FK_dispatchLog_dispatch");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.created_by).HasMaxLength(64);
            entity.Property(e => e.message).HasMaxLength(512);
            entity.Property(e => e.recipient_email).HasMaxLength(64);

            entity.HasOne(d => d.dispatch).WithMany(p => p.dispatch_logs)
                .HasForeignKey(d => d.dispatch_id)
                .HasConstraintName("FK_dispatchLog_dispatch");
        });

        modelBuilder.Entity<dispatch_select>(entity =>
        {
            entity.HasKey(e => new { e.dispatch_id, e.department_id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("dispatch_select")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.department_id, "FK_dispatchSelect_department");

            entity.HasOne(d => d.department).WithMany(p => p.dispatch_selects)
                .HasForeignKey(d => d.department_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dispatchSelect_department");

            entity.HasOne(d => d.dispatch).WithMany(p => p.dispatch_selects)
                .HasForeignKey(d => d.dispatch_id)
                .HasConstraintName("FK_dispatchSelect_dispatch");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("role")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.name, "UX_role_name").IsUnique();

            entity.Property(e => e.name).HasMaxLength(64);
            entity.Property(e => e.name_zh).HasMaxLength(64);
        });

        modelBuilder.Entity<submission>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("submission")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.created_by, "IX_submission_createdBy");

            entity.HasIndex(e => e.department_id, "IX_submission_departmentId");

            entity.HasIndex(e => e.is_deleted, "IX_submission_isDeleted");

            entity.HasIndex(e => e.status, "IX_submission_status");

            entity.HasIndex(e => e.trace_code, "UX_submission_traceCode").IsUnique();

            entity.Property(e => e.created_at).HasColumnType("datetime");
            entity.Property(e => e.created_by).HasMaxLength(64);
            entity.Property(e => e.is_deleted).HasDefaultValueSql("'1'");
            entity.Property(e => e.reporter_name).HasMaxLength(64);
            entity.Property(e => e.reporter_phone).HasMaxLength(32);
            entity.Property(e => e.title).HasMaxLength(100);
            entity.Property(e => e.trace_code).HasMaxLength(64);

            entity.HasOne(d => d.department).WithMany(p => p.submissions)
                .HasForeignKey(d => d.department_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_submission_department");
        });

        modelBuilder.Entity<submission_dispatch>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("submission_dispatch")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.submission_id, "IX_dispatch_submissionId");

            entity.Property(e => e.created_at).HasColumnType("datetime");
            entity.Property(e => e.created_by).HasMaxLength(64);
            entity.Property(e => e.director_sign).HasMaxLength(64);
            entity.Property(e => e.director_sign_at).HasColumnType("datetime");
            entity.Property(e => e.officer_sign).HasMaxLength(64);
            entity.Property(e => e.officer_sign_at).HasColumnType("datetime");

            entity.HasOne(d => d.submission).WithMany(p => p.submission_dispatches)
                .HasForeignKey(d => d.submission_id)
                .HasConstraintName("FK_dispatch_submission");
        });

        modelBuilder.Entity<submission_sequence>(entity =>
        {
            entity.HasKey(e => e.date_part).HasName("PRIMARY");

            entity
                .ToTable("submission_sequence")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.date_part).HasMaxLength(8);
        });

        modelBuilder.Entity<submission_version>(entity =>
        {
            entity.HasKey(e => new { e.submission_id, e.version })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("submission_version")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.created_at).HasColumnType("datetime");
            entity.Property(e => e.created_by).HasMaxLength(64);
            entity.Property(e => e.encrypted_dek).HasMaxLength(128);
            entity.Property(e => e.key_updated_at).HasColumnType("datetime");

            entity.HasOne(d => d.submission).WithMany(p => p.submission_versions)
                .HasForeignKey(d => d.submission_id)
                .HasConstraintName("FK_submissionVersions_submission");
        });

        modelBuilder.Entity<submission_workflow>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("submission_workflow")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.submission_id, "IX_submissionWorkflow_submissionId");

            entity.Property(e => e.comment).HasMaxLength(512);
            entity.Property(e => e.created_at).HasColumnType("datetime");
            entity.Property(e => e.created_by).HasMaxLength(64);

            entity.HasOne(d => d.submission).WithMany(p => p.submission_workflows)
                .HasForeignKey(d => d.submission_id)
                .HasConstraintName("FK_submissionWorkflow_submission");
        });

        modelBuilder.Entity<user_role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("user_role")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.department_id, "FK_admin_department");

            entity.HasIndex(e => e.role_id, "FK_admin_role");

            entity.HasIndex(e => new { e.email, e.role_id }, "UX_userRole_email_dept_role").IsUnique();

            entity.Property(e => e.email).HasMaxLength(64);

            entity.HasOne(d => d.department).WithMany(p => p.user_roles)
                .HasForeignKey(d => d.department_id)
                .HasConstraintName("FK_admin_department");

            entity.HasOne(d => d.role).WithMany(p => p.user_roles)
                .HasForeignKey(d => d.role_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_admin_role");
        });

        modelBuilder.Entity<pdf_password_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("pdf_password_log")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.created_at, "IX_pdfPasswordLog_createdAt");

            entity.Property(e => e.password).HasMaxLength(128);
            entity.Property(e => e.created_by).HasMaxLength(64);
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
