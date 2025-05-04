using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Repository.Models;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HiteshTaskAssignTask> HiteshTaskAssignTasks { get; set; }

    public virtual DbSet<HiteshTaskDocumentSave> HiteshTaskDocumentSaves { get; set; }

    public virtual DbSet<HiteshTaskMessage> HiteshTaskMessages { get; set; }

    public virtual DbSet<HiteshTaskProject> HiteshTaskProjects { get; set; }

    public virtual DbSet<HiteshTaskProjectAccess> HiteshTaskProjectAccesses { get; set; }

    public virtual DbSet<HiteshTaskRole> HiteshTaskRoles { get; set; }

    public virtual DbSet<HiteshTaskUserMaster> HiteshTaskUserMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=172.16.0.241;Database=TestDB;User Id=traininguser;Password=Admin#24;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HiteshTaskAssignTask>(entity =>
        {
            entity.ToTable("HiteshTaskAssignTask");

            entity.Property(e => e.AssignTo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectId).HasColumnName("Project_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HiteshTaskAssignTaskCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Crated_Id");

            entity.HasOne(d => d.Project).WithMany(p => p.HiteshTaskAssignTasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("p_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HiteshTaskAssignTaskUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Updated_Id");
        });

        modelBuilder.Entity<HiteshTaskDocumentSave>(entity =>
        {
            entity.ToTable("HiteshTaskDocumentSave");

            entity.Property(e => e.DocumentName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectId).HasColumnName("Project_Id");
            entity.Property(e => e.TaskId).HasColumnName("Task_Id");

            entity.HasOne(d => d.Project).WithMany(p => p.HiteshTaskDocumentSaves)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("ProjectId");

            entity.HasOne(d => d.Task).WithMany(p => p.HiteshTaskDocumentSaves)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_HiteshTaskDocumentSave_HiteshTaskAssignTask");
        });

        modelBuilder.Entity<HiteshTaskMessage>(entity =>
        {
            entity.ToTable("HiteshTaskMessage");

            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ReceiveId).HasColumnName("Receive_Id");
            entity.Property(e => e.SendId).HasColumnName("Send_Id");
            entity.Property(e => e.TaskId).HasColumnName("Task_Id");

            entity.HasOne(d => d.Task).WithMany(p => p.HiteshTaskMessages)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_ID");
        });

        modelBuilder.Entity<HiteshTaskProject>(entity =>
        {
            entity.ToTable("HiteshTaskProject");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HiteshTaskProjectCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Created_Id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HiteshTaskProjectUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Upd_Id");
        });

        modelBuilder.Entity<HiteshTaskProjectAccess>(entity =>
        {
            entity.ToTable("HiteshTaskProjectAccess");

            entity.Property(e => e.ProjectId).HasColumnName("Project_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Project).WithMany(p => p.HiteshTaskProjectAccesses)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Project_Id");

            entity.HasOne(d => d.User).WithMany(p => p.HiteshTaskProjectAccesses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Userid");
        });

        modelBuilder.Entity<HiteshTaskRole>(entity =>
        {
            entity.ToTable("HiteshTaskRole");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<HiteshTaskUserMaster>(entity =>
        {
            entity.ToTable("HiteshTaskUserMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.HiteshTaskUserMasters)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Roleid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
