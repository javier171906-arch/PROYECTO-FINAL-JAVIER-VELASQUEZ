using System;

namespace DonJuanMarket.Clases
{
    // TipoCliente
    public enum TipoCliente
    {
        VIP = 1,
        ADULTO_MAYOR = 2,
        REGULAR = 3
    }

    // Producto
    public class Producto
    {
        // Atributos privados: solo se exponen mediante propiedades (encapsulación).
        private readonly string codigo;
        private string nombre;
        private string categoria;
        private double precio;
        private int stock;

        public Producto(string codigo, string nombre, string categoria, double precio, int stock)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del producto no puede estar vacío.");
            if (precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0.");
            if (stock < 0)
                throw new ArgumentException("El stock debe ser mayor o igual a 0.");

            this.codigo = codigo.Trim().ToUpper();
            this.nombre = nombre;
            this.categoria = categoria;
            this.precio = precio;
            this.stock = stock;
        }

        // Clave única del producto
        public string Codigo
        {
            get { return codigo; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public double Precio
        {
            get { return precio; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El precio debe ser mayor que 0.");
                precio = value;
            }
        }

        public int Stock
        {
            get { return stock; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("La cantidad en stock debe ser mayor o igual a 0.");
                stock = value;
            }
        }

        // Reemplaza los datos modificables del producto
        public void ActualizarDatos(string nuevoNombre, string nuevaCategoria, double nuevoPrecio, int nuevoStock)
        {
            if (nuevoPrecio <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0.");
            if (nuevoStock < 0)
                throw new ArgumentException("La cantidad en stock debe ser mayor o igual a 0.");

            nombre = nuevoNombre;
            categoria = nuevaCategoria;
            precio = nuevoPrecio;
            stock = nuevoStock;
        }

        // Línea usada en el listado del inventario
        public string ComoFilaTabla()
        {
            return string.Format("{0,-8} {1,-22} {2,-14} Q{3,-8:0.00} {4,-6}",
                codigo, nombre, categoria, precio, stock);
        }

        public override string ToString()
        {
            return string.Format(
                "Código   : {0}\nNombre   : {1}\nCategoría: {2}\nPrecio   : Q{3:0.00}\nStock    : {4}",
                codigo, nombre, categoria, precio, stock);
        }
    }

    // Cliente
    public class Cliente
    {
        public string Nombre { get; private set; }
        public TipoCliente Tipo { get; private set; }
        public int NumeroProductos { get; private set; }
        public DateTime HoraLlegada { get; private set; }

        // Orden de llegada asignado por la cola de prioridad
        public int NumeroLlegada { get; set; }

        public Cliente(string nombre, TipoCliente tipo, int numeroProductos)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del cliente no puede estar vacío.");
            if (numeroProductos < 0)
                throw new ArgumentException("El número de productos no puede ser negativo.");

            Nombre = nombre;
            Tipo = tipo;
            NumeroProductos = numeroProductos;
            HoraLlegada = DateTime.Now;
        }

        // Nivel de prioridad del cliente
        public int Prioridad
        {
            get { return (int)Tipo; }
        }

        public override string ToString()
        {
            return string.Format(
                "Nombre   : {0}\nTipo     : {1}\nProductos: {2}",
                Nombre, Tipo, NumeroProductos);
        }
    }
}
