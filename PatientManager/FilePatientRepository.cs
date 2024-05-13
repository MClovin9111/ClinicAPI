using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManager
{
    public class FilePatientRepository
    {
        private readonly string _filePath;

        public FilePatientRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();
            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    patients.Add(new Patient
                    {
                        Name = parts[0],
                        LastName = parts[1],
                        CI = parts[2],
                        BloodGroup = parts[3]
                    });
                }
            }
            return patients;
        }

        public void SavePatients(List<Patient> patients)
        {
            using (var writer = new StreamWriter(_filePath))
            {
                foreach (var patient in patients)
                {
                    writer.WriteLine($"{patient.Name},{patient.LastName},{patient.CI},{patient.BloodGroup}");
                }
            }
        }
    }
}
