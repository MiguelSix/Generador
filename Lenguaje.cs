/*Olvera Morales Miguel Angel*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Requerimiento 1: Construir un metodo para escribir en el archivo Lenguaje.cs intentando el codigo
//                 "{" incrementa un tabulador, "}" decrementa un tabulador
//Requerimiento 2: Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la primera produccion de la gramatica

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        string nombreProyecto;
        public Lenguaje(string nombre) : base(nombre)
        {
            nombreProyecto = " ";
        }
        public Lenguaje()
        {
            nombreProyecto = " ";
        }
        public void Dispose()
        {
            Console.WriteLine("\n\nDestructor ejecutado exitosamente");
            cerrar();
        }
        private void Programa(string proyecto, string produccionPrincipal)
        {
            programa.WriteLine("using System;");
            programa.WriteLine("namespace " + proyecto);
            programa.WriteLine("{");
            programa.WriteLine("    public class Program");
            programa.WriteLine("    {");
            programa.WriteLine("        static void Main(string[] args)");
            programa.WriteLine("        {");
            programa.WriteLine("            try");
            programa.WriteLine("            {");
            programa.WriteLine("                using (Lenguaje a = new Lenguaje())");
            programa.WriteLine("                {");
            programa.WriteLine("                    a." + produccionPrincipal.ToLower() + "();");
            programa.WriteLine("                }");
            programa.WriteLine("            }");
            programa.WriteLine("            catch (Exception e)");
            programa.WriteLine("            {");
            programa.WriteLine("                Console.WriteLine(e.Message);");
            programa.WriteLine("            }");
            programa.WriteLine("        }");
            programa.WriteLine("    }");
            programa.WriteLine("}");
        }
        public void gramatica()
        {
            cabecera();
            Programa(nombreProyecto, "programa");
            cabeceraLenguaje(nombreProyecto);
            listaDeProducciones();
            lenguaje.WriteLine("\t}");
            lenguaje.WriteLine("}");
        }
        private void cabecera()
        {
            match("Gramatica");
            match(":");
            nombreProyecto = getContenido();
            match(Tipos.SNT);
            match(Tipos.FinProduccion);
        }
        private void cabeceraLenguaje(string proyecto)
        {
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("using System.Linq;");
            lenguaje.WriteLine("using System.Threading.Tasks;");
            lenguaje.WriteLine("");
            lenguaje.WriteLine("namespace " + proyecto);
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("    public class Lenguaje : Sintaxis, IDisposable");
            lenguaje.WriteLine("    {");
            lenguaje.WriteLine("        string nombreProyecto;");
            lenguaje.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            nombreProyecto = \" \";");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("        public Lenguaje()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            nombreProyecto = \" \";");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("        public void Dispose()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            cerrar();");
            lenguaje.WriteLine("        }");
        }
        private void listaDeProducciones()
        {
            lenguaje.WriteLine("\t\tprivate void " + getContenido() + "()");
            lenguaje.WriteLine("\t\t{");
            match(Tipos.SNT);
            match(Tipos.Produce);
            match(Tipos.FinProduccion);
            lenguaje.WriteLine("\t\t}");
            if (!FinArchivo())
            {
                listaDeProducciones();
            }
        }
    }
}