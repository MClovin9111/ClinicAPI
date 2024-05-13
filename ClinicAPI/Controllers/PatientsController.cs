using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly FilePatientRepository _patientRepository;

        public PatientsController(ILogger<PatientsController> logger, IConfiguration configuration)
        {
            _logger = logger;

            var filePath = configuration.GetSection("FileStorage:FilePath").Value;
            _patientRepository = new FilePatientRepository(filePath);
        }

        [HttpPost]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            try
            {
                var patients = _patientRepository.GetAllPatients();

                // Asignar aleatoriamente el grupo sanguíneo
                var bloodGroups = new List<string> { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
                var random = new Random();
                patient.BloodGroup = bloodGroups[random.Next(bloodGroups.Count)];

                patients.Add(patient);
                _patientRepository.SavePatients(patients);

                _logger.LogInformation($"Se ha creado un nuevo paciente: {patient.Name} {patient.LastName}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el paciente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{ci}")]
        public IActionResult UpdatePatient(string ci, [FromBody] Patient patient)
        {
            try
            {
                var patients = _patientRepository.GetAllPatients();
                var existingPatient = patients.FirstOrDefault(p => p.CI == ci);
                if (existingPatient == null)
                {
                    _logger.LogWarning($"No se encontró ningún paciente con CI: {ci}");
                    return NotFound("Paciente no encontrado");
                }

                existingPatient.Name = patient.Name;
                existingPatient.LastName = patient.LastName;
                _patientRepository.SavePatients(patients);

                _logger.LogInformation($"Se ha actualizado la información del paciente con CI: {ci}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el paciente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{ci}")]
        public IActionResult DeletePatient(string ci)
        {
            try
            {
                var patients = _patientRepository.GetAllPatients();
                var existingPatient = patients.FirstOrDefault(p => p.CI == ci);
                if (existingPatient == null)
                {
                    _logger.LogWarning($"No se encontró ningún paciente con CI: {ci}");
                    return NotFound("Paciente no encontrado");
                }

                patients.Remove(existingPatient);
                _patientRepository.SavePatients(patients);

                _logger.LogInformation($"Se ha eliminado el paciente con CI: {ci}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el paciente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        public IActionResult GetPatients()
        {
            try
            {
                var patients = _patientRepository.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener los pacientes: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{ci}")]
        public IActionResult GetPatientByCI(string ci)
        {
            try
            {
                var patients = _patientRepository.GetAllPatients();
                var patient = patients.FirstOrDefault(p => p.CI == ci);
                if (patient == null)
                {
                    _logger.LogWarning($"No se encontró ningún paciente con CI: {ci}");
                    return NotFound("Paciente no encontrado");
                }

                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el paciente con CI: {ci}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
