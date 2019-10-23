using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using ControlFirmaElectronica.NotificacionElectronica;
using System.Security.Cryptography;
using System.Text;

namespace ControlFirmaElectronica
{
    static class Program
    {
        public static int intOpcion =0;
        public static string strCentro = "";
        public static ContratoNotifiacionElectronica ne = new ContratoNotifiacionElectronicaClient();
        public static Acuses[] resolRp { get; set; }
        public static Verificador veri = new Verificador();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {           
            CargarValoresN();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            if (intOpcion == 7)
            {
                        
                Cuenta cue = new Cuenta();

                cue.Nick = strCentro;// "102110201";
                cue.Password = Md5Hash(strCentro);
                cue.CentroTrabajo = strCentro;
                EstadoLogeo edo = ne.IniciarSesion(cue);
                veri.ClaveCentro = edo.TicketSesion.Centro.Clave;
                veri.ClavePlataforma = edo.TicketSesion.Plataforma.ClavePlataforma;
                veri.TOKEN = edo.SesionInformacion.Token;


               // clsAcuerdos Acuerdos = new clsAcuerdos();
                Acuses[] Resoluciones;
                Resoluciones = ObtenerAlerta(strCentro);

                if (Resoluciones.Length > 0)
                {
                    Application.Run(new wfrAlerta(Resoluciones));
                }
                else
                {
                    MessageBox.Show("No se encontraron Resoluciones del RPP", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
               
            }
            else
            {
                Application.Run(new wfFirma());
            }
        }

        public static Acuses[] ObtenerAlerta(string CentroTrabajo)
        {

            resolRp = ne.ObtenerResolucionesSISCONEXPE(veri, CentroTrabajo);

            return resolRp;
        }
        public static string Md5Hash(string input)
        {
            // Creamos una nueva instancias
            MD5 md5Hasher = MD5.Create();

            // le sacamos los byte a la cadea
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            //Creamos un string builder para aterrizar la cadena
            StringBuilder sBuilder = new StringBuilder();

            // recorremos byte por byte hasta que se transforme toda en una cadena hex
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // la regresamos
            return sBuilder.ToString();
        }

        public static void CargarValoresN()
        {
            DataSet xmlParamentros = new DataSet();
            xmlParamentros.ReadXml(Application.StartupPath + "\\parFirma.xml");
            intOpcion = int.Parse(xmlParamentros.Tables[0].Rows[0]["Opcion"].ToString());
            strCentro = xmlParamentros.Tables[0].Rows[0]["Centro"].ToString();
            xmlParamentros.Dispose();
        }


    }
}
