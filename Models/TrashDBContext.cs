using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models.Trash_Track.Models;

namespace Trash_Track.Models
{
    public class TrashDBContext :IdentityDbContext
{
    
        public TrashDBContext(DbContextOptions<TrashDBContext> options) : base(options)
        {

        }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<PickupSchedule> PickupSchedules { get; set; }
        public DbSet<PickupOverride> PickupOverrides { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ReportPickupStatus> ReportPickupStatuses { get; set; }
        public DbSet<DriverPickupStatus> DriverPickupStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var wardNames = new[]
  {
    "Dilli Bazaar", "Maitidevi", "Gaushala", "Gyaneshwor", "Baneshwor", "Tinkune", "Sinamangal", "Tilganga",
    "Old Baneshwor", "New Baneshwor", "Minbhawan", "Shantinagar", "Anamnagar", "Babarmahal", "Tripureshwor", "Thapathali",
    "Teku", "Kalimati", "Balkhu", "Kuleshwor", "Chhetrapati", "Indra Chowk", "Ason", "Basantapur",
    "Thamel", "Lazimpat", "Maharajgunj", "Baluwatar", "Budhanilkantha", "Gongabu", "Tokha", "Samakhusi"
};

            var wards = new List<Ward>();
            for (int i = 0; i < 32; i++)
            {
                wards.Add(new Ward
                {
                    Id = i + 1,
                    No = i + 1,
                    Name = wardNames[i]
                });
            }

            modelBuilder.Entity<Ward>().HasData(wards);


            // Seed 5 drivers
            var drivers = new List<Driver>
{
    new Driver { Id = 1, Name = "Ram Bahadur", Contact = "9801000001", Status = "Active" },
    new Driver { Id = 2, Name = "Shyam Lal", Contact = "9801000002", Status = "Active" },
    new Driver { Id = 3, Name = "Sita Thapa", Contact = "9801000003", Status = "Active" },
    new Driver { Id = 4, Name = "Gopal Basnet", Contact = "9801000004", Status = "Active" },
    new Driver { Id = 5, Name = "Nisha Shrestha", Contact = "9801000005", Status = "Active" }
};

            modelBuilder.Entity<Driver>().HasData(drivers);

            // Seed pickup schedules (1 for each ward, rotating days)
            var pickupSchedules = new List<PickupSchedule>();
            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray();
            var random = new Random();

            int scheduleId = 1;
            for (int i = 1; i <= 32; i++)
            {
                pickupSchedules.Add(new PickupSchedule
                {
                    Id = scheduleId++,
                    WardId = i,
                    PickupDay = days[(i - 1) % 7], // cycle through Sunday to Saturday
                    PickupTime = new TimeSpan(6, 0, 0)

                });
            }

            modelBuilder.Entity<PickupSchedule>().HasData(pickupSchedules);
        }
    }
}


