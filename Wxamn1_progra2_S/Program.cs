using System;
using System.Linq;


//Examen 1
//Programación 2
//Sistema de simulacion de recetas de clinica
//Steven Moscoso Acuña

namespace SistemaGestionPacientes
{
    //Se define la clase pasiente
    public class Paciente
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string TipoSangre { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Medicamento[] Tratamiento { get; set; }

        public Paciente()
        {
            //Aqui se inicializa la matriz tratamiento
            Tratamiento = new Medicamento[3];
        }
    }

    public class Medicamento
    {
        //Se define la clase tratamiento
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }

    public class SistemaGestion
    {
        private Paciente[] Pacientes { get; set; }
        private Medicamento[] CatalogoMedicamentos { get; set; }

        public SistemaGestion()
        {
            //Inicialización de la matriz de pacientes y del catálogo de medicamentos con un maximo de 100 en cada uno
            Pacientes = new Paciente[100];
            CatalogoMedicamentos = new Medicamento[100];
        }

        public void NuevoPaciente(Paciente paciente)
        {
            for (int i = 0; i < Pacientes.Length; i++)
            {//se busca en la matriz mediante un bucle, si la posicion es nula entonces se ingresa ahi el nuevo pasiente
                if (Pacientes[i] == null)
                {
                    Pacientes[i] = paciente;
                    break;
                }
            }
        }

        public void AgregarMedicamento(Medicamento medicamento)
        {
            for (int i = 0; i < CatalogoMedicamentos.Length; i++)
            {
                //se recorre tada la matriz hasta validar que la posision sea nula para ingresar el nuevo medicamento
                if (CatalogoMedicamentos[i] == null)
                {
                    CatalogoMedicamentos[i] = medicamento;
                    break;
                }
            }
        }

        public void AsignarTratamiento()
        {
            Paciente paciente = null;
            while (paciente == null)
            {
                Console.WriteLine("=================================");
                Console.WriteLine("Cédulas disponibles:");
                foreach (var p in Pacientes)
                {
                    if (p != null)
                    {
                        Console.WriteLine(p.Cedula);
                    }
                }
                Console.WriteLine("=================================");

                Console.Write("Ingrese la cédula del paciente: ");
                string cedulaPaciente = Console.ReadLine();
                paciente = Pacientes.FirstOrDefault(p => p != null && p.Cedula == cedulaPaciente);

                if (paciente == null)
                {
                    Console.WriteLine("Cédula no encontrada. Por favor, intente de nuevo.");
                }
            }


            Console.WriteLine(" ");
            //se imprimen los medicamentos en pantalla con sus respectivos codigos
            //para que sea facil para el ususario recetar el medicamento
            Console.WriteLine("=================================");
            Console.WriteLine("Códigos disponibles:");
            foreach (var medicamento in CatalogoMedicamentos)
            {
                if (medicamento != null)
                {
                    Console.WriteLine($"{medicamento.Nombre}         {medicamento.Codigo}");
                    //se imprimen medicamentos hasta que ya no hayan mas
                }
            }
            Console.WriteLine("=================================");
            Console.WriteLine(" ");

            Console.WriteLine("   *Ingrese los códigos de los medicamentos (separados por comas):* ");
            string[] codigosMedicamentos = Console.ReadLine().Split(',');

            int count = 0;
            //se utiliza un contador para que se pueda definir una cantidad maxima de 3 medicamentos por pasiente
            foreach (var codigo in codigosMedicamentos)
            {
                //se busca en el catálogo de medicamentos un medicamento que el codigo coincida con el ingresado
                var medicamento = CatalogoMedicamentos.FirstOrDefault(m => m != null && m.Codigo == codigo);
                if (medicamento != null && medicamento.Cantidad > 0 && count < 3)
                {
                    Console.Write($"   *ingrese la cantidad de {medicamento.Nombre} a recetar:* ");
                    //se le solicita al usuario indicar la cantidad de ese medicamento a recetar
                    int cantidadAsignada = int.Parse(Console.ReadLine());
                    if (cantidadAsignada <= medicamento.Cantidad)
                    {
                        medicamento.Cantidad -= cantidadAsignada;
                        //se resta a la cantidad existente en inventario
                        paciente.Tratamiento[count] = new Medicamento
                        {
                            Codigo = medicamento.Codigo,
                            Nombre = medicamento.Nombre,
                            Cantidad = cantidadAsignada
                        };
                        count++;
                    }
                    else
                    {
                        //se repite si la cantidad recetada es mayor a la existente en el inventario
                        Console.WriteLine(" ");
                        Console.WriteLine("----La cantidad ingresada excede la cantidad disponible en el inventario----");
                        Console.WriteLine(" ");

                    }
                }
            }
        }

        public void DetallesPacientes()
        {
            int totalPacientes = 0;
            foreach (var paciente in Pacientes)
            {
                if (paciente != null)
                {
                    totalPacientes++;
                    Console.WriteLine($"Paciente {totalPacientes}:");
                    Console.WriteLine($"Cédula: {paciente.Cedula}");
                    Console.WriteLine($"Nombre: {paciente.Nombre}");
                    Console.WriteLine($"Teléfono: {paciente.Telefono}");
                    Console.WriteLine($"Tipo de Sangre: {paciente.TipoSangre}");
                    Console.WriteLine($"Dirección: {paciente.Direccion}");
                    Console.WriteLine("-----------------------------");
                }
            }
            Console.WriteLine($"Cantidad total de pacientes registrados: {totalPacientes}");
        }


        public string[] ReporteMedicamentos()
        {
            List<string> medicamentosRecetados = new List<string>();
            foreach (var paciente in Pacientes)
            //se recorre cada pasiente en pasientes
            {
                if (paciente != null)
                //se valida que mo sea nulo
                {
                    //recorre cada medicamento del pasiente
                    foreach (var medicamento in paciente.Tratamiento)
                    {
                        if (medicamento != null)
                        { //si lo anterior se cumple se añade el nombre del medicamento y la cantidad recetada a medicamentosRecetados
                            medicamentosRecetados.Add($"{medicamento.Nombre}: {medicamento.Cantidad}");
                        }
                    }
                }
            }
            return medicamentosRecetados.Distinct().ToArray();
        }

        public int[] ObtenerReporteEdades()
        {
            int[] edades = new int[4];
            foreach (var paciente in Pacientes)
            {
                if (paciente != null)
                {
                    int edad = DateTime.Now.Year - paciente.FechaNacimiento.Year;
                    if (edad <= 10)
                    {
                        edades[0]++;
                    }
                    else if (edad <= 30)
                    {
                        edades[1]++;
                    }
                    else if (edad <= 50)
                    {
                        edades[2]++;
                    }
                    else
                    {
                        edades[3]++;
                    }
                }
            }
            return edades;
        }

        public void MostrarMenu() //Menu
        {
            while (true)
            {
                Console.WriteLine(" ");
                Console.WriteLine("     ***Menú principal***:");
                Console.WriteLine("   1- Agregar paciente");
                Console.WriteLine("   2- Agregar medicamento al catálogo");
                Console.WriteLine("   3- Asignar tratamiento médico a un paciente");
                Console.WriteLine("   4- Consultas");
                Console.WriteLine("   5- Salir");
                Console.WriteLine(" ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        //ingreso de datos
                        Console.WriteLine(" ");
                        Console.WriteLine("Ingrese los datos del paciente:");
                        Console.Write("Cédula: ");
                        string cedula = Console.ReadLine();
                        Console.Write("Nombre: ");
                        string nombre = Console.ReadLine();
                        Console.Write("Teléfono: ");
                        string telefono = Console.ReadLine();
                        Console.Write("Tipo de sangre: ");
                        string tipoSangre = Console.ReadLine();
                        Console.Write("Dirección: ");
                        string direccion = Console.ReadLine();
                        Console.Write("Fecha de nacimiento (dd/mm/yyyy): ");
                        DateTime fechaNacimiento = DateTime.Parse(Console.ReadLine());

                        Paciente paciente = new Paciente
                        {
                            Cedula = cedula,
                            Nombre = nombre,
                            Telefono = telefono,
                            TipoSangre = tipoSangre,
                            Direccion = direccion,
                            FechaNacimiento = fechaNacimiento
                        };

                        NuevoPaciente(paciente);
                        Console.WriteLine(" ");
                        Console.WriteLine("----Paciente agregado exitosamente----");
                        Console.WriteLine(" ");
                        break;
                    case "2":
                        Console.WriteLine(" ");
                        Console.WriteLine("Ingrese los datos del medicamento:");
                        Console.Write("Código: ");
                        string codigo = Console.ReadLine();
                        Console.Write("Nombre: ");
                        string nombreMedicamento = Console.ReadLine();
                        Console.Write("Cantidad: ");
                        int cantidad = int.Parse(Console.ReadLine());

                        Medicamento medicamento = new Medicamento
                        {
                            Codigo = codigo,
                            Nombre = nombreMedicamento,
                            Cantidad = cantidad
                        };

                        AgregarMedicamento(medicamento);
                        Console.WriteLine(" ");
                        Console.WriteLine("----Medicamento agregado exitosamente----");
                        Console.WriteLine(" ");
                        break;
                    case "3":
                        AsignarTratamiento();
                        Console.WriteLine(" ");
                        Console.WriteLine("---Tratamiento asignado exitosamente----");
                        Console.WriteLine(" ");
                        break;
                    case "4":
                        while (true) //Submenu de consultas
                        {
                            Console.WriteLine("       ***Menú de consultas***:");
                            Console.WriteLine("   1- Cantidad total de pacientes registrados");
                            Console.WriteLine("   2- Reporte de todos los medicamentos recetados");
                            Console.WriteLine("   3- Reporte de cantidad de pacientes agrupados por edades: 0-10 años, 11-30 años, 31-50 años y mayores de 51 años");
                            Console.WriteLine("   4- Regresar al menú principal");
                            Console.WriteLine(" ");

                            var opcionConsulta = Console.ReadLine();

                            switch (opcionConsulta)
                            {
                                case "1":
                                    Console.WriteLine(" ");
                                    Console.WriteLine("============================================================");
                                    DetallesPacientes();
                                    Console.WriteLine("============================================================");
                                    Console.WriteLine(" ");
                                    break;
                                case "2":
                                    Console.WriteLine(" ");
                                    Console.WriteLine("====================================================");
                                    Console.WriteLine("Reporte de todos los medicamentos recetados:");
                                    Console.WriteLine("====================================================");
                                    Console.WriteLine(" ");
                                    foreach (var medicamentoReporte in ReporteMedicamentos())
                                    {
                                        Console.WriteLine(medicamentoReporte);  
                                    }
                                    Console.WriteLine("====================================================");
                                    Console.WriteLine(" ");
                                    break;
                                case "3":
                                    Console.WriteLine(" ");
                                    Console.WriteLine("======================================================");
                                    Console.WriteLine("Reporte de cantidad de pacientes agrupados por edades:");
                                    var reporteEdades = ObtenerReporteEdades();
                                    Console.WriteLine("0-10 años: " + reporteEdades[0]);
                                    Console.WriteLine("11-30 años: " + reporteEdades[1]);
                                    Console.WriteLine("31-50 años: " + reporteEdades[2]);
                                    Console.WriteLine("Mayores de 51 años: " + reporteEdades[3]);
                                    Console.WriteLine("======================================================");
                                    Console.WriteLine(" ");
                                    break;

                                case "4":
                                    return;
                                default:
                                    Console.WriteLine(" ");
                                    Console.WriteLine("////////opcion no valida por favor, intente de nuevo/////////");
                                    Console.WriteLine(" ");
                                    break;
                            }
                        }
                    case "5":
                        Console.WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        Console.WriteLine(" ");
                        Console.WriteLine("////////opcion no valida por favor, intente de nuevo/////////");
                        Console.WriteLine(" ");
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SistemaGestion sistema = new SistemaGestion();
            sistema.MostrarMenu();
        }
    }
}