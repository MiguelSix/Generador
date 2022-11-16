/*Olvera Morales Miguel Angel*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        public Lenguaje(string nombre) : base(nombre)
        {
        }
        public Lenguaje()
        {
        }
        public void Dispose()
        {
            Console.WriteLine("\n\nDestructor ejecutado exitosamente");
            cerrar();
        }
        public void gramatica()
        {
            cabecera();
            listaDeProducciones();
        }
        private void cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.SNT);
            match(Tipos.FinProduccion);
        }
        private void listaDeProducciones()
        {
            match(Tipos.SNT);
            match(Tipos.Produce);
            match(Tipos.FinProduccion);
            if (!FinArchivo())
            {
                listaDeProducciones();
            }
        }
    }
}