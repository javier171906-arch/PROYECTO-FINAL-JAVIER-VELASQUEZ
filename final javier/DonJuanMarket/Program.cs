using System;
using System.Collections.Generic;
using DonJuanMarket.EstructurasDatos;
using DonJuanMarket.Clases;

namespace DonJuanMarket.Sistema
{
    public class Supermercado
    {
        private readonly ArbolAVL inventario;
        private readonly ColaPrioridad filaAtencion;
        private int clientesAtendidos;

        public Supermercado()
        {
            inventario = new ArbolAVL();
            filaAtencion = new ColaPrioridad();
            clientesAtendidos = 0;
        }

        // Inventario (Árbol AVL)
        public bool RegistrarProducto(Producto producto)
        {
            return inventario.Insertar(producto);
        }

        public Producto BuscarProducto(string codigo)
        {
            return inventario.Buscar(codigo);
        }

        public bool ActualizarProducto(string codigo, string nombre, string categoria, double precio, int stock)
        {
            return inventario.Actualizar(codigo, nombre, categoria, precio, stock);
        }

        public bool EliminarProducto(string codigo)
        {
            return inventario.Eliminar(codigo);
        }

        public List<Producto> ListarInventarioOrdenado()
        {
            return inventario.RecorridoInOrden();
        }

        public bool InventarioVacio
        {
            get { return inventario.EstaVacio; }
        }

        // Atención de clientes (Cola de Prioridad / Heap)
        public int AgregarClienteACola(Cliente cliente)
        {
            return filaAtencion.Agregar(cliente);
        }

        public Cliente AtenderSiguienteCliente()
        {
            Cliente atendido = filaAtencion.Atender();
            if (atendido != null)
                clientesAtendidos++;
            return atendido;
        }

        public List<Cliente> ListarClientesEnEspera()
        {
            return filaAtencion.ObtenerClientesEnEspera();
        }

        public bool FilaVacia
        {
            get { return filaAtencion.EstaVacia; }
        }

        public int ClientesEnEspera
        {
            get { return filaAtencion.Cantidad; }
        }

        // Reportes
        public int TotalProductosRegistrados()
        {
            return inventario.ContarProductos();
        }

        public int TotalUnidadesEnStock()
        {
            return inventario.StockTotal();
        }

        public int AlturaInventario()
        {
            return inventario.AlturaArbol();
        }

        public int ClientesAtendidos()
        {
            return clientesAtendidos;
        }
    }
}

namespace DonJuanMarket.Interfaz
{
    // Menu
    public class Menu
    {
        private readonly DonJuanMarket.Sistema.Supermercado sistema;
        private bool bucle;

        public Menu()
        {
            sistema = new DonJuanMarket.Sistema.Supermercado();
            bucle = true;
        }

        // Punto de entrada del ciclo principal del programa
        public void Ejecutar()
        {
            while (bucle)
            {
                MostrarMenuPrincipal();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                int opcion = LeerEntero("Seleccione una opción (1-10): ");
                Console.ResetColor();

                switch (opcion)
                {
                    case 1: 
                        RegistrarProducto(); 
                    break;

                    case 2: 
                        BuscarProducto(); 
                    break;

                    case 3: 
                        ActualizarProducto(); 
                    break;

                    case 4: 
                        EliminarProducto(); 
                    break;

                    case 5: 
                        MostrarInventarioOrdenado(); 
                    break;

                    case 6: 
                        AgregarClienteACola(); 
                    break;

                    case 7: 
                        AtenderSiguienteCliente(); 
                    break;

                    case 8: 
                        MostrarClientesEnEspera(); 
                    break;

                    case 9: 
                        MostrarEstadisticas(); 
                    break;

                    case 10: 
                        Salir(); 
                    break;

                    default:
                        EscribirError("Opción no válida. Escriba un número del 1 al 10.");
                    break;
                }

                if (bucle)
                    PausarYVolverAlMenu();
            }
        }

        // Menú principal
        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=========================================");
            Console.WriteLine("     SISTEMA DE GESTIÓN DE SUPERMERCADO");
            Console.WriteLine("            Don Juan Market");
            Console.WriteLine("=========================================");
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ INVENTARIO AVL ------");
            Console.ResetColor();

            Console.WriteLine("1. Registrar producto");
            Console.WriteLine("2. Buscar producto por código");
            Console.WriteLine("3. Actualizar producto");
            Console.WriteLine("4. Eliminar producto");
            Console.WriteLine("5. Mostrar inventario ordenado");
            Console.WriteLine();          
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ ATENCION DE CLIENTES ------");
            Console.ResetColor();

            Console.WriteLine("6. Agregar cliente a la cola");
            Console.WriteLine("7. Atender siguiente cliente");
            Console.WriteLine("8. Mostrar clientes en espera");
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ REPORTES ------");    
            Console.ResetColor();     

            Console.WriteLine("9. Mostrar estadísticas del sistema");
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ SISTEMA ------");
            Console.ResetColor();

            Console.WriteLine("10. Salir");
            Console.WriteLine();
        }

        // Opción 1: Registrar producto
        private void RegistrarProducto()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ REGISTRAR PRODUCTO ------");
            Console.ResetColor();

            string codigo = LeerTexto("Código del producto: ").ToUpper();
            string nombre = LeerTexto("Nombre del producto: ");
            string categoria = LeerTexto("Categoría: ");
            double precio = LeerDecimalPositivo("Precio (Q): ");
            int stock = LeerEnteroNoNegativo("Cantidad en stock: ");

            try
            {
                DonJuanMarket.Clases.Producto producto = new DonJuanMarket.Clases.Producto(codigo, nombre, categoria, precio, stock);
                bool insertado = sistema.RegistrarProducto(producto);

                if (insertado)
                {
                    Console.WriteLine();
                    EscribirExito(string.Format("Producto {0} insertado correctamente.", codigo));
                }
                else
                {
                    Console.WriteLine();
                    EscribirError(string.Format("Error: Ya existe un producto con el código {0}.", codigo));
                }
            }
            catch (ArgumentException ex)
            {
                EscribirError("Error: " + ex.Message);
            }
        }

        // Opción 2: Buscar producto por código
        private void BuscarProducto()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ BUSCAR PRODUCTO ------");
            Console.ResetColor();

            string codigo = LeerTexto("Código del producto a buscar: ").ToUpper();
            DonJuanMarket.Clases.Producto producto = sistema.BuscarProducto(codigo);

            if (producto == null)
            {
                EscribirError("Producto no encontrado.");
                return;
            }

            Console.WriteLine();
            EscribirExito("Producto encontrado:");
            Console.WriteLine(producto.ToString());
        }

        // Opción 3: Actualizar producto
        private void ActualizarProducto()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ ACTUALIZAR PRODUCTO ------");
            Console.ResetColor();

            string codigo = LeerTexto("Código del producto a actualizar: ").ToUpper();
            DonJuanMarket.Clases.Producto existente = sistema.BuscarProducto(codigo);

            if (existente == null)
            {
                EscribirError(string.Format("Error: No existe un producto con el código {0}.", codigo));
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Producto actual:");
            Console.WriteLine(existente.ToString());
            Console.WriteLine();
            Console.WriteLine("Ingrese los nuevos datos del producto:");

            string nombre = LeerTexto("Nombre: ");
            string categoria = LeerTexto("Categoría: ");
            double precio = LeerDecimalPositivo("Precio (Q): ");
            int stock = LeerEnteroNoNegativo("Cantidad en stock: ");

            try
            {
                sistema.ActualizarProducto(codigo, nombre, categoria, precio, stock);
                Console.WriteLine();
                EscribirExito("Producto actualizado correctamente.");
                Console.WriteLine(sistema.BuscarProducto(codigo).ToString());
            }
            catch (ArgumentException ex)
            {
                EscribirError("Error: " + ex.Message);
            }
        }

        // Opción 4: Eliminar producto
        private void EliminarProducto()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ ELIMINAR PRODUCTO ------");
            Console.ResetColor();

            string codigo = LeerTexto("Código del producto a eliminar: ").ToUpper();
            bool eliminado = sistema.EliminarProducto(codigo);

            if (eliminado)
                EscribirExito(string.Format("Producto {0} eliminado correctamente.", codigo));
            else
                EscribirError(string.Format("Error: No existe un producto con el código {0}.", codigo));
        }

        // Opción 5: Mostrar inventario ordenado (recorrido in-orden)
        private void MostrarInventarioOrdenado()
        {
            if (sistema.InventarioVacio)
            {
                EscribirError("El inventario está vacío. Registre productos con la opción 1.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============================================");
            Console.WriteLine("          INVENTARIO DEL SUPERMERCADO       ");
            Console.WriteLine("============================================");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(string.Format("{0,-8} {1,-22} {2,-14} {3,-9} {4,-6}",
                "Código", "Nombre", "Categoría", "Precio", "Stock"));
            Console.WriteLine(new string('-', 62));
            Console.ResetColor();

            foreach (DonJuanMarket.Clases.Producto producto in sistema.ListarInventarioOrdenado())
                Console.WriteLine(producto.ComoFilaTabla());
        }

        // Opción 6: Agregar cliente a la cola
        private void AgregarClienteACola()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ AGREGAR CLIENTE ------");
            Console.ResetColor();

            string nombre = LeerTexto("Nombre del cliente: ");
            DonJuanMarket.Clases.TipoCliente tipo = LeerTipoCliente();
            int numeroProductos = LeerEnteroNoNegativo("Número de productos que llevará: ");

            try
            {
                DonJuanMarket.Clases.Cliente cliente = new DonJuanMarket.Clases.Cliente(nombre, tipo, numeroProductos);
                int posicion = sistema.AgregarClienteACola(cliente);

                Console.WriteLine();
                EscribirExito(string.Format("Cliente {0} agregado correctamente.", nombre));
            }
            catch (ArgumentException ex)
            {
                EscribirError("Error: " + ex.Message);
            }
        }

        // Submenú para elegir el tipo de cliente
        private DonJuanMarket.Clases.TipoCliente LeerTipoCliente()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Tipo de cliente:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  1. VIP");
            Console.WriteLine("  2. ADULTO MAYOR");
            Console.WriteLine("  3. REGULAR");
            Console.ResetColor();

            while (true)
            {
                int seleccion = LeerEntero("Seleccione el tipo (1-3): ");
                switch (seleccion)
                {
                    case 1: return DonJuanMarket.Clases.TipoCliente.VIP;
                    case 2: return DonJuanMarket.Clases.TipoCliente.ADULTO_MAYOR;
                    case 3: return DonJuanMarket.Clases.TipoCliente.REGULAR;
                    default:
                        EscribirError("Opción no válida. Escriba 1, 2 o 3.");
                        break;
                }
            }
        }

        // Opción 7: Atender siguiente cliente
        private void AtenderSiguienteCliente()
        {
            if (sistema.FilaVacia)
            {
                EscribirError("No hay clientes en espera.");
                return;
            }

            DonJuanMarket.Clases.Cliente atendido = sistema.AtenderSiguienteCliente();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ ATENDIENDO A CLIENTE ------");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine(atendido.ToString());
            Console.WriteLine();
            EscribirExito("Cliente atendido correctamente.");
        }

        // Opción 8: Mostrar clientes en espera
        private void MostrarClientesEnEspera()
        {
            if (sistema.FilaVacia)
            {
                EscribirError("No hay clientes en espera.");
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------ COLA DE ATENCION AL CLIENTE ------");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(string.Format("{0,-9}| {1,-18}| {2,-14}| {3}",
                "Posición", "Nombre", "Tipo", "Productos"));
            Console.WriteLine(new string('-', 55));
            Console.ResetColor();

            int posicion = 1;
            foreach (DonJuanMarket.Clases.Cliente cliente in sistema.ListarClientesEnEspera())
            {
                Console.WriteLine(string.Format("{0,-9}| {1,-18}| {2,-14}| {3}",
                    posicion, cliente.Nombre, cliente.Tipo, cliente.NumeroProductos));
                posicion++;
            }
        }

        // Opción 9: Mostrar estadísticas del sistema
        private void MostrarEstadisticas()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================");
            Console.WriteLine("       ESTADÍSTICAS DEL SISTEMA");
            Console.WriteLine("=========================================");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Inventario");
            Console.ResetColor();

            Console.WriteLine("  Productos registrados: " + sistema.TotalProductosRegistrados());
            Console.WriteLine("  Unidades en existencia: " + sistema.TotalUnidadesEnStock());
            Console.WriteLine("  Altura actual del árbol: " + sistema.AlturaInventario());
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Atención de clientes");
            Console.ResetColor();

            Console.WriteLine("  Clientes en espera: " + sistema.ClientesEnEspera);
            Console.WriteLine("  Clientes atendidos: " + sistema.ClientesAtendidos());
        }

        // Opción 10: Salir
        private void Salir()
        {
            bucle = false;
            Console.WriteLine();
            Console.WriteLine("Cerrando el programa...");
        }

        // manejo de excepciones
        private string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entrada))
                    return entrada.Trim();

                EscribirError("Este dato no puede quedar vacío. Inténtelo de nuevo.");
            }
        }

        // Repite si el usuario ingresa un valor no numerico
        private int LeerEntero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();
                int valor;

                if (int.TryParse(entrada, out valor))
                    return valor;

                EscribirError("Entrada no válida. Escriba únicamente números.");
            }
        }

        // Lee un entero mayor o igual a 0 (Stock y cantidad en el inventario)
        private int LeerEnteroNoNegativo(string mensaje)
        {
            while (true)
            {
                int valor = LeerEntero(mensaje);
                if (valor >= 0)
                    return valor;

                EscribirError("El valor debe ser mayor o igual a 0.");
            }
        }

        // Lee un número decimal mayor que 0 para el precio
        private double LeerDecimalPositivo(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();
                double valor;

                if (double.TryParse(entrada, System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out valor))
                {
                    if (valor > 0)
                        return valor;

                    EscribirError("El precio debe ser mayor que 0.");
                }
                else
                {
                    EscribirError("Entrada no válida. Escriba un número (ejemplo: 45.99).");
                }
            }
        }

        private void PausarYVolverAlMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Presione ENTER para volver al menú principal...");
            Console.ResetColor();
            Console.ReadLine();
        }

        // Colores y formatos
        private void EscribirExito(string texto)
        {
            ConsoleColor anterior = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(texto);
            Console.ForegroundColor = anterior;
        }

        private void EscribirError(string texto)
        {
            ConsoleColor anterior = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(texto);
            Console.ForegroundColor = anterior;
        }
    }
}

namespace DonJuanMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            DonJuanMarket.Interfaz.Menu menu = new DonJuanMarket.Interfaz.Menu();
            menu.Ejecutar();
        }
    }
}