using System;
using System.Collections.Generic;
using Core.DTOs.ObraSocial;
using Core.DTOs.Paciente;


namespace Core.DTOs.Paciente
{
    public class PacienteDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }


        public ObraSocialSimpleDTO ObraSocial { get; set; }

        public List<SesionHistorialDTO> Sesiones { get; set; }

        public List<PagoHistorialDTO> Pagos { get; set; }

    }



}