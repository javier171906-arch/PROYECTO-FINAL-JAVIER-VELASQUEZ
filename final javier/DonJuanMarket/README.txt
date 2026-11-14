SISTEMA DE GESTIÓN DE SUPERMERCADO - "DON JUAN MARKET"
========================================================

Universidad Regional de Guatemala
Administración de Sistemas Informáticos
Procesos y Algoritmos II - Proyecto Final

--------------------------------------------------------
REQUISITOS
--------------------------------------------------------
- .NET SDK 9.0 o superior instalado.
  Se puede verificar con:  dotnet --version

--------------------------------------------------------
ESTRUCTURA DEL PROYECTO (3 archivos de código)
--------------------------------------------------------
DonJuanMarket/
  DonJuanMarket.csproj     Archivo de proyecto (.NET 9)
  Modelos.cs                Producto, Cliente y el enum TipoCliente
  EstructurasDatos.cs         NodoAVL, ArbolAVL y ColaPrioridad (Heap)
  Program.cs                   Supermercado, Menu (interfaz de consola) y
                                 Program (punto de entrada / Main)

Aunque el código vive en solo 3 archivos, internamente sigue
organizado por namespaces (DonJuanMarket.Modelos,
DonJuanMarket.EstructurasDatos, DonJuanMarket.Sistema,
DonJuanMarket.Interfaz) para mantener la separación de
responsabilidades entre las clases pedidas en el enunciado:
Producto, NodoAVL, ArbolAVL, Cliente, ColaPrioridad,
Supermercado y Menu.

--------------------------------------------------------
CÓMO COMPILAR
--------------------------------------------------------
Desde la carpeta que contiene este archivo (donde está
DonJuanMarket.csproj), ejecutar en una terminal:

    dotnet build

Esto genera el ejecutable dentro de bin/Debug/net9.0/.

--------------------------------------------------------
CÓMO EJECUTAR
--------------------------------------------------------
Desde la misma carpeta, ejecutar:

    dotnet run

También se puede generar un ejecutable independiente con:

    dotnet publish -c Release -r <RID> --self-contained true

(por ejemplo, -r win-x64, -r linux-x64 o -r osx-x64 según el
sistema operativo donde se vaya a ejecutar).

--------------------------------------------------------
USO DEL PROGRAMA
--------------------------------------------------------
Al ejecutar el programa aparece un menú con 10 opciones,
divididas en 4 secciones: Inventario (Árbol AVL), Atención
de clientes (Heap), Reportes y Sistema. Se navega escribiendo
el número de la opción deseada y presionando ENTER. El uso
detallado de cada opción se explica en el Manual de Usuario
que acompaña esta entrega.

--------------------------------------------------------
NOTAS DE DISEÑO
--------------------------------------------------------
- El Árbol AVL y la Cola de Prioridad (Heap) están
  implementados desde cero, sin librerías externas, tal como
  lo exige el enunciado.
- El código del producto es la clave de ordenamiento del
  Árbol AVL; es inmutable una vez registrado el producto.
- La Cola de Prioridad usa un único heap (no colas separadas
  por tipo de cliente). La prioridad se determina primero por
  el tipo de cliente (VIP=1, ADULTO_MAYOR=2, REGULAR=3) y, en
  caso de empate, por un número de llegada asignado de forma
  automática y creciente, que garantiza el orden FIFO incluso
  si dos clientes se registran en el mismo instante.
- La lógica de ambas estructuras (rotaciones del AVL, orden
  de atención del heap) fue probada con un conjunto de
  pruebas automatizadas antes de la entrega, así como una
  sesión completa del menú interactivo.

Repositorio en GitHub: <pendiente de agregar por el estudiante>
