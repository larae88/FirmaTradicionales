using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ControlFirmaElectronica
{
    class clsConexionAPI
    {
        // SE CREA EL CLIENTE HTTP PARA HACER LA PETICIÓN 
        static HttpClient client = new HttpClient();

        // ESTRUCTURA DE LOS METODOS PARA LLAMAR AL API (PUEDEN O NO SER STATICS), SIEMPRE SON ASYNC PARA MANEJAR LAS TAREAS ASYNCRONAS
        // ESPERA UNA TAREA DE UN TIPO DEFINIDO EN LOS < TIPO ESPERADO >
        static async Task<string[]> GetTransacciones()
        {

            // OBJETO DE RESPUESTA
            string[] product = null;

            // LLAMADA TIPO GET A API
            HttpResponseMessage response = await client.GetAsync("https://archivoelectronico.poderjudicial-gto.gob.mx/SV_P3/api/Archivo/ObtenerTransacciones");

            // SE EVALUA LA RESPUESTA, SI ES EXITOSA, SE PROCESA, DE LO CONTRARIO SE MANEJA COMO ERROR, LA RESPUESTA PUEDE SER DE CUALQUIER TIPO DE RESPUESTA HTTP (400, 404, 302, 500, ETC)
            if (response.IsSuccessStatusCode)
            {

                // LEE LA RESPUESTA

                product = await response.Content.ReadAsAsync<string[]>();
            }

            // RETORNA
            return product;
        }

        // IGUAL QUE EL ANTERIOR SOLO QUE ESTE METODO ES POST, RECIBE UN OBJETO COMO PARAMETRO
        static async Task<bool> ArchivoNodoBorrarSet(string product)
        {
            // OBJETO DE RESPUESTA
            bool bandera = false;

            // LLAMADA TIPO POST

            HttpResponseMessage response = await client.PostAsJsonAsync("https://archivoelectronico.poderjudicial-gto.gob.mx/SV_P3/api/Archivo/ArchivoNodoBorrarSet", product);

            if (response.IsSuccessStatusCode)
            {
                bandera = await response.Content.ReadAsAsync<bool>();
            }
            return bandera;
        }

        // LOS METODOS QUE HAGAN USO DE LAS LLAMADAS API, DEBERAN SER ASYNCRONOS Y UTILIZAR EL METODO AWAIT
        private async void llamar()
        {
            string[] product = await GetTransacciones();
        }
    }
}
