using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_CT.Models;

public partial class GerenciadorCtDbContext : DbContext
{
    public GerenciadorCtDbContext()
    {
    }

    public GerenciadorCtDbContext(DbContextOptions<GerenciadorCtDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aluno> Alunos { get; set; }

    public virtual DbSet<AlunoAula> AlunoAulas { get; set; }

    public virtual DbSet<Aula> Aulas { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Modalidade> Modalidades { get; set; }

    public virtual DbSet<ModalidadesAluno> ModalidadesAlunos { get; set; }

    public virtual DbSet<Treinadore> Treinadores { get; set; }

    public virtual DbSet<TreinadoresModalidade> TreinadoresModalidades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=gerenciadorCT_db;User ID=Gerenciador_ct; password=Gerenciador_ct; language=Portuguese;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__alunos__3213E83F725F44E2");

            entity.ToTable("alunos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cpf");
            entity.Property(e => e.Idade).HasColumnName("idade");
            entity.Property(e => e.Nome)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<AlunoAula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__alunoAul__3213E83F8A5F66EA");

            entity.ToTable("alunoAula");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkAluno).HasColumnName("fk_aluno");
            entity.Property(e => e.FkAula).HasColumnName("fk_aula");

            entity.HasOne(d => d.FkAlunoNavigation).WithMany(p => p.AlunoAulas)
                .HasForeignKey(d => d.FkAluno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_alunos");

            entity.HasOne(d => d.FkAulaNavigation).WithMany(p => p.AlunoAulas)
                .HasForeignKey(d => d.FkAula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aulas");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__aula__3213E83F10AB8156");

            entity.ToTable("aula");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkHorario).HasColumnName("fk_horario");
            entity.Property(e => e.FkTreinador).HasColumnName("fk_treinador");

            entity.HasOne(d => d.FkHorarioNavigation).WithMany(p => p.Aulas)
                .HasForeignKey(d => d.FkHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pk_horario");

            entity.HasOne(d => d.FkTreinadorNavigation).WithMany(p => p.Aulas)
                .HasForeignKey(d => d.FkTreinador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pkTreinadores");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__horarios__3213E83F46790251");

            entity.ToTable("horarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dia");
            entity.Property(e => e.Hora)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hora");
        });

        modelBuilder.Entity<Modalidade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__modalida__3213E83F7017A7D6");

            entity.ToTable("modalidades");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasColumnName("descricao");
            entity.Property(e => e.Nome)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<ModalidadesAluno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Modalida__3213E83F3AC8DA79");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkAlunos).HasColumnName("fk_alunos");
            entity.Property(e => e.FkModalidades).HasColumnName("fk_modalidades");

            entity.HasOne(d => d.FkAlunosNavigation).WithMany(p => p.ModalidadesAlunos)
                .HasForeignKey(d => d.FkAlunos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkAlunos");

            entity.HasOne(d => d.FkModalidadesNavigation).WithMany(p => p.ModalidadesAlunos)
                .HasForeignKey(d => d.FkModalidades)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkAModalidades");
        });

        modelBuilder.Entity<Treinadore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__treinado__3213E83F9F9423A9");

            entity.ToTable("treinadores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cpf");
            entity.Property(e => e.Idade).HasColumnName("idade");
            entity.Property(e => e.Nome)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<TreinadoresModalidade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__treinado__3213E83F888C1A19");

            entity.ToTable("treinadoresModalidades");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkModalidades).HasColumnName("fk_modalidades");
            entity.Property(e => e.FkTreinadores).HasColumnName("fk_treinadores");

            entity.HasOne(d => d.FkModalidadesNavigation).WithMany(p => p.TreinadoresModalidades)
                .HasForeignKey(d => d.FkModalidades)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkModalidades");

            entity.HasOne(d => d.FkTreinadoresNavigation).WithMany(p => p.TreinadoresModalidades)
                .HasForeignKey(d => d.FkTreinadores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkTreinadores");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
