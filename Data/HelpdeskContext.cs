using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HelpDesk.Data
{
    public partial class HelpdeskContext : DbContext
    {
        public HelpdeskContext()
        {
        }

        public HelpdeskContext(DbContextOptions<HelpdeskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accesses> Accesses { get; set; }
        public virtual DbSet<Authentication> Authentication { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Partitions> Partitions { get; set; }
        public virtual DbSet<PerformersOfRequests> PerformersOfRequests { get; set; }
        public virtual DbSet<PerformersOfTasks> PerformersOfTasks { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ServerRoles> ServerRoles { get; set; }
        public virtual DbSet<StatusOfRequests> StatusOfRequests { get; set; }
        public virtual DbSet<Statuses> Statuses { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Visits> Visits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=SQLOLEDB.1;Password=PaZZw0rd;Persist Security Info=True;User ID=application;Initial Catalog=Helpdesk;Data Source=LAPTOP-H45JKRTV");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:DefaultSchema", "db_datawriter");

            modelBuilder.Entity<Accesses>(entity =>
            {
                entity.HasKey(e => new { e.IdRole, e.IdPartition })
                    .HasName("PK__Accesses__CC17C395A3FE57BC");

                entity.ToTable("Accesses", "dbo");

                entity.Property(e => e.IdRole).HasColumnName("ID_role");

                entity.Property(e => e.IdPartition).HasColumnName("ID_partition");

                entity.HasOne(d => d.IdPartitionNavigation)
                    .WithMany(p => p.Accesses)
                    .HasForeignKey(d => d.IdPartition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accesses_Partitions");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Accesses)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accesses_Roles");
            });

            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__Authenti__D7B4671ECFDD5C03");

                entity.ToTable("Authentication", "dbo");

                entity.Property(e => e.IdUser)
                    .HasColumnName("ID_user")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("User_password")
                    .HasMaxLength(150);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithOne(p => p.Authentication)
                    .HasForeignKey<Authentication>(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Authentication_Users");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => new { e.AddTime, e.IdUser })
                    .HasName("PK__Comments__A301D0E851115B6A");

                entity.ToTable("Comments", "dbo");

                entity.Property(e => e.AddTime)
                    .HasColumnName("Add_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasColumnName("ID_user");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.IdRequest).HasColumnName("ID_request");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Requests");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Users");
            });

            modelBuilder.Entity<Partitions>(entity =>
            {
                entity.ToTable("Partitions", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ForRequest).HasColumnName("For_request");

                entity.Property(e => e.IdSuperPartition).HasColumnName("ID_Super_Partition");

                entity.Property(e => e.Notation)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.IdSuperPartitionNavigation)
                    .WithMany(p => p.InverseIdSuperPartitionNavigation)
                    .HasForeignKey(d => d.IdSuperPartition)
                    .HasConstraintName("FK_Partitions_Partitions");
            });

            modelBuilder.Entity<PerformersOfRequests>(entity =>
            {
                entity.HasKey(e => new { e.IdRequest, e.IdPerformer })
                    .HasName("PK__Performe__98297CB383A529BA");

                entity.ToTable("Performers_of_requests", "dbo");

                entity.Property(e => e.IdRequest).HasColumnName("ID_request");

                entity.Property(e => e.IdPerformer).HasColumnName("ID_performer");

                entity.HasOne(d => d.IdPerformerNavigation)
                    .WithMany(p => p.PerformersOfRequests)
                    .HasForeignKey(d => d.IdPerformer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performers_of_requests_Users");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.PerformersOfRequests)
                    .HasForeignKey(d => d.IdRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performers_of_requests_Requests");
            });

            modelBuilder.Entity<PerformersOfTasks>(entity =>
            {
                entity.HasKey(e => new { e.IdTask, e.IdPerformer })
                    .HasName("PK__Performe__841D33DC07B7E29C");

                entity.ToTable("Performers_of_tasks", "dbo");

                entity.Property(e => e.IdTask).HasColumnName("ID_task");

                entity.Property(e => e.IdPerformer).HasColumnName("ID_performer");

                entity.HasOne(d => d.IdPerformerNavigation)
                    .WithMany(p => p.PerformersOfTasks)
                    .HasForeignKey(d => d.IdPerformer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performers_of_tasks_Users");

                entity.HasOne(d => d.IdTaskNavigation)
                    .WithMany(p => p.PerformersOfTasks)
                    .HasForeignKey(d => d.IdTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performers_of_tasks_Tasks");
            });

            modelBuilder.Entity<Requests>(entity =>
            {
                entity.ToTable("Requests", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationTime)
                    .HasColumnName("Creation_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Heading)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.IdCreator).HasColumnName("ID_creator");

                entity.Property(e => e.IdPartition).HasColumnName("ID_partition");

                entity.HasOne(d => d.IdCreatorNavigation)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.IdCreator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requests_Users");

                entity.HasOne(d => d.IdPartitionNavigation)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.IdPartition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requests_Partitions");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("Roles", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Notation)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ServerRoles>(entity =>
            {
                entity.HasKey(e => e.ServerRole)
                    .HasName("PK__Server_r__0C2001B94A07A554");

                entity.ToTable("Server_roles", "dbo");

                entity.Property(e => e.ServerRole)
                    .HasColumnName("Server_role")
                    .HasMaxLength(150)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<StatusOfRequests>(entity =>
            {
                entity.HasKey(e => new { e.ChangeTime, e.IdRequest })
                    .HasName("PK__Status_o__3C78E0235DE0F044");

                entity.ToTable("Status_of_requests", "dbo");

                entity.Property(e => e.ChangeTime)
                    .HasColumnName("Change_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdRequest).HasColumnName("ID_request");

                entity.Property(e => e.IdStatus).HasColumnName("ID_status");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.StatusOfRequests)
                    .HasForeignKey(d => d.IdRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Status_of_requests_Requests");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.StatusOfRequests)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Status_of_requests_Statuses");
            });

            modelBuilder.Entity<Statuses>(entity =>
            {
                entity.ToTable("Statuses", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdPartition).HasColumnName("ID_partition");

                entity.Property(e => e.Notation)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.IdPartitionNavigation)
                    .WithMany(p => p.Statuses)
                    .HasForeignKey(d => d.IdPartition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Statuses_Partitions");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.ToTable("Tasks", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddTime)
                    .HasColumnName("Add_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCreator).HasColumnName("ID_creator");

                entity.Property(e => e.IdRequest).HasColumnName("ID_request");

                entity.Property(e => e.IdSuperTask).HasColumnName("ID_super_task");

                entity.Property(e => e.Task)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.IdCreatorNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.IdCreator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Users");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.IdRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Requests");

                entity.HasOne(d => d.IdSuperTaskNavigation)
                    .WithMany(p => p.InverseIdSuperTaskNavigation)
                    .HasForeignKey(d => d.IdSuperTask)
                    .HasConstraintName("FK_Tasks_Tasks");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdRole })
                    .HasName("PK__User_rol__73E998A3C512BD15");

                entity.ToTable("User_roles", "dbo");

                entity.Property(e => e.IdUser).HasColumnName("ID_user");

                entity.Property(e => e.IdRole).HasColumnName("ID_role");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_roles_Roles");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_roles_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Post).HasMaxLength(200);

                entity.Property(e => e.ServerRole)
                    .IsRequired()
                    .HasColumnName("Server_role")
                    .HasMaxLength(150);

                entity.Property(e => e.Surname).HasMaxLength(150);

                entity.HasOne(d => d.ServerRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ServerRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Server_roles");
            });

            modelBuilder.Entity<Visits>(entity =>
            {
                entity.ToTable("Visits", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdRequest).HasColumnName("ID_request");

                entity.Property(e => e.IdUser).HasColumnName("ID_user");

                entity.Property(e => e.VisitTime)
                    .HasColumnName("Visit_time")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.Visits)
                    .HasForeignKey(d => d.IdRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visits_Requests");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Visits)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visits_Users");
            });
        }
    }
}
