/*Olvera Morales Miguel Angel*/
using System;
namespace Semantica
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje lenguaje = new Lenguaje())
                {
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}