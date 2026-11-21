using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PATIENTS.APP.Domain
{
    
    public class PatientsDbFactory : IDesignTimeDbContextFactory<PatientsDb>
    {
        const string CONNECTIONSTRING = "data source=PatientsDB.db";
        
        public PatientsDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PatientsDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new PatientsDb(optionsBuilder.Options);

        }

        public void SeedDb()
        {
            var db = CreateDbContext(null);
            
            var doctorPatients = db.DoctorPatients.ToList();
            db.DoctorPatients.RemoveRange(doctorPatients);
            
            var doctors = db.Doctors.ToList();
            db.Doctors.RemoveRange(doctors);
            
            var patients = db.Patients.ToList();
            db.Patients.RemoveRange(patients);
            
            var branches = db.Branches.ToList();
            db.Branches.RemoveRange(branches);

            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='DoctorPatients'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='Doctors'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='Patients'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='Branches'");

            var patient = new Patient
            {
                FirstName = "Ayse",
                LastName = "Yilmaz",
                Gender = Genders.Woman,
                BirthDate = new DateTime(2000, 5, 15),
                Height = 165,
                Weight = 60

            };
            
            db.Patients.Add(patient);
            db.SaveChanges();

            db.Branches.Add(new Branch
            {
                Title = "Cardiology",
                Doctors = new List<Doctor>
                {
                    new Doctor
                    {
                        FirstName = "Ali",
                        LastName = "Demir",
                        IsExpert = true,
                        DoctorPatients = new List<DoctorPatient>
                        {
                            new DoctorPatient { PatientId = patient.Id }
                        }
                    }
                }
            });
            db.Branches.Add(new Branch { Title = "Neurology" });
            db.SaveChanges();
        }
    }
}