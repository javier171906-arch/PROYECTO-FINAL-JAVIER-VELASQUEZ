using System;
using System.Collections.Generic;
using DonJuanMarket.Clases;

namespace DonJuanMarket.EstructurasDatos
{
    // NodoAVL
    public class NodoAVL
    {
        public Producto Dato { get; set; }
        public NodoAVL Izquierdo { get; set; }
        public NodoAVL Derecho { get; set; }
        public int Altura { get; set; }

        public NodoAVL(Producto dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
            // Un nodo recién creado, su altura es 1.
            Altura = 1;
        }
    }

    // ArbolAVL
    public class ArbolAVL
    {
        private NodoAVL raiz;

        // Altura de un nodo
        private int Altura(NodoAVL nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        // Factor de balance 
        private int FactorBalance(NodoAVL nodo)
        {
            return nodo == null ? 0 : Altura(nodo.Izquierdo) - Altura(nodo.Derecho);
        }

        // Recalcula la altura de un nodo a partir de la de sus hijos
        private void ActualizarAltura(NodoAVL nodo)
        {
            nodo.Altura = 1 + Math.Max(Altura(nodo.Izquierdo), Altura(nodo.Derecho));
        }

        // Rotaciones
        private NodoAVL RotarDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierdo;
            NodoAVL t2 = x.Derecho;

            // Se realiza la rotación.
            x.Derecho = y;
            y.Izquierdo = t2;

            ActualizarAltura(y);
            ActualizarAltura(x);

            return x;
        }

        // Rotación simple a la izquierda
        private NodoAVL RotarIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecho;
            NodoAVL t2 = y.Izquierdo;

            x.Derecho = t2;
            y.Izquierdo = x;

            ActualizarAltura(x);
            ActualizarAltura(y);

            return y;
        }

        // Revisa el factor de balance de un nodo y aplica la rotación
        private NodoAVL Balancear(NodoAVL nodo)
        {
            ActualizarAltura(nodo);
            int fb = FactorBalance(nodo);

            // Caso izquierda-izquierda
            if (fb > 1 && FactorBalance(nodo.Izquierdo) >= 0)
            {
                return RotarDerecha(nodo);
            }

            // Caso izquierda-derecha
            if (fb > 1 && FactorBalance(nodo.Izquierdo) < 0)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            // Caso derecha-derecha
            if (fb < -1 && FactorBalance(nodo.Derecho) <= 0)
            {
                return RotarIzquierda(nodo);
            }

            // Caso derecha-izquierda:
            if (fb < -1 && FactorBalance(nodo.Derecho) > 0)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            // El nodo ya está balanceado
            return nodo;
        }

        // Insertar
        public bool Insertar(Producto producto)
        {
            bool insertado;
            raiz = InsertarRec(raiz, producto, out insertado);
            return insertado;
        }

        private NodoAVL InsertarRec(NodoAVL nodo, Producto producto, out bool insertado)
        {
            if (nodo == null)
            {
                insertado = true;
                return new NodoAVL(producto);
            }

            int comparacion = string.CompareOrdinal(producto.Codigo, nodo.Dato.Codigo);

            if (comparacion < 0)
            {
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, producto, out insertado);
            }
            else if (comparacion > 0)
            {
                nodo.Derecho = InsertarRec(nodo.Derecho, producto, out insertado);
            }
            else
            {
                // Código duplicado
                insertado = false;
                return nodo;
            }

            // Se rebalancea el nodo
            return Balancear(nodo);
        }

        // Buscar
        public Producto Buscar(string codigo)
        {
            NodoAVL actual = raiz;
            string clave = codigo.Trim().ToUpper();

            while (actual != null)
            {
                int comparacion = string.CompareOrdinal(clave, actual.Dato.Codigo);
                if (comparacion == 0)
                    return actual.Dato;

                actual = comparacion < 0 ? actual.Izquierdo : actual.Derecho;
            }

            return null;
        }

        // Actualizar
        public bool Actualizar(string codigo, string nuevoNombre, string nuevaCategoria, double nuevoPrecio, int nuevoStock)
        {
            Producto producto = Buscar(codigo);
            if (producto == null)
                return false;

            producto.ActualizarDatos(nuevoNombre, nuevaCategoria, nuevoPrecio, nuevoStock);
            return true;
        }

        // Eliminar
        public bool Eliminar(string codigo)
        {
            bool eliminado;
            raiz = EliminarRec(raiz, codigo.Trim().ToUpper(), out eliminado);
            return eliminado;
        }

        private NodoAVL EliminarRec(NodoAVL nodo, string codigo, out bool eliminado)
        {
            if (nodo == null)
            {
                eliminado = false;
                return null;
            }

            int comparacion = string.CompareOrdinal(codigo, nodo.Dato.Codigo);

            if (comparacion < 0)
            {
                nodo.Izquierdo = EliminarRec(nodo.Izquierdo, codigo, out eliminado);
            }
            else if (comparacion > 0)
            {
                nodo.Derecho = EliminarRec(nodo.Derecho, codigo, out eliminado);
            }
            else
            {
                // Se encontró el nodo a eliminar
                eliminado = true;

                // Caso 1: nodo hoja (sin hijos) -> se elimina directamente.

                // Caso 2: nodo con un solo hijo -> el hijo lo reemplaza.
                if (nodo.Izquierdo == null || nodo.Derecho == null)
                {
                    NodoAVL hijo = (nodo.Izquierdo != null) ? nodo.Izquierdo : nodo.Derecho;
                    return hijo;
                }

                // Caso 3: nodo con dos hijos -> se reemplaza con su sucesor in-orden
                NodoAVL sucesor = NodoMinimo(nodo.Derecho);
                nodo.Dato = sucesor.Dato;
                bool descartar;
                nodo.Derecho = EliminarRec(nodo.Derecho, sucesor.Dato.Codigo, out descartar);
            }

            // Se rebalancea el nodo
            return Balancear(nodo);
        }

        // Nodo con el código menor dentro de un subárbol
        private NodoAVL NodoMinimo(NodoAVL nodo)
        {
            NodoAVL actual = nodo;
            while (actual.Izquierdo != null)
                actual = actual.Izquierdo;
            return actual;
        }

        // Recorrido In-Orden
        public List<Producto> RecorridoInOrden()
        {
            List<Producto> resultado = new List<Producto>();
            RecorridoInOrdenRec(raiz, resultado);
            return resultado;
        }

        private void RecorridoInOrdenRec(NodoAVL nodo, List<Producto> resultado)
        {
            if (nodo == null)
                return;

            RecorridoInOrdenRec(nodo.Izquierdo, resultado);
            resultado.Add(nodo.Dato);
            RecorridoInOrdenRec(nodo.Derecho, resultado);
        }

        // Estadísticas
        public bool EstaVacio
        {
            get { return raiz == null; }
        }

        public int ContarProductos()
        {
            return ContarProductosRec(raiz);
        }

        private int ContarProductosRec(NodoAVL nodo)
        {
            if (nodo == null)
                return 0;
            return 1 + ContarProductosRec(nodo.Izquierdo) + ContarProductosRec(nodo.Derecho);
        }

        public int StockTotal()
        {
            int total = 0;
            foreach (Producto p in RecorridoInOrden())
                total += p.Stock;
            return total;
        }

        // Altura actual del árbol
        public int AlturaArbol()
        {
            return Altura(raiz);
        }
    }

    // ColaPrioridad
    public class ColaPrioridad
    {
        private readonly List<Cliente> heap;
        private int siguienteNumeroLlegada;

        public ColaPrioridad()
        {
            heap = new List<Cliente>();
            siguienteNumeroLlegada = 1;
        }

        public int Cantidad
        {
            get { return heap.Count; }
        }

        public bool EstaVacia
        {
            get { return heap.Count == 0; }
        }

        // Comparación de prioridad
        private bool TienePrioridad(Cliente a, Cliente b)
        {
            if (a.Prioridad != b.Prioridad)
                return a.Prioridad < b.Prioridad;

            return a.NumeroLlegada < b.NumeroLlegada;
        }

        private int Padre(int i) { return (i - 1) / 2; }
        private int HijoIzquierdo(int i) { return 2 * i + 1; }
        private int HijoDerecho(int i) { return 2 * i + 2; }

        private void Intercambiar(int i, int j)
        {
            Cliente temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        // Agregar (bubble up)
        public int Agregar(Cliente cliente)
        {
            cliente.NumeroLlegada = siguienteNumeroLlegada;
            siguienteNumeroLlegada++;

            heap.Add(cliente);
            int posicion = heap.Count - 1;
            BubbleUp(posicion);

            return PosicionEnOrdenDeAtencion(cliente);
        }

        private void BubbleUp(int i)
        {
            while (i > 0)
            {
                int padre = Padre(i);
                if (TienePrioridad(heap[i], heap[padre]))
                {
                    Intercambiar(i, padre);
                    i = padre;
                }
                else
                {
                    break; // Ya está en su lugar correcto.
                }
            }
        }

        // Atender (bubble down)
        public Cliente Atender()
        {
            if (EstaVacia)
                return null;

            Cliente raiz = heap[0];
            int ultimo = heap.Count - 1;

            heap[0] = heap[ultimo];
            heap.RemoveAt(ultimo);

            if (heap.Count > 0)
                BubbleDown(0);

            return raiz;
        }

        private void BubbleDown(int i)
        {
            int mejor = i;

            while (true)
            {
                int izq = HijoIzquierdo(mejor);
                int der = HijoDerecho(mejor);
                int candidato = mejor;

                if (izq < heap.Count && TienePrioridad(heap[izq], heap[candidato]))
                    candidato = izq;

                if (der < heap.Count && TienePrioridad(heap[der], heap[candidato]))
                    candidato = der;

                if (candidato == mejor)
                    break;

                Intercambiar(mejor, candidato);
                mejor = candidato;
            }
        }

        // Consulta al siguiente cliente a atender
        public Cliente VerSiguiente()
        {
            return EstaVacia ? null : heap[0];
        }

        // Consulta de la cola completa
        public List<Cliente> ObtenerClientesEnEspera()
        {
            List<Cliente> copia = new List<Cliente>(heap);
            copia.Sort(delegate (Cliente a, Cliente b)
            {
                if (a.Prioridad != b.Prioridad)
                    return a.Prioridad.CompareTo(b.Prioridad);
                return a.NumeroLlegada.CompareTo(b.NumeroLlegada);
            });
            return copia;
        }

        private int PosicionEnOrdenDeAtencion(Cliente cliente)
        {
            List<Cliente> ordenados = ObtenerClientesEnEspera();
            return ordenados.IndexOf(cliente) + 1;
        }
    }
}
