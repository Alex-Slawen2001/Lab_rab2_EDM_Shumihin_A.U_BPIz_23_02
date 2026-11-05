// ==========================================
// Файл: CompanyEntities.cs
// Назначение: Модель ADO.NET EDM (Entity Framework 6, подход Code First).
//             Класс сгенерирован шаблоном «Модель ADO.NET EDM» → «Пустая модель Code First»
//             с именем CompanyEntities; в него добавлены сущности модели:
//               Roles   — должности  (класс Model/Role.cs)
//               Persons — сотрудники (класс Model/Person.cs)
//             Строка подключения "name=CompanyEntities" берётся из App.config.
//             При первом обращении создаётся локальная БД CompanyEntities
//             (SQL Server Express LocalDB) с таблицами Roles и People.
// ==========================================

using System;
using System.Data.Entity;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Model;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02
{
    public class CompanyEntities : DbContext
    {
        // Контекст настроен для использования строки подключения "CompanyEntities"
        // из файла конфигурации приложения (App.config).
        public CompanyEntities()
            : base("name=CompanyEntities")
        {
            // БД создаётся, если её ещё нет, и заполняется начальными данными
            Database.SetInitializer(new CompanyInitializer());
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель.

        /// <summary>должности сотрудников</summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>сотрудники</summary>
        public virtual DbSet<Person> Persons { get; set; }
    }

    /// <summary>
    /// Инициализатор БД: создаёт базу данных, если её нет,
    /// и заполняет таблицы начальными данными (метод Seed).
    /// </summary>
    public class CompanyInitializer : CreateDatabaseIfNotExists<CompanyEntities>
    {
        protected override void Seed(CompanyEntities context)
        {
            Role director   = new Role { NameRole = "Директор"  };
            Role accountant = new Role { NameRole = "Бухгалтер" };
            Role manager    = new Role { NameRole = "Менеджер"  };
            context.Roles.Add(director);
            context.Roles.Add(accountant);
            context.Roles.Add(manager);

            context.Persons.Add(new Person { Role = director,   FirstName = "Иван",   LastName = "Иванов",   Birthday = new DateTime(1980, 02, 28) });
            context.Persons.Add(new Person { Role = accountant, FirstName = "Петр",   LastName = "Петров",   Birthday = new DateTime(1981, 03, 20) });
            context.Persons.Add(new Person { Role = manager,    FirstName = "Виктор", LastName = "Викторов", Birthday = new DateTime(1982, 04, 15) });
            context.Persons.Add(new Person { Role = manager,    FirstName = "Сидор",  LastName = "Сидоров",  Birthday = new DateTime(1983, 05, 10) });

            context.SaveChanges();
        }
    }
}
