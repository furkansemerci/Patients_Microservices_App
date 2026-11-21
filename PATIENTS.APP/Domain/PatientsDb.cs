using Microsoft.EntityFrameworkCore;

namespace PATIENTS.APP.Domain
{

    public class PatientsDb : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }

        public PatientsDb(DbContextOptions options) : base(options)
        {
            
        }

    }
}